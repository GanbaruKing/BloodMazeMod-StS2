using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public sealed class BloodPhoenix() : BloodMazeCard(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1m), new BlockVar(20m, ValueProp.Unpowered)];

    public override CardMultiplayerConstraint MultiplayerConstraint
        => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords
        => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var deadAllies = this.CombatState!
            .GetTeammatesOf(this.Owner.Creature)
            .Where(c => c.IsPlayer && c.IsDead)
            .ToList();

        foreach (var creature in deadAllies)
        {
            await CreatureCmd.Heal(creature, DynamicVars.Heal.IntValue);
            await CreatureCmd.GainBlock(creature, DynamicVars.Block.IntValue, ValueProp.Unpowered, play);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}