using System;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Powers;

public class FreeMpAttackPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(CardModel card, Decimal originalCost, out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != this.Owner || card.Type != CardType.Attack) return false;
        if (card is not MpConsumeCard) return false;
        if (this.Amount <= 0) return false;
        var pileType = card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play) return false;
        modifiedCost = 0m;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner || cardPlay.Card.Type != CardType.Attack) return;
        var pileType = cardPlay.Card.Pile?.Type;
        if (pileType != PileType.Hand && pileType != PileType.Play) return;
        if (cardPlay.Card is not MpConsumeCard) return;

        if (cardPlay.Card is MpConsumeCard mpCard)
            mpCard.IsFreeThisPlay = true;

        await PowerCmd.Decrement(this);
    }
}