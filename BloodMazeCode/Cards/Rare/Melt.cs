using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards.Token;
using BloodMaze.BloodMazeCode.Patches;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Rare;



public class Melt() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(1m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<BloodFlask>(this.IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<HemorrhagePower>(choiceContext, this);
        
        var handCards = PileType.Hand.GetPile(this.Owner).Cards
            .Where(c => c != this)
            .ToList();
        if (handCards.Count == 0) return;

        TransformInputLock.Lock();
        try
        {
            foreach (CardModel original in handCards)
            {
                CardModel flask = this.CombatState!.CreateCard<BloodFlask>(this.Owner);
                if (this.IsUpgraded)
                    CardCmd.Upgrade(flask);
                await CardCmd.Transform(original, flask);
            }
        }
        finally
        {
            TransformInputLock.Unlock();
        }
    }

    protected override void OnUpgrade()
    {
    }
}