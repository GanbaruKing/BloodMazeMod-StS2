using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace BloodMaze.BloodMazeCode.Patches;

// 現行ゲームの NMerchantCharacter._Ready は `new MegaSprite(GetChild(0))` を行い、
// 子0番が本物の SpineSprite でないと例外になる。カスタム商人は非Spine
// (Sprite2D + AnimationPlayer) なので、その場合のみ Spine 設定をスキップし、
// BaseLib のパッチ済み PlayAnimation 経由で AnimationPlayer を再生させる。
[HarmonyPatch(typeof(NMerchantCharacter), "_Ready")]
public static class MerchantNonSpineReadyPatch
{
    static bool Prefix(NMerchantCharacter __instance)
    {
        // 本物の Spine 商人（基本キャラ等）は元の処理に任せる
        if (__instance.GetChildCount() > 0 && __instance.GetChild(0).IsClass("SpineSprite"))
            return true;

        // 非Spine のカスタム商人：クラッシュする MegaSprite バインドを回避
        if (CustomAnimation.HasCustomAnimation(__instance))
            __instance.PlayAnimation("relaxed_loop", true);
        return false;
    }
}
