using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class Foresight() : MpConsumeCard(5, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DamageVar(24m, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attack = await VampirePlay(choiceContext, play.Target);
        
    }

    public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
    {
        if (side == CombatSide.Player
            && this.Owner.PlayerCombatState!.Hand.Cards.Contains(this))
        {
            int current = EnergyCost.GetWithModifiers((CostModifiers)2);
            if (current > 0)
                EnergyCost.SetThisCombat(current - 1, false);
        }
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
