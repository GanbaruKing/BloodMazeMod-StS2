using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Rare;

 
public class ChronoBreak() : MpConsumeCard(3,
    CardType.Power, CardRarity.Rare, TargetType.Self, 8)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<ChronoBreakPower>(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ChronoBreakPower>(this.Owner.Creature , DynamicVars["ChronoBreakPower"].IntValue, this.Owner.Creature, this);
        await PowerCmd.Apply<RetainHandPower>(this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}