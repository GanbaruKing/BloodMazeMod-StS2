using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BloodMaze.BloodMazeCode.Powers;


public class LinkPower : BloodMazePower
{
    private CombatSide _currentSide = CombatSide.Player;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
    {
        _currentSide = side;
        return Task.CompletedTask;
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (delta >= 0) return;
        if (_currentSide != CombatSide.Player) return;
        if (creature != Owner.Player!.Creature) return;
        if (Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;

        await PowerCmd.Apply<HemorrhagePower>(
            new ThrowingPlayerChoiceContext(),
            this.Owner.CombatState!.HittableEnemies,
            (decimal)this.Amount,
            this.Owner,
            null
        );
    }
}
