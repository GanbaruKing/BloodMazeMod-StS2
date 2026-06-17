using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class FatalInjury() : BloodMazeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new DynamicVar("Multiplier", 2m) ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        HemorrhagePower power = play.Target!.GetPower<HemorrhagePower>()!;
        if (power == null)
            return;
        if (((PowerModel)power).Amount > 0)
        {
            int amount = ((PowerModel)power).Amount;
            int num = amount * ((CardModel)this).DynamicVars["Multiplier"].IntValue - amount;
            await PowerCmd.ModifyAmount(power, num, this.Owner.Creature, this);
        }

    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}