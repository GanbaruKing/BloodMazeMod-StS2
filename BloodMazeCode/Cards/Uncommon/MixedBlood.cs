using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;


namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class MixedBlood() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Uncommon,TargetType.Self)
{

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1);
        CardModel original = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null!, (AbstractModel) this)).FirstOrDefault<CardModel>()!;
        if (original == null) return;

        List<CardPoolModel> list1 = this.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
        if (list1.Count > 1)
            list1.Remove(this.Owner.Character.CardPool);
        IEnumerable<CardModel> cards = list1.SelectMany(pool => 
            pool.GetUnlockedCards(original.Owner.UnlockState, original.RunState!.CardMultiplayerConstraint)).Where(c => c.Rarity is CardRarity.Common
            or CardRarity.Uncommon
            or CardRarity.Rare);;
        CardModel newCard = CardFactory.CreateRandomCardForTransform(original, cards, true, Owner.RunState.Rng.CombatCardGeneration);
        if (this.IsUpgraded)
        {
            CardCmd.Upgrade(newCard);
        }
        else
        {
            CardCmd.Downgrade(newCard);
        }
        newCard.SetToFreeThisTurn();
        await CardCmd.Transform(original, newCard);
    }


}