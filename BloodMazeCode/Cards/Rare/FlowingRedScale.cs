using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Rare;

public class FlowingRedScale() : BloodMazeCard(2,
    CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FlowingRedScalePower>(1m),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<FlowingRedScalePower>(
            this.Owner.Creature,
            this.DynamicVars["FlowingRedScalePower"].BaseValue,
            this.Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => this.DynamicVars["FlowingRedScalePower"].UpgradeValueBy(1m);
}