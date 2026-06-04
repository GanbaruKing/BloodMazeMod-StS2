using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Token;


public class BloodBag() : BloodMazeCard(0,
    CardType.Skill, CardRarity.Token, TargetType.Self, showInCardLibrary: false)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1m),new PowerVar<RegenPower>(1m), new BlockVar(3m, ValueProp.Move)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Heal(this.Owner.Creature, DynamicVars.Heal.IntValue);
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<RegenPower>(this.Owner.Creature, DynamicVars["RegenPower"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1m);
        DynamicVars["RegenPower"].UpgradeValueBy(1m);
    }
    
    
    
    
    public static async Task<CardModel?> CreateInHand(Player owner, CombatState combatState)
    {
        return (await BloodBag.CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, CombatState combatState)
    {
        if (count == 0) return [];
        if (CombatManager.Instance.IsOverOrEnding) return [];
    
        var cards = new List<CardModel>();
        for (int i = 0; i < count; i++)
            cards.Add(combatState.CreateCard<BloodBag>(owner));
    
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, true);
        return cards;
    }
    
    
}