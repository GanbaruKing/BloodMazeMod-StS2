using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class FlashInvincibility() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self, 8)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<BufferPower>(1m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CommonActions.ApplySelf<BufferPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BufferPower"].UpgradeValueBy(1m);
    }
}