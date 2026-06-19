using BloodMaze.BloodMazeCode.Mp;
using HarmonyLib;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(AchievementsHelper), nameof(AchievementsHelper.AfterRunEnded))]
public static class RunEndPatch
{
    public static void Postfix(RunState state, Player player, bool isVictory)
    {
        MpSaveData.Delete();
    }
}
