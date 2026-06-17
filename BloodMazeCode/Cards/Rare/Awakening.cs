using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;

public class Awakening() : BloodMazeCard(
    1,
    CardType.Attack,
    CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private int _pendingUpgradeCount;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        new PowerVar<ImprovementPower>(1m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool shouldTriggerFatal = play.Target!.Powers.All(
            (Func<PowerModel, bool>)(p => p.ShouldOwnerDeathTriggerFatal()));

        AttackCommand attackCommand =
            await CommonActions.CardAttack(this, play.Target)
                .Execute(choiceContext);

        bool killedByThisAttack = attackCommand.Results.Any(
            (Func<DamageResult, bool>)(r => r.WasTargetKilled));

        if (!shouldTriggerFatal || !killedByThisAttack)
            return;

        _pendingUpgradeCount += DynamicVars["ImprovementPower"].IntValue;
    }

    public override Task BeforeCombatStart()
    {
        _pendingUpgradeCount = 0;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (_pendingUpgradeCount > 0)
        {
            UpgradeRandomDeckCards(_pendingUpgradeCount);
            _pendingUpgradeCount = 0;
        }

        return Task.CompletedTask;
    }

    private void UpgradeRandomDeckCards(int amount)
    {
        List<CardModel> upgradableCards = PileType.Deck
            .GetPile(this.Owner)
            .Cards
            .Where(c => c.IsUpgradable)
            .ToList();

        for (int i = 0; i < amount && upgradableCards.Count != 0; i++)
        {
            CardModel card = this.Owner.RunState.Rng.CombatCardSelection
                .NextItem<CardModel>(upgradableCards)!;

            upgradableCards.Remove(card);
            CardCmd.Upgrade(card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}
