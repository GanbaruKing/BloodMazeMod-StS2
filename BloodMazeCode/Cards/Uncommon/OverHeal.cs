using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class OverHeal() : BloodMazeCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<OverHealPower>(1m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        bool isFirstApply = !Owner.Creature.HasPower<OverHealPower>();
        int savedMaxHp = Owner.Creature.MaxHp;
        
        
        await CommonActions.ApplySelf<OverHealPower>(choiceContext, this);

        if (isFirstApply)
        {
            OverHealPower power = Owner.Creature.GetPower<OverHealPower>()!; 
            power!.SavedMaxHp = savedMaxHp;
        }

        
        await CreatureCmd.SetMaxHp(Owner.Creature, 999m);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}