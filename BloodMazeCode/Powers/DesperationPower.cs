using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;


public sealed class DesperationPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult results,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (target != this.Owner) return;
        if (dealer == null || dealer.IsDead) return;
        if (results.UnblockedDamage <= 0) return;

        await CreatureCmd.Damage(choiceContext, dealer, results.UnblockedDamage,
            ValueProp.Unblockable | ValueProp.Unpowered, this.Owner, null);
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side)
        {
            return PowerCmd.ModifyAmount(this, -1m, null, null);
        }
        return Task.CompletedTask;
    }
}