using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Setup() : MpConsumeCard(1, CardType.Skill, CardRarity.Common, TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new CardsVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ConsumeMp();
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(
            choiceContext,
            this.Owner,
            prefs,
            c => !c.Keywords.Contains(CardKeyword.Retain),
            this)).FirstOrDefault();

        if (card == null) return;
        card.AddKeyword(CardKeyword.Retain);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}