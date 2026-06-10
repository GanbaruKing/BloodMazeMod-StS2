using BloodMaze.BloodMazeCode.Character;
using BloodMaze.BloodMazeCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace BloodMaze.BloodMazeCode.Patches;

[HarmonyPatch(typeof(NCard), "Reload")]
public static class StarIconTexturePatch
{
    static void Postfix(NCard __instance)
    {
        if (__instance.Model?.Pool is not BloodMazeCardPool) return;

        var texture = PreloadManager.Cache.GetTexture2D(
            "charui/mp_icon.png".ImagePath());

        __instance.GetNodeOrNull<TextureRect>("%StarIcon")
            ?.Set(TextureRect.PropertyName.Texture, Variant.From(texture));
    }
}