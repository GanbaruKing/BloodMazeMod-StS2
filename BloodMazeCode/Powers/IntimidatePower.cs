using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Powers;


public class IntimidatePower : BloodMazePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power != this) return;
        if (amount <= 0) return;

        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, -amount, applier, cardSource);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
    {
        if (side != this.Owner.Side) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;

        decimal amount = this.Amount;
        await PowerCmd.Remove<IntimidatePower>(this.Owner);
        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, amount, this.Owner, null);
    }
}
