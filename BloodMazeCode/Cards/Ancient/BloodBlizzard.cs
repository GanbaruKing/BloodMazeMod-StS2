using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Ancient;


public class BloodBlizzard() : MpConsumeCard(0,
    CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(12m, ValueProp.Move), 
        new PowerVar<VulnerablePower>(2m), new PowerVar<WeakPower>(2m), new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(1m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await VampirePlayAllEnemies(choiceContext);
        await PowerCmd.Apply<WeakPower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
        await PowerCmd.Apply<VulnerablePower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel) this); 
        await PowerCmd.Apply<HemorrhagePower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars["HemorrhagePower"].BaseValue, this.Owner.Creature, (CardModel) this); 
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
        DynamicVars["WeakPower"].UpgradeValueBy(1m);
        DynamicVars["HemorrhagePower"].UpgradeValueBy(1m);
    }
}