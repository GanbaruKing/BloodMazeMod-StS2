using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Mp;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards;

public abstract class MpConsumeCard(int cost, CardType type, CardRarity rarity, TargetType target, int mpCost)
    : BloodMazeCard(cost, type, rarity, target)
{
    protected readonly int MpCost = mpCost;
    
    public override int CanonicalStarCost => MpCost;
    
    public bool IsFreeThisPlay { get; set; } = false;
    public bool IsVampireForm { get; set; } = false;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<MpConsumeCard>("ConsumeMp", (_) => MpCost.ToString()),
    ];

    protected override bool IsPlayable => 
        Owner.Creature.HasPower<FreeMpAttackPower>() ||
        MpSaveData.CurrentMp >= MpCost;
        private int _consumeCallsThisCycle = 0;

        protected void ConsumeMp()
        {
            if (IsFreeThisPlay) { IsFreeThisPlay = false; return; }

            int totalPlays = GetEnchantedReplayCount() + 1; 

            _consumeCallsThisCycle++;
            if (_consumeCallsThisCycle == 1)
            {
                MpSaveData.TryConsume(MpCost);
            }
            
            if (_consumeCallsThisCycle >= totalPlays)
            {
                _consumeCallsThisCycle = 0;
            }
        }
    protected async Task VampirePlay(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (IsVampireForm)
        {
            await CreatureCmd.Damage(choiceContext, this.Owner.Creature, MpCost,
                ValueProp.Unblockable | ValueProp.Unpowered, this);
            await VampireAttack(choiceContext, target);
            IsVampireForm = false;
        }
        else
        {
            ConsumeMp();
            await CommonActions.CardAttack(this, target).Execute(choiceContext);
        }
    }
    
    
    
    protected async Task VampirePlayAllEnemies(PlayerChoiceContext choiceContext)
    {
        if (IsVampireForm)
        {
            await CreatureCmd.Damage(choiceContext, this.Owner.Creature, MpCost,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            AttackCommand attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).TargetingAllOpponents(this.CombatState!).Execute(choiceContext);
            decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
            await CreatureCmd.Heal(this.Owner.Creature, restore);
            IsVampireForm = false;
        }
        else
        {
            ConsumeMp();
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).TargetingAllOpponents(this.CombatState!).Execute(choiceContext);
        }
    }
    
    
    protected async Task VampirePlayMultiHit(PlayerChoiceContext choiceContext, Creature? target, int hitCount)
    {
        if (IsVampireForm)
        {
            await CreatureCmd.Damage(choiceContext, this.Owner.Creature, MpCost,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            AttackCommand attack = await CommonActions.CardAttack(this, target, hitCount).Execute(choiceContext);
            decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
            await CreatureCmd.Heal(this.Owner.Creature, restore);
            IsVampireForm = false;
        }
        else
        {
            ConsumeMp();
            await CommonActions.CardAttack(this, target, hitCount).Execute(choiceContext);
        }
    }
    
}