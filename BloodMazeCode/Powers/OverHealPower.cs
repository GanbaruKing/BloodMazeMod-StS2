using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace BloodMaze.BloodMazeCode.Powers;


public class OverHealPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public int SavedMaxHp { get; set; }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        int clampedHp = Math.Min(this.Owner.Player!.Creature.CurrentHp, SavedMaxHp);
        await CreatureCmd.SetMaxHp(this.Owner.Player.Creature, (decimal)SavedMaxHp);
        await CreatureCmd.SetCurrentHp(this.Owner.Player.Creature, (decimal)clampedHp);
    }
}