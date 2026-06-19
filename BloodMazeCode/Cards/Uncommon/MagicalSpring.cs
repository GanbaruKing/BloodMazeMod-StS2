using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class MagicalSpring() : BloodMazeCard(2,
    CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        this.EnergyHoverTip,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<MagicalSpringPower>(
            choiceContext,
            this.Owner.Creature,
            this.DynamicVars.Energy.BaseValue,
            this.Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
