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


public class Blut() : MpConsumeCard(1, CardType.Skill,
    CardRarity.Rare, TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<PlatingPower>(5), new HpLossVar(6m), new PowerVar<ArtifactPower>(1m)];
 
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,this);
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, this);
        await CommonActions.ApplySelf<ArtifactPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ArtifactPower"].UpgradeValueBy(1m);
    }
}