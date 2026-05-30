using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class BloodStealing() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), ..base.CanonicalVars];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        if (play.Target == null) return;
        decimal powerAmount = (decimal)play.Target.GetPowerAmount<HemorrhagePower>();
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, powerAmount);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}