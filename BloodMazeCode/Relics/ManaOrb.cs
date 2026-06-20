using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Cards.Variables;
using BloodMaze.BloodMazeCode.Mp;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace BloodMaze.BloodMazeCode.Relics;

public class ManaOrb : BloodMazeRelic
{
    public const int InitialMaxMp = 65;
    private const decimal WearyTravelerInitialMpRate = 0.8m;
    protected virtual int CombatEndRestore => 9;

    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<AncientManaOrb>();
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<ManaOrb>("CurrentMp", (_) => MpSaveData.CurrentMp.ToString()),
        new DisplayVar<ManaOrb>("MaxMp", (_) => MpSaveData.MaxMp.ToString()),
    ];

    public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (MpSaveData.RestSiteHealed) return;
        if (MpSaveData.RestSiteEnteredMp == null) return;
        MpSaveData.MaxMp += 2;
        int restoreAmount = (int)(MpSaveData.MaxMp * 0.3m);
        int cap = MpSaveData.RestSiteEnteredMp.Value + restoreAmount;
        int toRestore = cap - MpSaveData.CurrentMp;
        if (toRestore > 0)
            MpSaveData.Restore(toRestore);
        MpSaveData.SetRestSiteHealed();
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        MpSaveData.Load();
        if (MpSaveData.MaxMp == 0)
            InitializeStartingMp();

        if (room is RestSiteRoom && !MpSaveData.RestSiteHealed && MpSaveData.RestSiteEnteredMp == null)
            MpSaveData.SaveRestSiteEntered();
        else if (room is not RestSiteRoom)
            MpSaveData.ClearRestSiteEntered();
    }

    public override async Task BeforeCombatStart()
    {
        MpSaveData.Load();
        if (MpSaveData.MaxMp == 0)
            InitializeStartingMp();

        MpSaveData.SaveCombatStart();
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        MpSaveData.ClearCombatStart();

        if (room.RoomType == RoomType.Boss)
            return;

        int overflow = RestoreAndGetOverflow(CombatEndRestore);
        if (overflow > 0 && !Owner.Creature.IsDead)
            await CreatureCmd.Heal(Owner.Creature, overflow);
    }

    public override Task AfterActEntered()
    {
        MpSaveData.Load();
        if (MpSaveData.MaxMp == 0)
        {
            InitializeStartingMp();
            return Task.CompletedTask;
        }
        else if ((int)Owner.RunState.AscensionLevel >= (int)AscensionLevel.WearyTraveler)
        {
            MpSaveData.ApplyActEnteredRecovery(Owner.RunState.CurrentActIndex, 0.8m);
        }
        else
        {
            MpSaveData.Initialize(MpSaveData.MaxMp);
        }

        return Task.CompletedTask;
    }

    private void InitializeStartingMp()
    {
        int currentMp = IsWearyTravelerOrHigher()
            ? (int)(InitialMaxMp * WearyTravelerInitialMpRate)
            : InitialMaxMp;

        MpSaveData.Initialize(InitialMaxMp, currentMp, Owner.RunState.CurrentActIndex);
    }

    private bool IsWearyTravelerOrHigher()
    {
        return (int)Owner.RunState.AscensionLevel >= (int)AscensionLevel.WearyTraveler;
    }
    
    private int RestoreAndGetOverflow(int amount)
    {
        int before = MpSaveData.CurrentMp;
        MpSaveData.Restore(amount);
        int restored = MpSaveData.CurrentMp - before;
        return amount - restored;
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        return Task.CompletedTask;
    }

    [HarmonyPatch(typeof(NAbandonRunConfirmPopup), "OnYesButtonPressed")]
    public static class AbandonRunPatch
    {
        public static void Postfix()
        {
            MpSaveData.Delete();
        }
    }
}
