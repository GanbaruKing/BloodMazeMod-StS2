using System.Linq;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Powers;

public class HemorrhagePower : BloodMazePower
{
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerType Type => PowerType.Debuff;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != this.Owner.Side) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;

        await PowerCmd.Apply<HemorrhagePower>(this.Owner, 1m, this.Owner, null);
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;

        decimal damage = this.Amount;
        var players = this.Owner.CombatState?.PlayerCreatures;
        decimal multiplier = 1m + (players?
            .Where(c => c.HasPower<BloodSprayPower>())
            .Select(c => c.GetPower<BloodSprayPower>()!.Multiplier - 1m)
            .DefaultIfEmpty(0m)
            .Sum() ?? 0m);
        damage *= multiplier;

        await CreatureCmd.Damage(choiceContext, this.Owner, damage,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null);
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power != this) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;
        if (this.Amount < 20) return;

        decimal remainder = this.Amount - 20;

        var players = this.Owner.CombatState?.PlayerCreatures;
        decimal multiplier = 1m + (players?
            .Where(c => c.HasPower<BloodSprayPower>())
            .Select(c => c.GetPower<BloodSprayPower>()!.Multiplier - 1m)
            .DefaultIfEmpty(0m)
            .Sum() ?? 0m);

        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), this.Owner, 100m * multiplier,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await CreatureCmd.Stun(this.Owner);
        await PowerCmd.Remove<HemorrhagePower>(this.Owner);

        if (remainder > 0)
            await PowerCmd.Apply<HemorrhagePower>(this.Owner, remainder, applier, cardSource);
        else if (remainder == 0)
            await PowerCmd.Apply<HemorrhagePower>(this.Owner, 1m, applier, cardSource);

        var harvestOwners = players?
            .Where(c => c.HasPower<HarvestPower>());
        if (harvestOwners != null)
        {
            foreach (var creature in harvestOwners)
            {
                var harvest = creature.GetPower<HarvestPower>();
                if (harvest == null) continue;
                int healAmount = harvest.IsUpgraded ? 10 : 7;
                await CreatureCmd.Heal(creature, healAmount);
                MpSaveData.Restore(healAmount);
            }
        }
    }
}