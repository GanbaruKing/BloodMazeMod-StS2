using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Powers;


public class SilentCast() : BloodMazeCard(1,
    CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MagicCardVar(), new PowerVar<SilentCastPower>(1m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SilentCastPower>(this.Owner.Creature, DynamicVars["SilentCastPower"].IntValue, this.Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}