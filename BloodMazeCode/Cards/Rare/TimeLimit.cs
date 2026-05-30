using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class TimeLimit() : BloodMazeCard(1,
    CardType.Power, CardRarity.Rare,TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(1m), new PowerVar<RegenPower>(10m), new PowerVar<TimeLimitPower>(2m)];



    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RegenPower>(this.Owner.Creature, DynamicVars["RegenPower"].IntValue,
            this.Owner.Creature, this);
        await PowerCmd.Apply<TimeLimitPower>(this.Owner.Creature, DynamicVars["TimeLimitPower"].IntValue,
            this.Owner.Creature, this);
        await PowerCmd.Apply<HemorrhagePower>(this.Owner.Creature, DynamicVars["HemorrhagePower"].IntValue,
            this.Owner.Creature, this);
    }
    

    protected override void OnUpgrade()
    {
        DynamicVars["TimeLimitPower"].UpgradeValueBy(1m);
        DynamicVars["RegenPower"].UpgradeValueBy(1m);
    }
}