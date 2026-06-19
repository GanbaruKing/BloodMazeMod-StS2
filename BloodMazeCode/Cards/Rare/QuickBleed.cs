using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class QuickBleed() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new DamageVar(10m, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsUpgraded)
        {
            await PowerCmd.Apply<HemorrhagePower>(choiceContext, this.CombatState!.HittableEnemies, 2m, this.Owner.Creature, this);
        }
        AttackCommand attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(this.CombatState!).Execute(choiceContext);
        var enemyList = this.CombatState!.HittableEnemies;
        foreach (var enemy in enemyList)
        {
            var current  = enemy.GetPowerAmount<HemorrhagePower>();
            var toadd = 20 - current;
            if (current < 10) continue;
            await PowerCmd.Apply<HemorrhagePower>(choiceContext, enemy, (decimal)toadd, this.Owner.Creature, this);
        }
    }
    
}
