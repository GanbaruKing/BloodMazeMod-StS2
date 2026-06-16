using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(NPlayerHand), "StartCardPlay")]
public static class BlockCardPlayDuringTransformPatch
{
    static bool Prefix(NHandCardHolder holder, bool startedViaShortcut)
    {
        if (!TransformInputLock.IsLocked)
            return true;

        MainFile.Logger.Info(
            $"[BlockCardPlayDuringTransformPatch] blocked StartCardPlay during transform. shortcut={startedViaShortcut}");

        return false;
    }
}