using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Cards.Variables;
using BloodMaze.BloodMazeCode.Mp;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Common;


public class BloodPrice() : BloodMazeCard(0,
    CardType.Skill, CardRarity.Common, TargetType.Self)
{
    private int _mprestore = 5;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(2m),new DisplayVar<BloodPrice>("MpRestore", (_) => _mprestore.ToString()), new CardsVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.IntValue, this.Owner);
        MpSaveData.Restore(_mprestore);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}