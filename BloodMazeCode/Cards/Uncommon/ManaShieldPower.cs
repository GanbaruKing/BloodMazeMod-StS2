using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class ManaShield() : BloodMazeCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ManaShieldPower>(3m), new MagicCardVar()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<ManaShieldPower>(choiceContext, this, DynamicVars["ManaShieldPower"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ManaShieldPower"].UpgradeValueBy(1m);
    }
}