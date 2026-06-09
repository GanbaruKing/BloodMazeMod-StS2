using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class Innovation() : MpConsumeCard(1,
    CardType.Skill, CardRarity.Uncommon ,TargetType.Self, 2)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), ..base.CanonicalVars, new HpLossVar(3m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ConsumeMp();
        await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel? inHand = await BloodFlask.CreateInHand(this.Owner, this.CombatState!);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}