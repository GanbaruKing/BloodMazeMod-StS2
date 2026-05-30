using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BloodMaze.BloodMazeCode.Powers;


public class ChronoBreakPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldTakeExtraTurn(Player player)
    {
        if (player != this.Owner.Player) return false;
        return true;
    }

    public override Task AfterTakingExtraTurn(Player player)
    {
        if (player != this.Owner.Player) return Task.CompletedTask;
        
        return PowerCmd.Remove<ChronoBreakPower>(this.Owner);
    }

}