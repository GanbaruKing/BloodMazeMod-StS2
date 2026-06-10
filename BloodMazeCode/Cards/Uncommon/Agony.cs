using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class Agony() : MpConsumeCard(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..base.CanonicalVars,
        new CalculationBaseVar(8m),
        new ExtraDamageVar(1m), 
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            ((Func<CardModel, Creature, decimal>)((card, target) =>
            {

                if (target == null) return 1m;
                return (decimal)target.GetPowerAmount<HemorrhagePower>();
            })!)!)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");

        if (IsVampireForm)
        {

            await CreatureCmd.Damage(choiceContext, Owner.Creature, MpCost,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            AttackCommand attack = await CommonActions
                .CardAttack(this, play.Target, DynamicVars.CalculatedDamage)
                .Execute(choiceContext);

            decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
            await CreatureCmd.Heal(this.Owner.Creature, restore);

            IsVampireForm = false;
            return;
        }

        ConsumeMp();
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}