using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Lightning() : MpConsumeCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy, 4)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [..base.CanonicalVars, new DamageVar(8m, ValueProp.Move), new CardsVar(1)];
  
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await VampirePlay(choiceContext, play.Target);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.IntValue, this.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}