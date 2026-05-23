using BaseLib.Abstracts;
using BloodMaze.BloodMazeCode.Extensions;
using Godot;

namespace BloodMaze.BloodMazeCode.Character;

public class BloodMazePotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => BloodMaze.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}