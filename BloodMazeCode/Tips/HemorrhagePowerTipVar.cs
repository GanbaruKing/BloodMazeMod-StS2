using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Tips;

public class HemorrhagePowerTipVar : DynamicVar
{
    public const string Key = "HemorrhagePowerTip";

    public HemorrhagePowerTipVar() : base
        (Key, 1)
    {
        this.WithTooltip(locTable: "card_keywords");
    }
}