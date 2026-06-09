using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class BloodFlaskSupply() : BloodMazeCard(0,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodFlaskSupplyPower>(0m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<BloodFlaskSupplyPower>(choiceContext, this, this.ResolveEnergyXValue() + DynamicVars["BloodBagSupplyPower"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BloodBagSupplyPower"].UpgradeValueBy(1m);
    }
}