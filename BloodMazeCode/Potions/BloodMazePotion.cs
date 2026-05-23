using BaseLib.Abstracts;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Character;

namespace BloodMaze.BloodMazeCode.Potions;

[Pool(typeof(BloodMazePotionPool))]
public abstract class BloodMazePotion : CustomPotionModel;