# BloodMaze — A Custom Character Mod for Slay the Spire 2

**Adds a new character, "Revenant," to Slay the Spire 2.**
A blood mage built around **MP**, a custom resource that carries over between battles.

*[日本語版 / Japanese version](README.ja.md)*

<p align="center">
<table>
<tr>
<td align="center"><img src="docs/revenant_combat.png" width="200" alt="Revenant combat sprite"></td>
<td align="center"><img src="docs/attack.gif" width="280" alt="Attack animation"></td>
</tr>
</table>
</p>

---

## Overview

BloodMaze adds an original character, cards, and relics to Slay the Spire 2.
Every fight forces a decision the base game never asks: **spend resources for a powerful hit now, or save them for the battles ahead.**

- New character: **Revenant**
- **88 custom cards** (Attack / Skill / Power, all rarities, plus co-op cards)
- New buffs and debuffs
- Co-op mode support

---

## Highlights

- **MP system** — a custom resource that persists across battles, with full persistence, a custom UI display, and save-scum protection.
- **Card design** — a new strategic axis combining MP attacks, heavy bleed, and HP manipulation.
- **88 cards** — instead of paying mana, many cards spend MP in exchange for broadly useful, high-impact effects.
- **Animated sprites** — PNGs converted into `NCreatureVisuals`, with idle / hurt / attack / die states driven by an AnimationTree.

---

## Design Concept

**Managing resources across battles** is the core of this mod.

HP persists through a run, but it's rarely something you spend strategically. So BloodMaze introduces **MP — a resource you deliberately spend, and that carries between fights.**

> You want to save MP for the boss, but you also want to use it right now.
> That dilemma is the decision you face every battle.

Cards are built along two main axes:

| Card type | Cost | Upside | Downside |
|---|---|---|---|
| MP-spending | MP carried across battles | Versatile or unconditional high damage | Makes the next battle harder |
| Bleed | Time and damage taken within the fight | Preserves your MP | Fights drag on and slow down |

---

## Installation

### Requirements

- Slay the Spire 2
- [BaseLib](https://github.com/Alchyr/BaseLib-StS2) (required dependency)

### Download

- Nexus Mods (recommended) — *coming soon*
- [GitHub Releases](https://github.com/GanbaruKing/BloodMazeMod-StS2/releases)

> ⚠️ Install mods at your own risk. **Always back up your save data before installing (Step 1).**

### Steps

1. **Back up your save data (required).**
   To avoid corrupting your normal save, copy it first.
   Copy the `save` folder inside `C:\Users\<username>\AppData\Roaming\SlayTheSpire2\Steam\<numeric id>\profile`,
   then create `modded\profile\` inside the `<numeric id>` folder and paste it there.

2. **Create a `mods` folder.**
   In Steam, right-click Slay the Spire 2 → **Manage** → **Browse local files** to open the game folder, and create a `mods` folder there if one doesn't exist.

3. **Install the mod.**
   Download the latest version from one of the links above, unzip it, and copy the files into the `mods` folder.

4. **Launch the game.**
   On startup you'll see a message that a mod was detected. Approve it, and Revenant will be available to play.

---

## Technical Highlights

- **Custom resource persistence** — MP's current and maximum values are saved to JSON, with rollback protection against mid-battle reloads and save scumming.
- **UI extension via Harmony patches** — an MP bar is drawn dynamically beneath the HP bar. By riding on the official Star (secondary cost) UI, a card's MP cost is shown without adding badge nodes to the card itself.
- **Sprite generation from a single PNG** — ordinary Node2D scenes are auto-converted into `NCreatureVisuals` through `CustomVisualPath`, with automatic return to idle handled by an AnimationTree StateMachine.
- **Animation from few frames** — starting from a limited set of difference images, motions like *attack* are composed by keying the sprite's position, scale, and playback speed.
- **Image-processing pipeline** — generated images are touched up and color-adjusted into finished sprites and card art.
- **Merchant & rest-site portraits** — Node2D scenes structured to match the generation specs of `NMerchantCharacter` / `NRestSiteCharacter`.

---

## Development Notes

Documentation for BaseLib and STS2 modding is scarce online. To work around that, I developed this mod by **decompiling the game and BaseLib to read the source directly.**

- **Comparing working vs. broken examples** — when the cause of a bug was unclear, I placed a correctly working card next to a misbehaving one and let the shared/differing traits reveal the underlying cause. I used this approach heavily.

Digging out primary sources by hand in an under-documented area was the main way this was built.

---

## Credits / Tools Used

- [BaseLib](https://github.com/Alchyr/BaseLib-StS2) — library for STS2 mod development
- [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) — referenced as the basis for project structure and setup
- [Harmony](https://github.com/pardeike/Harmony) — runtime patching
- Godot Engine — scenes and animation
- Midjourney — card art and sprite generation
- Python + OpenCV — background removal for sprites
- Reference mod: [Oddmelt](https://github.com/Alchyr/Oddmelt)
- DALL·E 3 — assistance with image generation and editing

---

## About This Project

This mod was made solo as a way to learn C# and game development.
The goal was to experience the full pipeline — design, implementation, and art — while digging out primary sources in a sparsely documented area.
The card art and character sprites started from images generated with Midjourney and DALL·E 3, then were edited toward the intended look: touch-ups for a painterly feel, color and contrast correction, and emphasis to bring out the subject.

---

## Acknowledgements

This mod would not exist without [BaseLib](https://github.com/Alchyr/BaseLib-StS2) and [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2), developed and shared by [Alchyr](https://github.com/Alchyr) and the other contributors.
In the under-documented world of STS2 modding, these libraries and templates were the very foundation I built on, and I learned a great deal from their careful craftsmanship.
My thanks also go to the STS2 modding community, who shared examples and knowledge so generously.

*Special thanks to [Alchyr](https://github.com/Alchyr) and all the contributors behind BaseLib and ModTemplate-StS2, as well as the STS2 modding community. This mod would not have been possible without them.*

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
