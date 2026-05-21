using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using WizardMod.WizardModCode.Mp;
using WizardMod.WizardModCode.Relics;


namespace WizardMod.WizardModCode.Cards.Basic;



public class Fire() : MpConsumeCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AllEnemies, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(8m, ValueProp.Move), new PowerVar<VulnerablePower>(1m), new PowerVar<WeakPower>(1m)];

    protected override bool IsPlayable => MpSaveData.CurrentMp >= MpCost;


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        MpSaveData.TryConsume(2);
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<VulnerablePower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel) this); 
        await PowerCmd.Apply<WeakPower>((IEnumerable<Creature>)this.CombatState.HittableEnemies, DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1m);
        this.DynamicVars.Weak.UpgradeValueBy(1m);
    }
}
