# \[Apolonia] — Turn-Based Mini JRPG (Unity)

> A turn-based JRPG mini-game prototype, built as a \*\*Pre-Interview Test for the Game Programmer position at Toge Productions\*\*.

\---

## 📖 About

This project demonstrates the core gameplay loop of a JRPG at prototype scale: map exploration, NPC interaction, dialogue, automated cinematic cutscenes, and a full turn-based battle system. Beyond gameplay features, the main focus of this project is **code architecture** — how systems are structured to stay readable, extensible, and aligned with design patterns commonly used in the industry.

## 🎮 Gameplay Features

|Feature|Status|Implementation|
|-|-|-|
|World map exploration (WASD / Arrow Keys)|✅|`PlayerInput.cs`, `CharacterMovement.cs`|
|NPC/object interaction (**E** key)|✅|`NPCInteractable.cs`|
|Dialogue (single-line \& multi-line sequences)|✅|`SimpleDialogueManager.cs`, `DialogueManager.cs`|
|Yes/No branching dialogue|✅|`ChoiceDialogueManager.cs`, `SimpleDialogueManager.cs`|
|In-game cutscene (character moves automatically)|✅|`CutsceneController.cs`|
|JRPG-style turn-based battle|✅|`BattleManager.cs`, `IBattleAction` + implementations|
|Camera follow|✅|`CameraFollow\_3D.cs`|
|Global background music|✅|`BGMManager.cs`|
|Main menu|✅|`MenuManager.cs`|

> Note: the original brief specified the \*\*Space\*\* key for NPC interaction; this implementation uses the \*\*E\*\* key instead — adjust as needed to match the original spec if required.

## 🏗️ Architecture \& Design Patterns

### 1\. Strategy Pattern — Battle Action System

The `IBattleAction` interface defines a single `Execute(attacker, target)` contract, implemented by three independent classes:

```
IBattleAction (interface)
 ├── AttackAction   — random 1\~ATK damage with a critical hit chance
 ├── SkillAction    — 1\~ATK × 1.5 damage, consumes MP
 └── GuardAction    — reduces incoming damage on the next hit
```

`BattleManager` never needs to know the internal details of each action — it simply calls `currentAction.Execute(...)`. Adding a new action type (e.g. Item, Ultimate) only requires creating a new class that implements `IBattleAction`, without touching `BattleManager` at all (Open/Closed Principle).

### 2\. ScriptableObject — Dialogue Data

`DialogueSequence` (`\[CreateAssetMenu(menuName = "JRPG/Dialogue Sequence")]`) stores a list of `DialogueLine` entries (speaker name + sentence). This lets dialogue/cutscene content be authored as Unity assets (`.asset`) entirely inside the Editor, without touching code — well-suited to a workflow where a writer or designer fills in content independently of the programmer.

### 3\. Singleton — Cross-Scene State

* `GameManager` — handles scene loading and application quit, persisted via `DontDestroyOnLoad`.
* `GameSessionManager` — holds the player's `CharacterStats` so HP/MP/stats stay consistent when moving from the world map into a battle scene.
* `BattleManager` \& `BGMManager` — single instance per their respective scene.

### 4\. Separation of Concerns by Folder

```
Script/
├── Battle/       → Battle logic (BattleManager, IBattleAction \& its implementations)
├── Character/    → Character data \& behavior (BaseCharacter, PlayerCharacter, EnemyCharacter, stats, animator, HP/MP UI)
├── Core/         → Cross-scene core systems (GameManager, PlayerInput, CharacterMovement, BGMManager)
├── Dialogue/     → Dialogue ScriptableObjects \& data assets
├── Session/      → Cross-scene player data persistence (GameSessionManager)
├── UI/           → Dialogue \& menu UI managers
└── World/        → Outdoor-world interactions (NPC, cutscenes, scene transition triggers, camera, sprite sorting)
```

`BaseCharacter` is an abstract class that both `PlayerCharacter` and `EnemyCharacter` inherit from, so `TakeDamage`/`Heal`/`IsDead` logic isn't duplicated between the player and enemies.

## 🚀 Getting Started

1. Clone this repository
2. Open the project with **Unity \[fill in Unity version used]**
3. Open the `MainMenu` scene in `Assets/Scenes`
4. Press Play

**Controls:**

* `W A S D` / Arrow Keys — move on the world map
* `E` — interact with NPCs / advance dialogue
* UI buttons — select a battle action (Attack / Skill / Guard)

## 📦 Build \& Demo

* 🔗 Downloadable build: `https://drive.google.com/drive/folders/14VSQ2We3YVO3Lie5mNCO5MoJRyY50mWP?usp=drive\_link`
* 🎥 Gameplay video: `https://drive.google.com/file/d/1fN6y39lbuOknzOgMqT73TWPItBLfADJi/view?usp=drive\_link`
* 📁 Google Drive folder (pre-interview test): `https://drive.google.com/drive/folders/1UZCzPdo2Ii1EGnwsxGlKQtvf7jHHFt\_J?usp=drive\_link`

## 🛠️ Tools

* **Engine:** Unity (hybrid 2D/3D, sprites with Z-position-based sorting for a 2.5D look)
* **Language:** C#
* **Design Patterns:** Strategy, Singleton, and data separation via ScriptableObject

## 📌 Development Notes

A few known areas for further improvement (kept transparent for review purposes):

* Dialogue currently has two parallel implementations (`DialogueManager` \& `SimpleDialogueManager`) that could ideally be consolidated to avoid duplication.
* `EnemyAI` in `BattleManager` still relies on simple probability without factoring in the player's current HP.
* Some cross-object references in the duel cutscene rely on `GameObject.Find` by name (e.g. `"Enemy"`, `"Arena\_Gate"`), which is fragile if scene object names change — ideally replaced with direct references or tags.

\---

Developed as part of the Game Programmer Pre-Interview Test — Toge Productions.

