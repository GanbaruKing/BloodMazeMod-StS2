using BloodMaze.BloodMazeCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.HasEnoughResourcesFor))]
public static class IgnoreStarRequirementPatch
{
    static void Postfix(CardModel card, ref bool __result, ref UnplayableReason reason)
    {
        if (card is MpConsumeCard)
        {
            reason &= ~UnplayableReason.StarCostTooHigh;   
            __result = (reason == UnplayableReason.None);
        }
    }
}

[HarmonyPatch(typeof(CardModel), "SpendStars")]   
public static class SkipStarSpendPatch
{
    static void Prefix(CardModel __instance, ref int amount)
    {
        if (__instance is MpConsumeCard)
            amount = 0;   
    }
}