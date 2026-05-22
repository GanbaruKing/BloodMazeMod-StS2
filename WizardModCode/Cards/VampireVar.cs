using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace WizardMod.WizardModCode.Cards;

public class VampireVar : DynamicVar
{
    public const string Key = "Vampire";

    public VampireVar() : base
        (Key, 1)
    {
        this.WithTooltip();
    }
}