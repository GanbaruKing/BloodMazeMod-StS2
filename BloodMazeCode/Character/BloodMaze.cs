using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using BloodMaze.BloodMazeCode.Cards.Basic;
using BloodMaze.BloodMazeCode.Extensions;
using BloodMaze.BloodMazeCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace BloodMaze.BloodMazeCode.Character;


public class BloodMaze : PlaceholderCharacterModel
{
    public const string CharacterId = "Revenant"; 

    public static readonly Color Color = new("#f0f4ff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 60;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeBloodMaze>(),
        ModelDb.Card<StrikeBloodMaze>(),
        ModelDb.Card<StrikeBloodMaze>(),
        ModelDb.Card<StrikeBloodMaze>(),
        ModelDb.Card<DefendBloodMaze>(),
        ModelDb.Card<DefendBloodMaze>(),
        ModelDb.Card<DefendBloodMaze>(),
        ModelDb.Card<DefendBloodMaze>(),
        ModelDb.Card<Blizzard>(),
        ModelDb.Card<Drizzle>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [ModelDb.Relic<ManaOrb>()];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<BloodMazeCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<BloodMazeRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<BloodMazePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    
    public override string CustomIconTexturePath => "character_icon_revenant.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_maker_revenant.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_revenantV2.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_revenant_lockedV2.png".CharacterUiPath();
    
    public override string CustomCharacterSelectBg
        => "res://BloodMaze/scenes/char_select_bg_revenant.tscn";
    
    public override string CustomVisualPath
        => "res://BloodMaze/scenes/revenant_combat.tscn";
    
    public override string CustomRestSiteAnimPath
        => "res://BloodMaze/scenes/revenant_rest_site.tscn";

    public override string CustomMerchantAnimPath
        => "res://BloodMaze/scenes/revenant_merchant.tscn";
}


