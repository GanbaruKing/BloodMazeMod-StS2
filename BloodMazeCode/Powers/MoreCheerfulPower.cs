using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Powers;


public class MoreCheerfulPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private CombatSide _currentSide = CombatSide.Player;
    
    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        _currentSide = side;
        return Task.CompletedTask;
    }

    public override async Task  AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (delta < 0) return;
        if (creature != this.Owner) return;
        if (CombatSide.Player != _currentSide) return;
        await PowerCmd.Apply<StrengthPower>(
            this.Owner, this.Amount, this.Owner, null);
        
    }
}