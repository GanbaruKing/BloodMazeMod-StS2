using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;

public class HemorrhagePower : BloodMazePower
{
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerType Type => PowerType.Debuff;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != this.Owner.Side) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;
        
        await PowerCmd.Apply<HemorrhagePower>(this.Owner, 1m, this.Owner, null);
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;
        
        await CreatureCmd.Damage(choiceContext, this.Owner, this.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power != this) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;
        if (this.Amount < 10) return;
        
        decimal remainder = this.Amount - 10;
        
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), this.Owner, 50m,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await CreatureCmd.Stun(this.Owner);
        await PowerCmd.Remove<HemorrhagePower>(this.Owner);
        
        if (remainder > 0)
            await PowerCmd.SetAmount<HemorrhagePower>(this.Owner, remainder, applier, cardSource);
    }
}