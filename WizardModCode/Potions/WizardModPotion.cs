using BaseLib.Abstracts;
using BaseLib.Utils;
using WizardMod.WizardModCode.Character;

namespace WizardMod.WizardModCode.Potions;

[Pool(typeof(WizardModPotionPool))]
public abstract class WizardModPotion : CustomPotionModel;