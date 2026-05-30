using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class SpiritBottle() : MpConsumeCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new EnergyVar(1), new CardsVar(1)];
   
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        MpSaveData.TryConsume(MpCost);
        await PlayerCmd.GainEnergy((Decimal)DynamicVars.Energy.IntValue, this.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}