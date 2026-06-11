# BloodMaze — A Custom Character Mod for Slay the Spire 2

English | [日本語](README.ja.md)

**A mod that adds a new character, "Revenant," to Slay the Spire 2.**
A blood-mage built around **MP**, a custom resource that carries over between combats.


<p align="center">
  <table>
    <tr>
      <td align="center"><img src="docs/revenant_combat.png" width="200" alt="Combat sprite"></td>
      <td align="center"><img src="docs/attack.gif" width="280" alt="Attack animation"></td>
    </tr>
  </table>
</p>


---


## Overview

A mod that adds an original character, cards, and relics to Slay the Spire 2.
Every fight forces a kind of decision the base game doesn't have: **spend big now, or save your resources for the combats ahead?**

- New character: **Revenant**
- **88 custom cards** (Attacks / Skills / Powers, across rarities, including co-op cards)
- New buffs and debuffs
- Co-op mode support


---


## Highlights

- **MP system** — A custom resource carried across combats. Persistence, UI display, and save-scum protection all implemented.
- **Card design** — A new strategic layer combining "MP attacks," "heavy bleed," and "HP manipulation."
- **88 cards** — Many offer broadly useful, powerful effects in exchange for the cost of spending MP.
- **Animated sprites** — PNGs are turned into `NCreatureVisuals`, with idle / hurt / attack / die driven by an AnimationTree.

---

## About This Project

This mod was built solo as a way to learn C# and game development.
The goal was to go through the whole pipeline — design, implementation, and art — while digging up primary information firsthand in an area with little documentation.

---


## Design Concept

**"Resource management that spans combats"** is the core of this mod.

HP persists across a run, but it's rarely a resource you spend strategically.
So I added **MP — a resource you consciously spend across the gaps between fights.**

> You want to save MP for the boss, but you also want to use it right now —
> that dilemma becomes a decision in every combat.

Cards are built mainly along two axes:

| Card type | Cost | Upside | Downside |
|---|---|---|---|
| MP cost | MP spent across combats | Versatile or unconditional high damage | Later fights get harder |
| Bleed | Time and damage taken within a fight | Preserves MP | Fights drag on and slow down |


---


## Installation

### Requirements

- Slay the Spire 2
- [BaseLib](https://github.com/Alchyr/BaseLib-StS2) (required)

### Download

- [Nexus Mods (recommended)](coming soon)
- [GitHub Releases](https://github.com/GanbaruKing/BloodMazeMod-StS2/releases)

> ⚠️ Install mods at your own risk. Always back up your save data before installing (Step 1).

### Steps

1. **Back up your save data (required)**
   To avoid corrupting your normal save, copy it first.
   Copy the `save` folder inside `C:\Users\<username>\AppData\Roaming\SlayTheSpire2\Steam\<numeric ID>\profile`,
   then create `modded\profile\` inside the `<numeric ID>` folder and paste it there.

2. **Create a `mods` folder**
   In Steam, right-click Slay the Spire 2 → "Manage" → "Browse local files" to open the game folder,
   and create a `mods` folder if one doesn't already exist.

3. **Install the mod**
   Download the latest version from one of the links above, unzip it, and copy the files into the `mods` folder.

4. **Launch the game**
   On startup you'll see a message that the mod was detected; approve it, and Revenant becomes playable.


---


## Technical Highlights

- **Custom resource persistence** — MP's current and max values are saved as JSON. Includes rollback protection against save-scumming and against interrupting/resuming combat.
- **UI extension via Harmony patches** — An MP bar is drawn dynamically below the HP bar. By riding on the official Star (secondary cost) UI, MP cost is shown without adding badge nodes to cards.
- **Sprite generation from a single PNG** — A regular Node2D scene is auto-converted into `NCreatureVisuals` via `CustomVisualPath`, with an AnimationTree StateMachine handling automatic return to idle.
- **Animation from few frames** — Starting from a limited set of difference images, motion like attacks is built by keying the sprite's position, scale, and playback speed.
- **Merchant / campfire sprites** — Node2D scene structures matching the generation requirements of `NMerchantCharacter` / `NRestSiteCharacter`.

---


## Dev Notes / Behind the Scenes

There's little documentation online for BaseLib and STS2 modding, and not enough to go on.
So I developed by **decompiling the game itself and BaseLib to read the primary information directly.**

- **Comparing working vs. non-working examples** — When the cause of a bug was unclear, I'd line up a card that behaved correctly against one that didn't, and surface the underlying cause from their similarities and differences. I used this often.

Digging up primary information firsthand in an undocumented area was the main way I built this.

---


## Credits / Tools

- [BaseLib](https://github.com/Alchyr/BaseLib-StS2) — Library for STS2 mod development
- [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) — Referenced as the basis for project structure and setup
- [Harmony](https://github.com/pardeike/Harmony) — Runtime patching
- Godot Engine — Scenes and animation
- Midjourney — Card art and sprite generation
- Python + OpenCV — Background removal for sprites
- Reference mod: [Oddmelt](https://github.com/Alchyr/Oddmelt)
- DALL·E 3 — Image generation and editing support



---


## About the Art

The art is made using AI. But I put real work into every single piece.

Every card starts as a composition in my head. I put it into words as a prompt,
generate it, and — since what I'm after rarely comes out on the first try (hands and
creatures especially, along with the feel of the colors and brushwork) — I redraw and
retouch it by hand until I'm satisfied. I spent time on all of it, and I cared about all of it.

The AI is just a tool. The composition, the fixes, and the heart in it are mine.
I know AI-generated art isn't to everyone's taste, but I hope you won't dislike my work for it.

---

## Acknowledgements

This mod would not exist without [BaseLib](https://github.com/Alchyr/BaseLib-StS2) and [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2), developed and shared by [Alchyr](https://github.com/Alchyr) and the other contributors.
In STS2 mod development, where documentation is scarce, these libraries and templates were the very foundation of my work, and I learned a great deal from how carefully they were built.
I'm also deeply grateful to everyone in the STS2 modding community who generously shared implementation examples and information.
My sincere thanks to all those who laid the groundwork and shared their knowledge.

*Special thanks to [Alchyr](https://github.com/Alchyr) and all the contributors behind BaseLib and ModTemplate-StS2, as well as the STS2 modding community. This mod would not have been possible without them.*


---


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
