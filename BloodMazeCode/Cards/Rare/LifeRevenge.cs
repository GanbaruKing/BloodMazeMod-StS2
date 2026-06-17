using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;

public class LifeRevenge() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private int _healCount;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(1),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("CalculatedHits").WithMultiplier(
            (Func<CardModel, Creature?, decimal>)(static (card, _) =>
                (decimal)(1 + ((LifeRevenge)card)._healCount))!)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.IntValue).WithHitCount((int)((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(play.Target)).FromCard((CardModel) this).Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (creature == this.Owner.Creature && delta > 0m)
            _healCount++;
        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        _healCount = 0;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _healCount = 0;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}