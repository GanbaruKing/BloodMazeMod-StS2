using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class Hocuspocus() : MpConsumeCard(0,
    CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, 5)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DynamicVar("DamageDecrease", 30M),(DynamicVar) new RepeatVar(2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await PowerCmd.Apply<ShrinkPower>(play.Target!, DynamicVars.Repeat.BaseValue, this.Owner.Creature, (CardModel) this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}