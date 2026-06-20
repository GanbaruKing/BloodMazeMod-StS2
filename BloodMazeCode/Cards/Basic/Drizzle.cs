using System.Collections.Generic;
using System.Threading.Tasks;
using BloodMaze.BloodMazeCode.Cards.Token;
using BloodMaze.BloodMazeCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Basic;

public class Drizzle() : MpConsumeCard(0,
    CardType.Attack, CardRarity.Basic, TargetType.AllEnemies, 1)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [..base.CanonicalVars, new DamageVar(4m, ValueProp.Move), new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<BloodFlask>(this.IsUpgraded)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await VampirePlayAllEnemies(choiceContext);

        CardModel? inHand = await BloodFlask.CreateInHand(this.Owner, this.CombatState!);
        if (this.IsUpgraded && inHand != null)
            CardCmd.Upgrade(inHand);
    }

    protected override void OnUpgrade()
    {
    }
}
