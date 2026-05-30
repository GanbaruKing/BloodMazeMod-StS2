using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Anemia() : MpConsumeCard(3,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, 8)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars,new DamageVar(10m,ValueProp.Move), new VampireVar()];
    public override int CanonicalStarCost => MpCost;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await VampireAttack(choiceContext, play.Target);
        await CreatureCmd.Stun(play.Target!);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}