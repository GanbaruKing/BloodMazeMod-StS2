using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace BloodMaze.BloodMazeCode.Powers;


public class EternalNightPower: BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    public override bool ShouldPlayerResetEnergy(Player player)
    {
        return player.Creature.CombatState.RoundNumber == 1 || player != this.Owner.Player;
    }
}