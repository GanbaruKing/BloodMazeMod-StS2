using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Powers;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Uncommon;


public class GoreArmor() : BloodMazeCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HemorrhagePowerTipVar(), new BlockVar("BaseBlock",8m, ValueProp.Move), new BlockVar("ExtraBlock",12m, ValueProp.Move)];

    protected override bool ShouldGlowGoldInternal => this.WasHemorrhageAppliedThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars["BaseBlock"], play);
        if (WasHemorrhageAppliedThisTurn)
        {
            await CommonActions.CardBlock(this, DynamicVars["ExtraBlock"], play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BaseBlock"].UpgradeValueBy(2m);
        DynamicVars["ExtraBlock"].UpgradeValueBy(2m);
    }

    private bool WasHemorrhageAppliedThisTurn => 
        CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
            .Any((Func<PowerReceivedEntry, bool>)(e => e.HappenedThisTurn(this.CombatState) && e.Power is HemorrhagePower && e.Applier == this.Owner.Creature));
}