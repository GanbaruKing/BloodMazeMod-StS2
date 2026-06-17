using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class MoneyExchange() : BloodMazeCard(0,
    CardType.Skill, CardRarity.Rare,TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("MpRestore",3m), new CardsVar(1), new EnergyVar(1), new GoldVar(5)];
    
    protected override bool IsPlayable =>
        this.Owner.Creature.Player?.Gold >= DynamicVars.Gold.IntValue;


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, this.Owner.Creature.Player!, GoldLossType.Lost);
        await CardPileCmd.Draw(choiceContext, this.Owner);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, this.Owner);
        MpSaveData.Restore(DynamicVars["MpRestore"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Gold.UpgradeValueBy(-2m);
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
}