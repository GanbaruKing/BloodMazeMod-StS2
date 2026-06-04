using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;


public class ManaShieldPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
 

    public override async Task BeforeCardPlayed(CardPlay play)
    {
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;

        if (play.Card is MpConsumeCard mp
            && !mp.IsVampireForm
            && !mp.IsFreeThisPlay)
        {
            await CreatureCmd.GainBlock(this.Owner.Player!.Creature , ((PowerModel)this).Amount, ValueProp.Unpowered, play);
        }
    }
    
}