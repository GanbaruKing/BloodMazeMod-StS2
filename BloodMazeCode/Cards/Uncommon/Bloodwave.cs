using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class Bloodwave() : BloodMazeCard(1,
    CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodwavePower>(5m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<BloodwavePower>(choiceContext,this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BloodwavePower"].UpgradeValueBy(2m);
    }
}