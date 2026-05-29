using System;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;


public class BloodSprayPower : BloodMazePower
{
    
    public decimal Multiplier { get; set; } = 1.75m;
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target == null) return 1m;
        if (!target.HasPower<HemorrhagePower>()) return 1m;
        if (dealer != null || cardSource != null) return 1m;
        return this.Amount;
    }
}