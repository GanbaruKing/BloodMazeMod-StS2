using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Tips;


public class OverflowVar : DynamicVar
{
    public const string Key = "Overflow";

    public OverflowVar() : base
        (Key, 1)
    {
        this.WithTooltip();
    }
}