using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class BloodHunt() : BloodMazeCard(2,
    CardType.Skill, CardRarity.Common,TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(2m), new PowerVar<WeakPower>(2m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HemorrhagePower>(CombatState!.HittableEnemies, DynamicVars["HemorrhagePower"].IntValue,this.Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(CombatState!.HittableEnemies, DynamicVars["WeakPower"].IntValue,this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HemorrhagePower"].UpgradeValueBy(1m);
    }
}