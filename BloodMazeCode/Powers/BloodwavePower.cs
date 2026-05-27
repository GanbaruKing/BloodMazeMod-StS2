using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;


public class BloodwavePower : BloodMazePower
{
    private CombatSide _currentSide = CombatSide.Player;
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        _currentSide = side;
        return Task.CompletedTask;
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        BloodwavePower bloodwavePower = this;
        
        if (_currentSide != CombatSide.Player) return;
        if (creature != Owner.Player!.Creature) return;
        if (delta == 0) return;
        
        await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>)creature.CombatState!.HittableEnemies, bloodwavePower.Amount ,ValueProp.Unpowered, this.Owner, null);
    }
}