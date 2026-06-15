using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;


public sealed class VampireFormPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != this.Owner) return;
        await CreatureCmd.Damage(choiceContext, this.Owner, 5m,
            ValueProp.Unblockable | ValueProp.Unpowered,
            (Creature)null!, (CardModel)null!);
    }
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner) return;
        if (cardPlay.Card.Type != CardType.Attack) return;
        if (cardPlay.Card is not MpConsumeCard mpCard) return;

        mpCard.IsVampireForm = true;
    }
}