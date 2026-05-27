using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class MagicCircle() : BloodMazeCard(2,
    CardType.Skill, CardRarity.Uncommon,
    MegaCrit.Sts2.Core.Entities.Cards.TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move), new MagicCardVar()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<FreeMpAttackPower>(this.Owner.Creature, 1m, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}