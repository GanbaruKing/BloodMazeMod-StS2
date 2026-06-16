using System;
using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(NPlayerHand), "ReturnHolderToHand")]
public static class ClampReturnHolderToHandPatch
{
    private static readonly AccessTools.FieldRef<NPlayerHand, Dictionary<NHandCardHolder, int>> QueueRef =
        AccessTools.FieldRefAccess<NPlayerHand, Dictionary<NHandCardHolder, int>>("_holdersAwaitingQueue");

    static bool Prefix(NPlayerHand __instance, NHandCardHolder holder)
    {
        MainFile.Logger.Info("[ClampReturnHolderToHandPatch] Prefix called");

        if (holder == null) return true;

        var queue = QueueRef(__instance);
        if (queue == null) return true;
        if (!queue.TryGetValue(holder, out int index)) return true;


        CardModel? card = holder.CardModel;

  
        if (card?.Pile?.Type != PileType.Hand)
        {
            MainFile.Logger.Info(
                $"[ClampReturnHolderToHandPatch] skipped ReturnHolderToHand. card={card?.Id.Entry}, pile={card?.Pile?.Type}");

            queue.Remove(holder);
            return false;
        }

        int childCount = __instance.CardHolderContainer.GetChildCount();
        int maxIndex = Math.Max(0, childCount - 1);
        int clamped = Math.Clamp(index, 0, maxIndex);

        if (index != clamped)
        {
            MainFile.Logger.Info(
                $"[ClampReturnHolderToHandPatch] clamped index {index} -> {clamped}, childCount={childCount}");
            queue[holder] = clamped;
        }

        return true;
    }
}