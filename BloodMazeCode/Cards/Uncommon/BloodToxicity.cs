using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class BloodToxicity() : MpConsumeCard(0,
    CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, 3)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<DebilitatePower>(2m)];
 
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await PowerCmd.Apply<DebilitatePower>(choiceContext, CombatState!.HittableEnemies, this.DynamicVars["DebilitatePower"].BaseValue ,this.Owner.Creature , this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DebilitatePower"].UpgradeValueBy(1m);
    }
}
