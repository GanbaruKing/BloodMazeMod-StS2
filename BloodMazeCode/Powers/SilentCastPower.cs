using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BloodMaze.BloodMazeCode.Powers;



public class SilentCastPower : FreeMpAttackPower 
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != this.Owner) return Task.CompletedTask;
        return PowerCmd.ModifyAmount(this, 1m, null, null);
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
        if (this.Amount <= 0) return Task.CompletedTask;
        return PowerCmd.ModifyAmount(this, -1m, null, null);
    }
}