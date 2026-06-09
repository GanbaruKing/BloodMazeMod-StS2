using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards.Token;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class ReuseRush() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Uncommon,TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(3m), new ExtraDamageVar(2m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, creature) => (Decimal)PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c is BloodFlask))];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}