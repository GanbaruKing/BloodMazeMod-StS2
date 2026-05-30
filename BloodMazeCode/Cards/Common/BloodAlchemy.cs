using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class BloodAlchemy() : MpConsumeCard(2,
    CardType.Skill, CardRarity.Common, TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new BlockVar(8m, ValueProp.Move), new CardsVar(2) ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<BloodBag>(this.IsUpgraded)];
    public override int CanonicalStarCost => MpCost;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CommonActions.CardBlock(this, play);
        for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            CardModel? inHand = await BloodBag.CreateInHand(this.Owner, this.CombatState!);
            await Cmd.Wait(0.25f);
            if (this.IsUpgraded)
                CardCmd.Upgrade(inHand!);
        }
    }
    
}