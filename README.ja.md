# BloodMaze — Slay the Spire 2 追加キャラクター MOD

 [English](README.md) | 日本語

**Slay the Spire 2 に新キャラクター「Revenant（レヴナント）」を追加する MOD。**
戦闘をまたいで持ち越す独自リソース「MP」を中核にした、ブラッドメイジのキャラクターです。


<p align="center">
  <table>
    <tr>
      <td align="center"><img src="docs/revenant_combat.png" width="200" alt="戦闘立ち絵"></td>
      <td align="center"><img src="docs/attack.gif" width="280" alt="攻撃モーション"></td>
    </tr>
  </table>
</p>


---


## 概要

Slay the Spire 2 に、オリジナルのキャラクター・カード・レリックを追加する MOD です。
プレイヤーは「今の戦闘で強い一撃を撃つか、次の戦闘までリソースを温存するか」という、本家には存在しない種類の判断を毎戦闘で迫られます。

- 追加キャラクター: **レヴナント**
- 独自のカード **88 枚**（アタック / スキル / パワー、各レアリティ、協力用カード）
- 新しいバフとデバフを追加
- 協力モード対応


---


## 特徴・ハイライト

- **MP システム** — 戦闘をまたいで持ち越す独自リソース。永続化・UI 表示・セーブスキャム対策まで実装
- **カード設計** — 「MP 攻撃」「大出血」「HP 操作」を組み合わせる新しい戦略
- **カード 88 枚** — MP を消費するというコストの代わりに、汎用的で強力な効果を持つカードが多数
- **立ち絵アニメ** — PNG を `NCreatureVisuals` 化し、AnimationTree で idle / hurt / attack / die を実装

---
  
## このプロジェクトについて

この MOD は、ゲーム開発の学習を目的として個人制作したものです。
ドキュメントの少ない領域で一次情報を掘りながら、設計・実装・アート制作までを一通り経験することを目指して作りました。

---


## 設計コンセプト

**「戦闘をまたぐリソース管理」** がこの MOD の核です。

HP はランを通じて持続するものの、戦略的に消費するリソースではないことが多いです。
そこで、**戦闘のあいだをまたいで意識的に消費する「MP」** を新設しました。

> ボスまで MP を温存したいが、今の戦闘でも使いたい——
> このジレンマが毎戦闘の判断になります。

カードは主に 2 つの軸で構成されています。

| カードの種類 | コスト | メリット | デメリット |
|---|---|---|---|
| MP 消費 | 戦闘またぎの MP 消耗 | 汎用的または無条件の高火力 | 次の戦闘が苦しくなる |
| 出血 | 戦闘内の時間と被ダメージ | MP を温存できる | 戦闘が長引く・遅い |


---


## 導入方法

### 必要環境

- Slay the Spire 2
- [BaseLib](https://github.com/Alchyr/BaseLib-StS2)（前提）

### ダウンロード

- [Nexus Mods（推奨）](coming soon)
- [GitHub Releases](https://github.com/GanbaruKing/BloodMazeMod-StS2/releases)

> ⚠️ MOD の導入は自己責任で行ってください。導入前に必ずセーブデータを退避してください（手順 1）。

### インストール手順

1. **セーブデータを退避する（必須）**
   通常プレイのセーブ破損を防ぐため、先にセーブデータをコピーします。
   `C:\Users\<ユーザー名>\AppData\Roaming\SlayTheSpire2\Steam\<数字の羅列>\profile` 内の `save` フォルダをコピーし、
   `<数字の羅列>` フォルダ内に `modded\profile\` を作成して、そこへ貼り付けます。

2. **`mods` フォルダを作る**
   Steam で Slay the Spire 2 を右クリック →「管理」→「ローカルファイルを閲覧」でゲームフォルダを開き、
   その中に `mods` フォルダが無ければ作成します。

3. **MOD を配置する**
   上記のいずれかから最新版をダウンロードし、解凍したファイルを `mods` フォルダ内にコピーします。

4. **ゲームを起動する**
   起動すると MOD を検知したメッセージが表示されるので、承認すると レヴナント が使用可能になります。



---


## クレジット / 使用ツール

- [BaseLib](https://github.com/Alchyr/BaseLib-StS2) — STS2 MOD 開発用ライブラリ
- [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) — MOD のプロジェクト構成・セットアップの土台として参照
- [Harmony](https://github.com/pardeike/Harmony) — ランタイムパッチング
- Godot Engine — シーン・アニメーション
- Midjourney — カードアート・立ち絵生成
- GIMP — アートの加工や書き直し
- 参考 MOD: [Oddmelt](https://github.com/Alchyr/Oddmelt)



---


## アートについて

このプロジェクトのアートはAIを補助的に使って制作していますが、すべての作品にちゃんとした制作過程があります。

各カードは、まず頭の中で構図を考えるところから始まります。そのイメージをプロンプトとして言葉にし、ベース画像を生成します。その後、編集、描き直し、加筆、クリーンアップ、細かな調整を重ねて仕上げています。最初の生成結果がそのまま理想通りになることはほとんどありません。特に手、クリーチャー、色味、筆の質感などは調整が必要なので、ゲームに合うまで一枚ずつ手を入れています。

AIを使ったアートがすべての人に受け入れられるものではないことは理解しています。それでも、このプロジェクトのすべてのビジュアルアセットには、私自身の努力とこだわりを込めています。

---

## 謝辞

この MOD は [Alchyr](https://github.com/Alchyr) 氏をはじめとする貢献者の方々が開発・公開されている [BaseLib](https://github.com/Alchyr/BaseLib-StS2) と [ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) なくしては成立しませんでした。
ドキュメントの少ない STS2 MOD 開発において、これらのライブラリとテンプレートは開発の土台そのものであり、その丁寧な作り込みから多くを学ばせていただきました。
また、実装例や情報を惜しみなく共有してくださった STS2 modding コミュニティの皆さまにも、深く感謝いたします。
基盤を築き、知見を分かち合ってくださったすべての方々に、心よりお礼申し上げます。

*Special thanks to [Alchyr](https://github.com/Alchyr) and all the contributors behind BaseLib and ModTemplate-StS2, as well as the STS2 modding community. This mod would not have been possible without them.*


---


## ライセンス

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
