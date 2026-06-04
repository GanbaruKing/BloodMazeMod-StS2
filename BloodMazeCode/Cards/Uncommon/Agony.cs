using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;

 
public class Agony() : MpConsumeCard(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars,new CalculationBaseVar(8m), new ExtraDamageVar(1m), 
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, target) => (decimal)target!.GetPowerAmount<HemorrhagePower>())
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await VampirePlay(choiceContext, play.Target);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}