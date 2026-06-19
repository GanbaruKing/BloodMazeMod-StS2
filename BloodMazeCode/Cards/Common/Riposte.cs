using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Common;



public class Riposte() : BloodMazeCard(1,
    CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(8m, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attack = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);

        decimal dealt = attack.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage - r.BlockedDamage);
        decimal block = Math.Floor(dealt / 2m);

        if (block > 0)
            await CreatureCmd.GainBlock(this.Owner.Creature, block, ValueProp.Move, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
