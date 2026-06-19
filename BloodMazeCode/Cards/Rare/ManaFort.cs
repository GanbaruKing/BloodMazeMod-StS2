using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;



public class ManaFort() : BloodMazeCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<TemporaryDexterityPower>(0m),
        new BlockVar(3m, ValueProp.Move)
    ];

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int amount = ResolveEnergyXValue() + DynamicVars["TemporaryDexterityPower"].IntValue;
        if (amount <= 0) return;

        await CommonActions.ApplySelf<TemporaryDexterityPower>(choiceContext, this, amount);
        for (int i = 0; i < amount; i++)
            await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TemporaryDexterityPower"].UpgradeValueBy(1m);
    }
}
