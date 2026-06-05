using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class HealingBonfire() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<RegenPower>(3m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        foreach (Creature creature in this.CombatState!.GetTeammatesOf(this.Owner.Creature)
                     .Where<Creature>((Func<Creature, bool>)(c => c != null && c.IsAlive && c.IsPlayer)))
        {
            await CommonActions.Apply<RegenPower>(choiceContext, creature, this);
        }

    }

    protected override void OnUpgrade()
    {
        DynamicVars["RegenPower"].UpgradeValueBy(1m);
    }
}