using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Cards.Variables;
using BloodMaze.BloodMazeCode.Mp;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace BloodMaze.BloodMazeCode.Relics;

public class ManaOrb : BloodMazeRelic
{
    public const int InitialMaxMp = 45;
    public const int CombatEndRestore = 7;

    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<ManaOrb>("CurrentMp", (_) => MpSaveData.CurrentMp.ToString()),
        new DisplayVar<ManaOrb>("MaxMp", (_) => MpSaveData.MaxMp.ToString()),
    ];

    public override Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (MpSaveData.RestSiteHealed) return Task.CompletedTask;
        if (MpSaveData.RestSiteEnteredMp == null) return Task.CompletedTask;
        int cap = MpSaveData.RestSiteEnteredMp.Value + 15;
        int toRestore = cap - MpSaveData.CurrentMp;
        if (toRestore > 0)
            MpSaveData.Restore(toRestore);
        MpSaveData.SetRestSiteHealed();
        return Task.CompletedTask;
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        MpSaveData.Load();
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);

        if (room is RestSiteRoom && !MpSaveData.RestSiteHealed && MpSaveData.RestSiteEnteredMp == null)
            MpSaveData.SaveRestSiteEntered();
        else if (room is not RestSiteRoom)
            MpSaveData.ClearRestSiteEntered();
    }

    public override async Task BeforeCombatStart()
    {
        MpSaveData.Load();
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);

        MpSaveData.SaveCombatStart();
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        MpSaveData.ClearCombatStart();

        if (room.RoomType == RoomType.Boss)
            MpSaveData.Initialize(MpSaveData.MaxMp);
        else
            MpSaveData.Restore(CombatEndRestore);
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature.IsPlayer)
            MpSaveData.Delete();
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