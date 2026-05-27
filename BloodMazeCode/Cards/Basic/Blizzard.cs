using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Basic;




public class Blizzard() : MpConsumeCard(0,
    CardType.Attack, CardRarity.Basic,
    TargetType.AllEnemies, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(2m, ValueProp.Move), new PowerVar<VulnerablePower>(1m), new PowerVar<WeakPower>(1m)];
    


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await VampirePlayAllEnemies(choiceContext);
        await PowerCmd.Apply<WeakPower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
        await PowerCmd.Apply<VulnerablePower>((IEnumerable<Creature>)this.CombatState!.HittableEnemies, DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel) this); 
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1m);
        this.DynamicVars.Weak.UpgradeValueBy(1m);
    }
}
