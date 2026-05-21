using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace WizardMod.WizardModCode.Cards;

public abstract class MpConsumeCard(int cost, CardType type, CardRarity rarity, TargetType target, int mpCost)
    : WizardModCard(cost, type, rarity, target)
{
    protected readonly int MpCost = mpCost;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<MpConsumeCard>("ConsumeMp", (_) => MpCost.ToString()),
    ];
}