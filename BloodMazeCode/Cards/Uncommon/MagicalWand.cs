using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class MagicalWand() : MpConsumeCard(2,
    CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy,0)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars,
        new CalculationBaseVar(0m),
        new ExtraDamageVar(0m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            ((Func<CardModel, Creature, Decimal>)((card, _) =>
                (Decimal)MpSaveData.CurrentMp * 3))!)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust]; 
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int damage = MpSaveData.CurrentMp * 3;
        MpSaveData.TryConsume(MpSaveData.CurrentMp);
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
        EnergyCost.UpgradeBy(-1);
    }
}