using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class HexChain() : MpConsumeCard(1,
    CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(10m, ValueProp.Move), new PowerVar<VulnerablePower>(2m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await VampirePlay(choiceContext, play.Target);
        var last = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault();
        if (last?.CardPlay.Card is MpConsumeCard { Type: CardType.Attack })
        {
            await CommonActions.Apply<VulnerablePower>(choiceContext, play.Target!, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
    }
}