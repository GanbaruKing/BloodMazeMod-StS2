using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace BloodMaze.BloodMazeCode.Powers;


public class HarvestPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    public bool IsUpgraded { get; set; } = false;
}