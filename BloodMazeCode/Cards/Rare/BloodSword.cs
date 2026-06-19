using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class BloodSword() : BloodMazeCard(9,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    
    private int _hpLossTriggers;

    private int _baseCost = 9;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new VampireVar()];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int damage = Owner.Creature.CurrentHp;
        AttackCommand attack = await CommonActions.CardAttack(this, play.Target, damage).Execute(choiceContext);
        decimal restore = attack.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage - r.BlockedDamage);
        await CreatureCmd.Heal(this.Owner.Creature, restore);
        _hpLossTriggers -= 2;
    }
    
    private void RefreshCost()
    {
        this.EnergyCost.SetCustomBaseCost(Math.Max(0, _baseCost - _hpLossTriggers));
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        RefreshCost();
        return Task.CompletedTask;
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
        EnergyCost.UpgradeBy(-2);
    }
}
