using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;  


public class Armageddon() : MpConsumeCard(1,
    CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, 4)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int hits = IsUpgraded ? 5 : 4;
        await VampirePlayAllEnemiesMultiHit(choiceContext, hits);
    }
}