using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Exceptions;

namespace BloodMaze.BloodMazeCode.Cards.Ancient;


public class BloodPact() : BloodMazeCard(1,
    CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodPactPower>(1m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<BloodPactPower>(choiceContext, this, 1);
        var power = this.Owner.Creature.GetPower<BloodPactPower>();
        if (power != null && IsUpgraded)
            ((BloodPactPower)power).RestoreAmount = 2;
    }
    
}