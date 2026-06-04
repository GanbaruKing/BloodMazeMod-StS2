using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards.Token;
using MegaCrit.Sts2.Core.Combat; // BloodBag の名前空間に合わせる
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Powers;


public class BloodBagSupplyPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;




        var bag = await BloodBag.CreateInHand(this.Owner.Player!, this.CombatState);
        await Cmd.Wait(0.25f);
        CardCmd.Upgrade(bag!);

        await PowerCmd.ModifyAmount(this, -1, this.Owner, null);
        if (((PowerModel)this).Amount <= 0)
            await PowerCmd.Remove<BloodBagSupplyPower>(this.Owner);
    }
}