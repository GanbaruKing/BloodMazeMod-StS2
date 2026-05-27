using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace BloodMaze.BloodMazeCode.Powers;


public sealed class MagicalSpringPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this),
    ];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != this.Owner.Player) return amount;
        return amount + this.Amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != this.Owner) return;
        MpSaveData.TryConsume(1);
    }
}