using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Anemia() : MpConsumeCard(2,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, 5)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars,new DamageVar(3m,ValueProp.Move), new VampireVar()];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (!IsVampireForm)
        {
            ConsumeMp();
            await VampireAttack(choiceContext, play.Target);
        }
        else
        {
            await VampirePlay(choiceContext, play.Target);
        }
        
        await CreatureCmd.Stun(play.Target!);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}