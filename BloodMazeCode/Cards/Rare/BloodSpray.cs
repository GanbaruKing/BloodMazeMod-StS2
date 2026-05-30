using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Rare;

public class BloodSpray() : BloodMazeCard(1,
    CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<BloodSprayPower>(1.75m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<BloodSprayPower>(choiceContext, this, 1.75m);
        var power = this.Owner.Creature.GetPower<BloodSprayPower>();
        if (power != null)
            power.Multiplier = IsUpgraded ? 2m : 1.75m;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BloodSprayPower"].UpgradeValueBy(0.25m); 
    }
}