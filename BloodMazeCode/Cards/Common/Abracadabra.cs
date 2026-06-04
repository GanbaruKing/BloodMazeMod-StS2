using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Abracadabra() : MpConsumeCard(0,
    CardType.Skill, CardRarity.Common, TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new CardsVar(2)];
  
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.IntValue, this.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}