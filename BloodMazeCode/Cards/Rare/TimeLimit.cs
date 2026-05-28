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


public class TimeLimit() : BloodMazeCard(0,
    CardType.Skill, CardRarity.Rare,TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new PowerVar<HemorrhagePower>(1m), new PowerVar<RegenPower>(8m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HemorrhagePower>(this.Owner.Creature, DynamicVars["HemorrhagePower"].IntValue,
            this.Owner.Creature, this);
        await PowerCmd.Apply<RegenPower>(this.Owner.Creature, DynamicVars["RegenPower"].IntValue,
            this.Owner.Creature, this);
    }
    

    protected override void OnUpgrade()
    {
        DynamicVars["RegenPower"].UpgradeValueBy(2m);
    }
}