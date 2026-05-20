using BaseLib.Cards.Variables;
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

    // 最初の部屋に入った時点で初期化
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);
    }

    // ラン開始時にMPをロードまたは初期化
    public override async Task BeforeCombatStart()
    {
        if (MpSaveData.MaxMp == 0)
            MpSaveData.Initialize(InitialMaxMp);
        else
            MpSaveData.Load();
    }

    // 戦闘終了時：ボスなら全回復、それ以外は+5回復
    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (room.RoomType == RoomType.Boss)
            MpSaveData.Initialize(MpSaveData.MaxMp);
        else
            MpSaveData.Restore(CombatEndRestore);
    }

    // 敗北時にセーブデータ削除
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner?.Creature?.CurrentHp <= 0)
            MpSaveData.Delete();
    }
}