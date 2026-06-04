using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class HealingSpirit() : MpConsumeCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 8)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new HealVar(10m), new CardsVar(1), new EnergyVar(1)];
 
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Retain];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CreatureCmd.Heal(this.Owner.Creature, DynamicVars.Heal.IntValue);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.IntValue, this.Owner);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, this.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1m);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}