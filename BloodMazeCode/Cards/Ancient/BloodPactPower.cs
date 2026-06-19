using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Ancient;


public class BloodPactPower : BloodMazePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public int RestoreAmount { get; set; } = 2;
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
    {
        if (!this.Owner.IsPlayer) return;
        if (this.Owner.IsDead) return;
        if (CombatManager.Instance.IsOverOrEnding) return;
        if (side != this.Owner.Side) return;
        
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), this.Owner, 1m, ValueProp.Unblockable, this.Owner, null);
        MpSaveData.Restore(RestoreAmount);
    }


}
