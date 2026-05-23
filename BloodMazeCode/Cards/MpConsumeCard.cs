using System.Collections.Generic;
using BaseLib.Cards.Variables;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards;

public abstract class MpConsumeCard(int cost, CardType type, CardRarity rarity, TargetType target, int mpCost)
    : BloodMazeCard(cost, type, rarity, target)
{
    protected readonly int MpCost = mpCost;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<MpConsumeCard>("ConsumeMp", (_) => MpCost.ToString()),
    ];
    
    protected override bool IsPlayable => MpSaveData.CurrentMp >= MpCost;
}