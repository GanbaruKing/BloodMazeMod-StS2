using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Cards.Token;
using BloodMaze.BloodMazeCode.Mp;
using BloodMaze.BloodMazeCode.Tips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BloodMaze.BloodMazeCode.Cards.Rare;


public class Crimson() : BloodMazeCard(2,
    CardType.Attack, CardRarity.Rare, TargetType.Self)
{
    //変更前
    /* protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        new DisplayVar<Crimson>("HitCount", (_) => MpSaveData.CombatMpConsumeCount.ToString())
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var hitCount = MpSaveData.CombatMpConsumeCount;
        await CommonActions.CardAttack(this, play.Target, hitCount+1)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    } */
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<Crimson>("PlayCount", (_) => MpSaveData.CombatMpConsumeCount.ToString())
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int count = MpSaveData.CombatMpConsumeCount;
        if (count <= 0) return;

        MpSaveData.ResetCombatMpConsumeCount();
        Player player = this.Owner;

        var pool = player.Character.CardPool
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .Where(c => c is not Crimson).Where(c => c is not BloodFlask)                          // ① トークン(血液パック)を除外
            .Where(c => c is not ITranscendenceCard)             
            .Where(c => c.Rarity is CardRarity.Common
                or CardRarity.Uncommon
                or CardRarity.Rare);

        var generated = CardFactory.GetForCombat(
            player, pool, count, player.RunState.Rng.CombatCardGeneration).ToList();

        if (generated.Count == 0) return;

        foreach (var card in generated)
        {
            await CardPileCmd.AddGeneratedCardToCombat(
                card, PileType.Draw, player, CardPilePosition.Top);

            card.SetToFreeThisTurn();        
            if (card is MpConsumeCard mp)
                mp.IsFreeThisPlay = true;      
        }

        await CardPileCmd.AutoPlayFromDrawPile(
            choiceContext, player, generated.Count, CardPilePosition.Top, false);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

}
