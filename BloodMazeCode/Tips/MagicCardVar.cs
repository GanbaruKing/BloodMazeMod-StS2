using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Tips;

public class MagicCardVar : DynamicVar
{
    public const string Key = "Magiccard";

    public MagicCardVar() : base
        (Key, 1)
    {
        this.WithTooltip();
    }
}