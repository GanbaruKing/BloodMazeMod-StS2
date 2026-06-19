
#nullable enable
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Powers;



public sealed class TimeLimitPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
    {
        TimeLimitPower timeLimitPower = this;
        if (side != timeLimitPower.Owner.Side)
            return;
        timeLimitPower.Flash();
        StrengthPower strengthPower = (await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), timeLimitPower.Owner, (Decimal) timeLimitPower.Amount, timeLimitPower.Owner, (CardModel) null!))!;
        await PowerCmd.Apply<HemorrhagePower>(new ThrowingPlayerChoiceContext(), this.Owner, 1m, this.Owner, null);
    }
}
