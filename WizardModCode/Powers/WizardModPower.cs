using BaseLib.Abstracts;
using BaseLib.Extensions;
using WizardMod.WizardModCode.Extensions;
using Godot;

namespace WizardMod.WizardModCode.Powers;

public abstract class WizardModPower : CustomPowerModel
{
    //Loads from WizardMod/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}