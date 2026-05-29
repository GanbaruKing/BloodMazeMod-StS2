using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards.Token;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Extract() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new HemorrhagePowerTipVar()];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<BloodBag>(this.IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HemorrhagePower>(play.Target!, 1m, this.Owner.Creature, this);

        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1);
        CardModel original = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, null, this)).FirstOrDefault()!;
        if (original == null!) return;

        CardModel card = this.CombatState!.CreateCard<BloodBag>(this.Owner);
        if (this.IsUpgraded)
            CardCmd.Upgrade(card);
        await CardCmd.Transform(original, card);
    }
}