using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class ManaDrain() : MpConsumeCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, 1)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(4m, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        MpSaveData.TryConsume(MpCost);
        AttackCommand attack = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        int damageDealt = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
        MpSaveData.Restore(damageDealt);
    }

    protected override void OnUpgrade()
    {
        this.RemoveKeyword(CardKeyword.Exhaust);
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}