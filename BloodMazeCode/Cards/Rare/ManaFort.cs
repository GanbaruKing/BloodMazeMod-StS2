using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;



public class ManaFort() : BloodMazeCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(0m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(
            ((Func<CardModel, Creature, Decimal>)((card, _) =>
                (Decimal)MpSaveData.CurrentMp)!)!)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered, null, this);
        int block = MpSaveData.CurrentMp;
        await CreatureCmd.GainBlock(this.Owner.Creature, block, ValueProp.Move, play);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.SetCustomBaseCost(2);
    }
}