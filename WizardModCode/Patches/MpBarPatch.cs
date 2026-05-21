using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using WizardMod.WizardModCode.Mp;

namespace WizardMod.WizardModCode.Patches;

[HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.SetCreature))]
public class MpBarPatch
{
    private const string MpBarBgName = "WizardModMpBarBg";
    private const float BarHeight = 10f;
    private const float CornerRadius = 5f;
    private const float FontSize = 20f;

    private static readonly Color BgColor     = new Color(0.0f, 0.0f, 0.20f, 1.0f);
    private static readonly Color FillColor   = new Color(0.25f, 0.45f, 0.90f, 1.0f);
    private static readonly Color TextColor   = new Color(1.0f,  1.0f,  1.0f,  1.0f);
    private static readonly Color OutlineColor = new Color(0.1f, 0.2f, 0.8f, 1.0f);

    [HarmonyPostfix]
    static void AddMpBar(NHealthBar __instance, Creature creature)
    {
        if (!creature.IsPlayer) return;
        if (creature.Player?.Character is not WizardMod.WizardModCode.Character.WizardMod) return;
        if (__instance.FindChild(MpBarBgName) != null) return;

        var hpBar = __instance.HpBarContainer;
        
        if (hpBar == null) return;
        
        float fullWidth = hpBar != null ? hpBar.Size.X : 120f;
        float barWidth = fullWidth * 0.94f;
        float offsetX   = hpBar!.Position.X + (fullWidth - barWidth) / 2f + 2f;
        float offsetY  = hpBar != null ? hpBar.Position.Y + hpBar.Size.Y + 3f : 48f;
        GD.Print($"[WizardMod] fullWidth={fullWidth}, barWidth={barWidth}, offsetX={offsetX}");
        
        var bg = new Panel();
        bg.Name = MpBarBgName;
        bg.Size = new Vector2(barWidth, BarHeight);
        bg.Position = new Vector2(offsetX, offsetY);
        var bgStyle = new StyleBoxFlat();
        bgStyle.BgColor = BgColor;
        bgStyle.CornerRadiusTopLeft     = (int)CornerRadius;
        bgStyle.CornerRadiusTopRight    = (int)CornerRadius;
        bgStyle.CornerRadiusBottomLeft  = (int)CornerRadius;
        bgStyle.CornerRadiusBottomRight = (int)CornerRadius;
        bg.AddThemeStyleboxOverride("panel", bgStyle);
        __instance.AddChild(bg);
        
        var fill = new Panel();
        fill.Size = new Vector2(GetFillWidth(barWidth), BarHeight);
        fill.Position = Vector2.Zero;
        var fillStyle = new StyleBoxFlat();
        fillStyle.BgColor = FillColor;
        fillStyle.CornerRadiusTopLeft     = (int)CornerRadius;
        fillStyle.CornerRadiusTopRight    = (int)CornerRadius;
        fillStyle.CornerRadiusBottomLeft  = (int)CornerRadius;
        fillStyle.CornerRadiusBottomRight = (int)CornerRadius;
        fill.AddThemeStyleboxOverride("panel", fillStyle);
        bg.AddChild(fill);
        
        var label = new Label();
        label.Text = GetMpText();
        label.Size = new Vector2(barWidth, BarHeight);
        label.Position = new Vector2(0, -(FontSize - BarHeight) / 2f);
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", (int)FontSize);
        label.AddThemeColorOverride("font_color", TextColor);
        label.AddThemeConstantOverride("outline_size", 7);
        label.AddThemeColorOverride("font_outline_color", OutlineColor);
        bg.AddChild(label);
        
        MpSaveData.MpChanged += () =>
        {
            if (!GodotObject.IsInstanceValid(fill)) return;
            fill.Size = new Vector2(GetFillWidth(barWidth), BarHeight);
            if (GodotObject.IsInstanceValid(label))
                label.Text = GetMpText();
        };
    }

    private static float GetFillWidth(float barWidth)
    {
        if (MpSaveData.MaxMp <= 0) return 0f;
        return barWidth * ((float)MpSaveData.CurrentMp / MpSaveData.MaxMp);
    }

    private static string GetMpText() =>
        $"{MpSaveData.CurrentMp} / {MpSaveData.MaxMp}";
}