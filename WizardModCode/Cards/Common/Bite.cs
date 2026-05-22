using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.GameInfo.Objects;
using WizardMod.WizardModCode.Cards;

namespace WizardMod.WizardModCode.Cards.Common;


public class Bite() : WizardModCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new HpLossVar(7M), new VampireVar()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        AttackCommand attack = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
        await CreatureCmd.Heal(this.Owner.Creature, restore);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}