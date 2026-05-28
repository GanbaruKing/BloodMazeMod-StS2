using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class BloodSword() : BloodMazeCard(11,
    CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    
    private int _hpLossTriggers;

    private int _baseCost = 11;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(40m, ValueProp.Move), new VampireVar()];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).TargetingAllOpponents(this.CombatState!).Execute(choiceContext);
        decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
        await CreatureCmd.Heal(this.Owner.Creature, restore);
        _hpLossTriggers -= 3;
    }
    
    private void RefreshCost()
    {
        this.EnergyCost.SetCustomBaseCost(Math.Max(0, _baseCost - _hpLossTriggers));
    }
    
    
    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (creature == this.Owner.Creature && delta < 0m)
        {
            _hpLossTriggers++;
            RefreshCost();
        }
        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        _hpLossTriggers = 0;
        RefreshCost();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _hpLossTriggers = 0;
        RefreshCost();
        return Task.CompletedTask;
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10m);
    }
}