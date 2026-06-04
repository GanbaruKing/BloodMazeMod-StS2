using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Immortality() : BloodMazeCard(2,
    CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(2m), new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(1m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
        await CommonActions.ApplySelf<HemorrhagePower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["IntangiblePower"].UpgradeValueBy(1m);
    }
}