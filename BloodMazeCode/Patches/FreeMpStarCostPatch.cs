using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.GetStarCostWithModifiers))]
public static class FreeMpStarCostPatch
{
    static void Postfix(CardModel __instance, ref int __result)
    {
        if (__instance is not MpConsumeCard) return;

        try
        {
            var creature = __instance.Owner?.Creature;
            var silentCast = creature?.GetPower<SilentCastPower>();

            if (creature?.HasPower<FreeMpPower>() == true ||
                (__instance.Type == CardType.Attack &&
                 creature?.HasPower<FreeMpAttackPower>() == true) ||
                silentCast?.CanAffect(__instance) == true)
            {
                __result = 0;
            }
        }
        catch (CanonicalModelException) { }
    }
}