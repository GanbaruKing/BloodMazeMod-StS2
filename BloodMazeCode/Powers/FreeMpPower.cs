using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BloodMaze.BloodMazeCode.Powers;


public class FreeMpPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner || cardPlay.Card.Type != CardType.Attack) return;
        var pileType = cardPlay.Card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play) return;
        if (cardPlay.Card is not MpConsumeCard) return;

        if (cardPlay.Card is MpConsumeCard mpCard)
            mpCard.IsFreeThisPlay = true;
        
    }
    
}