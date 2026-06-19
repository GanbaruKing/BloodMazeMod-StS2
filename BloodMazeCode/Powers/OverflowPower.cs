using System.Threading.Tasks;
using System.Collections.Generic;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;


namespace BloodMaze.BloodMazeCode.Powers;



public sealed class OverflowPower : BloodMazePower
{

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int ModifyCardPlayCount(CardModel cardModel, Creature? creature, int playCount)
    {
        if (cardModel is MpConsumeCard)
            return playCount + 1;
        return playCount;
    }
    
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
    {
        if (side == this.Owner.Side)
        {
            return PowerCmd.ModifyAmount(choiceContext, this, -1m, null, null);
        }
        return Task.CompletedTask;
    }
}
