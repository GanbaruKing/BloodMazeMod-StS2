using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;



public class Selection() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new CardsVar(1)];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {    
        ConsumeMp();
        var candidates = PileType.Discard.GetPile(this.Owner).Cards
            .Concat(PileType.Draw.GetPile(this.Owner).Cards)
            .ToList();
        
        if (!candidates.Any()) return;
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, candidates, this.Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null) return;
        
        await CardPileCmd.Add(card, PileType.Hand);

    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}