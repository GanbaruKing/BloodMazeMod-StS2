using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Overflow() : BloodMazeCard(2,
    CardType.Power, CardRarity.Rare,TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodMaze.BloodMazeCode.Powers.Overflow>(2m), new OverflowVar()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BloodMaze.BloodMazeCode.Powers.Overflow>(this.Owner.Creature, DynamicVars["Overflow"].BaseValue, this.Owner.Creature, (CardModel)this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Overflow"].UpgradeValueBy(1m);
    }
}