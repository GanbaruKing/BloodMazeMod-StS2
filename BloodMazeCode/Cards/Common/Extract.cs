using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards.Token;
using BloodMaze.BloodMazeCode.Patches;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace BloodMaze.BloodMazeCode.Cards.Common;

public class Extract() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HemorrhagePower>(1m), new HemorrhagePowerTipVar(), new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<BloodFlask>(this.IsUpgraded)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {

        await PowerCmd.Apply<HemorrhagePower>(
            choiceContext,
            play.Target!, DynamicVars["HemorrhagePower"].IntValue, this.Owner.Creature, this);
        
        var hand = NPlayerHand.Instance;

        var handCards = PileType.Hand.GetPile(this.Owner).Cards;
        if (!handCards.Any()) return;

        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1);
        CardModel original = (await CardSelectCmd.FromHand(
            choiceContext, this.Owner, prefs, null, this)).FirstOrDefault()!;
        if (original == null) return;;

        CardModel card = this.CombatState!.CreateCard<BloodFlask>(this.Owner);
        if (this.IsUpgraded)
            CardCmd.Upgrade(card);
        TransformInputLock.Lock();
        try
        {
            await CardCmd.Transform(original, card);
        }
        finally
        {
            TransformInputLock.Unlock();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HemorrhagePower"].UpgradeValueBy(1m);
    }
}
