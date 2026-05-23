using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using BloodMaze.BloodMazeCode.Cards.Basic;
using BloodMaze.BloodMazeCode.Extensions;
using BloodMaze.BloodMazeCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;

namespace BloodMaze.BloodMazeCode.Character;


public class BloodMaze : PlaceholderCharacterModel
{
    public const string CharacterId = "BloodMaze";

    public static readonly Color Color = new("#f0f4ff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 50;

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

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}