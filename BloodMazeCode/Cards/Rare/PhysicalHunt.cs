using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class PhysicalHunt() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, 5)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        StrengthPower power = play.Target!.GetPower<StrengthPower>()!;
        if (power == null! || power.Amount <= 0) return;
        int amount = power!.Amount;
        await PowerCmd.Remove<StrengthPower>(play.Target);
        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, amount, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
