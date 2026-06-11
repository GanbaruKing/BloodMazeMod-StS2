using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Powers;

public class SilentCastPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public bool CanAffect(CardModel card)
    {
        return !_usedThisTurn && IsTarget(card);
    }
    private bool _usedThisTurn;

    private static bool IsTarget(CardModel card)
    {
        return card is MpConsumeCard
               && card.Type == CardType.Attack;
    }

    public override Task AfterPlayerTurnStartEarly(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player.Creature == this.Owner)
            _usedThisTurn = false;

        return Task.CompletedTask;
    }

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal currentCost,
        out decimal modifiedCost)
    {
        modifiedCost = currentCost;

        if (_usedThisTurn)
            return false;

        if (!IsTarget(card))
            return false;

        modifiedCost = 0m;
        return true;
    }

    public override Task BeforeCardPlayed(CardPlay play)
    {
        if (_usedThisTurn)
            return Task.CompletedTask;

        if (!IsTarget(play.Card))
            return Task.CompletedTask;

        if (play.Card is MpConsumeCard mpCard)
            mpCard.IsFreeThisPlay = true;

        _usedThisTurn = true;

        return Task.CompletedTask;
    }
}