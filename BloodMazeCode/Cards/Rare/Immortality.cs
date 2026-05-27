using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Immortality() : BloodMazeCard(2,
    CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(2m), new HpLossVar(12m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["IntangiblePower"].UpgradeValueBy(1m);
    }
}