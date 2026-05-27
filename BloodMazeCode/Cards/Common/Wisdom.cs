using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Wisdom() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal]; 

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        CardModel card = (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Discard.GetPile(this.Owner).Cards, this.Owner, prefs)).FirstOrDefault<CardModel>()!;
        if (card == null) return;
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
        card.AddKeyword(CardKeyword.Retain);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}