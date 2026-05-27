using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Crimson() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        new DisplayVar<Crimson>("HitCount", (_) => MpSaveData.CombatMpConsumeCount.ToString())
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var hitCount = MpSaveData.CombatMpConsumeCount;
        await CommonActions.CardAttack(this, play.Target, hitCount+1)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}