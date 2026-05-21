using HarmonyLib;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;
using WizardMod.WizardModCode.Mp;

namespace WizardMod.WizardModCode.Patches;

[HarmonyPatch(typeof(AchievementsHelper), nameof(AchievementsHelper.AfterRunEnded))]
public static class RunEndPatch
{
    public static void Postfix(RunState state, Player player, bool isVictory)
    {
        if (isVictory)
            MpSaveData.Delete();
    }
}