// Patches/TopBarMpPatch.cs
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.sts2.Core.Nodes.TopBar;
using WizardMod.WizardModCode.Mp;

namespace WizardMod.WizardModCode.Patches;

[HarmonyPatch(typeof(NTopBarHp), nameof(NTopBarHp.Initialize))]
public static class TopBarMpPatch
{
    private const string MpLabelName = "WizardModTopBarMpLabel";

    public static void Postfix(NTopBarHp __instance, Player player)
    {
        GD.Print($"[WizardMod] instance.Size={__instance.Size}, instance.Position={__instance.Position}");
        if (player.Character is not Character.WizardMod) return;
        if (__instance.FindChild(MpLabelName) != null) return;

        var label = new Label();
        label.Name = MpLabelName;
        label.Text = GetMpText();
        label.AddThemeFontSizeOverride("font_size", 30);
        label.AddThemeColorOverride("font_color", new Color(0.30f, 0.60f, 1.0f, 1.0f));
        label.AddThemeConstantOverride("outline_size", 5);
        label.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 1.0f));
        label.Position = new Vector2(-50f, -20);
        __instance.AddChild(label);

        MpSaveData.MpChanged += () =>
        {
            if (!GodotObject.IsInstanceValid(label)) return;
            label.Text = GetMpText();
        };
    }

    private static string GetMpText() => $"{MpSaveData.CurrentMp}/{MpSaveData.MaxMp}";
}