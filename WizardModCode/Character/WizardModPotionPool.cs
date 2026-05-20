using BaseLib.Abstracts;
using WizardMod.WizardModCode.Extensions;
using Godot;

namespace WizardMod.WizardModCode.Character;

public class WizardModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => WizardMod.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}