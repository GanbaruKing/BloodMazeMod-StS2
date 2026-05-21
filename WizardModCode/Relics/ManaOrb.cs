using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using WizardMod.WizardModCode.Mp;

namespace WizardMod.WizardModCode.Relics;

public class ManaOrb : WizardModRelic
{
    public const int InitialMaxMp = 50;
    public const int CombatEndRestore = 5;

    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<ManaOrb>("CurrentMp", (_) => MpSaveData.CurrentMp.ToString()),
        new DisplayVar<ManaOrb>("MaxMp", (_) => MpSaveData.MaxMp.ToString()),
    ];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);
    }

    public override async Task BeforeCombatStart()
    {
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);
        else
            MpSaveData.Load();

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

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner?.Creature?.CurrentHp <= 0)
            MpSaveData.Delete();
    }
    
}