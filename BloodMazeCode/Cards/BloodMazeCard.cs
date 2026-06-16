using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using BloodMaze.BloodMazeCode.Character;
using BloodMaze.BloodMazeCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BloodMaze.BloodMazeCode.Cards;

[Pool(typeof(BloodMazeCardPool))]
public abstract class BloodMazeCard(int cost, CardType type, CardRarity rarity, TargetType target,
    bool showInCardLibrary = true, bool autoAdd = true) :
    CustomCardModel(cost, type, rarity, target, showInCardLibrary, autoAdd)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    
    protected async Task<AttackCommand> VampireAttack(PlayerChoiceContext choiceContext, Creature? target)
    {
        AttackCommand attack = await CommonActions.CardAttack(this, target).Execute(choiceContext);
        decimal restore = attack.Results.Sum(r => r.TotalDamage + r.OverkillDamage - r.BlockedDamage);
        if(restore >0)
            await CreatureCmd.Heal(this.Owner.Creature, restore);
        return attack;
    }
    
    
}