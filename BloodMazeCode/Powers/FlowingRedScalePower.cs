using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;

public sealed class FlowingRedScalePower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private const decimal HpLoss = 3m;
    private const int DrawAmount = 1;
    private const decimal PlatedArmorAmount = 2m;
    

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != this.Owner) return;
        
        await CreatureCmd.Damage(choiceContext, this.Owner, HpLoss,
            ValueProp.Unblockable | ValueProp.Unpowered,
            (Creature)null!, (CardModel)null!);
        
        await CardPileCmd.Draw(choiceContext, DrawAmount, player);
        
        await PowerCmd.Apply<StrengthPower>(
            choiceContext, this.Owner, this.Amount, this.Owner, null);
        
        await PowerCmd.Apply<PlatingPower>(
            choiceContext, this.Owner, PlatedArmorAmount, this.Owner, null);
    }
}
