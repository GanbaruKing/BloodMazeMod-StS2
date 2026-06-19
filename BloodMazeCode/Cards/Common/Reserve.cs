using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class Reserve() : BloodMazeCard(1,
    CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    private int _turnStartConsumeCount;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(8m, ValueProp.Move), new EnergyVar(2)];

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
    {
        if (side == CombatSide.Player)
            _turnStartConsumeCount = MpSaveData.CombatMpConsumeCount;
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (MpSaveData.CombatMpConsumeCount == _turnStartConsumeCount)
            await PlayerCmd.GainEnergy((Decimal)DynamicVars.Energy.IntValue, this.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
}
