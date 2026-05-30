using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;

public class Rejuvenate() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self, 5)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..base.CanonicalVars,
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("HealAmount").WithMultiplier(
            ((Func<CardModel, Creature, decimal>)((card, _) =>
                (decimal)(MpSaveData.CombatMpConsumeCount +
                          CombatManager.Instance.History.Entries
                              .OfType<DamageReceivedEntry>()
                              .Count(e => e.Receiver == card.Owner.Creature && e.Result.UnblockedDamage > 0))))!)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ConsumeMp();
        int healCount = (int)((CalculatedVar)this.DynamicVars["HealAmount"]).Calculate(null);
        for (int i = 0; i < healCount; i++)
        {
            await CreatureCmd.Heal(this.Owner.Creature, 1m);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars["HealAmount"].UpgradeValueBy(3m);
}