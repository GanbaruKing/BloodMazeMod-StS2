using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;



public class Grudge() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private decimal RetainGain = 4m;
    
    private decimal _retainBonus;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4m, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }
    
    public override Task AfterTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Player
            && !this.Owner.Creature.IsDead
            && this.Owner.PlayerCombatState!.Hand.Cards.Contains(this))
        {
            DynamicVars.Damage.UpgradeValueBy(RetainGain);
            _retainBonus += RetainGain;
        }

        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        ResetBonus();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ResetBonus();
        return Task.CompletedTask;
    }
    
    private void ResetBonus()
    {
        if (_retainBonus != 0m)
        {
            DynamicVars.Damage.UpgradeValueBy(-_retainBonus);
            _retainBonus = 0m;
        }
    }

    protected override void OnUpgrade()
    {
        RetainGain = 5m;
    }
}