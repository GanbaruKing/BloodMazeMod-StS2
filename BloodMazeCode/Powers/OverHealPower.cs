using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace BloodMaze.BloodMazeCode.Powers;


public class OverHealPower : BloodMazePower
{
    public const int TemporaryMaxHp = 999;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public int SavedMaxHp { get; set; }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        int maxHpGainDuringCombat = Math.Max(0, this.Owner.Player!.Creature.MaxHp - TemporaryMaxHp);
        int restoredMaxHp = SavedMaxHp + maxHpGainDuringCombat;
        int clampedHp = Math.Min(this.Owner.Player.Creature.CurrentHp, restoredMaxHp);
        await CreatureCmd.SetMaxHp(this.Owner.Player.Creature, (decimal)restoredMaxHp);
        await CreatureCmd.SetCurrentHp(this.Owner.Player.Creature, (decimal)clampedHp);
    }
}
