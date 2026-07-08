# \[Apolonia] — Mini JRPG Turn-Based (Unity)

> Prototipe mini-game RPG turn-based bergaya JRPG klasik, dikerjakan sebagai \*\*Pre-Interview Test untuk posisi Game Programmer di Toge Productions\*\*.

\---

## 📖 Tentang Project

Project ini mendemonstrasikan alur gameplay inti sebuah JRPG dalam skala prototipe: eksplorasi map, interaksi NPC, dialog, cutscene, hingga pertempuran turn-based. Selain fitur gameplay, fokus utama project ini adalah pada **arsitektur kode** — bagaimana sistem disusun agar mudah dibaca, mudah diperluas, dan mengikuti design pattern yang umum dipakai di industri game.

## 🎮 Fitur Gameplay

|Fitur|Status|Implementasi|
|-|-|-|
|Eksplorasi world map (WASD / Arrow Key)|✅|`PlayerInput.cs`, `CharacterMovement.cs`|
|Interaksi NPC/objek (tombol **E** / tombol **Space**)|✅|`NPCInteractable.cs`|
|Dialog (single-line \& sequence bertingkat)|✅|`SimpleDialogueManager.cs`, `DialogueManager.cs`|
|Dialog dengan pilihan Yes/No|✅|`ChoiceDialogueManager.cs`, `SimpleDialogueManager.cs`|
|In-game cutscene (karakter bergerak otomatis)|✅|`CutsceneController.cs`|
|Battle turn-based ala JRPG|✅|`BattleManager.cs`, `IBattleAction` + implementasinya|
|Kamera mengikuti player|✅|`CameraFollow\_3D.cs`|
|Musik latar|✅|`BGMManager.cs`|
|Main menu|✅|`MenuManager.cs`|



## 🏗️ Arsitektur \& Design Pattern

### 1\. Strategy Pattern — Sistem Aksi Battle

Interface `IBattleAction` mendefinisikan kontrak `Execute(attacker, target)`, diimplementasikan oleh tiga class terpisah:

```
IBattleAction (interface)
 ├── AttackAction   — damage acak 1\~ATK + peluang critical
 ├── SkillAction    — damage 1\~ATK × 1.5, mengurangi MP
 └── GuardAction    — mengurangi damage yang diterima di giliran berikutnya
```

`BattleManager` tidak perlu tahu detail masing-masing aksi — ia cukup memanggil `currentAction.Execute(...)`. Menambah jenis serangan baru (misal Item, Ultimate) cukup dengan membuat class baru yang mengimplementasikan `IBattleAction`, tanpa mengubah `BattleManager` sama sekali (Open/Closed Principle).

### 2\. ScriptableObject — Data Dialog

`DialogueSequence` (`\[CreateAssetMenu(menuName = "JRPG/Dialogue Sequence")]`) menyimpan daftar `DialogueLine` (nama pembicara + kalimat). Dengan ini, konten dialog/cutscene bisa dibuat sebagai asset (`.asset`) di Unity Editor tanpa menyentuh kode sama sekali — cocok untuk workflow di mana writer/designer mengisi konten secara independen dari programmer.

### 3\. Singleton — State Lintas Scene

* `GameManager` — mengelola load scene dan quit aplikasi, persist via `DontDestroyOnLoad`.
* `GameSessionManager` — menyimpan `CharacterStats` milik pemain agar HP/MP/stat tetap konsisten saat berpindah dari world map ke battle scene.
* `BattleManager` \& `BGMManager` — instance tunggal per scene-nya masing-masing.

### 4\. Pemisahan Tanggung Jawab per Folder

```
Script/
├── Battle/       → Logika pertempuran (BattleManager, IBattleAction \& implementasinya)
├── Character/    → Data \& perilaku karakter (BaseCharacter, PlayerCharacter, EnemyCharacter, stats, animator, UI HP/MP)
├── Core/         → Sistem inti lintas-scene (GameManager, PlayerInput, CharacterMovement, BGMManager)
├── Dialogue/     → ScriptableObject dialog \& data asset
├── Session/      → Persistensi data pemain lintas scene (GameSessionManager)
├── UI/           → Manajer UI dialog \& menu
└── World/        → Interaksi dunia luar (NPC, cutscene, trigger transisi scene, kamera, sprite sorting)
```

`BaseCharacter` sebagai kelas abstrak menjadi induk dari `PlayerCharacter` dan `EnemyCharacter`, sehingga logika `TakeDamage`/`Heal`/pengecekan `IsDead` tidak terduplikasi antara pemain dan musuh.

## 🚀 Cara Menjalankan

1. Clone repository ini
2. Buka project menggunakan **Unity \[isi versi Unity yang dipakai]**
3. Buka scene `MainMenu` di folder `Assets/Scenes`
4. Tekan Play

**Kontrol:**

* `W A S D` / Arrow Key — bergerak di world map
* `E` — interaksi dengan NPC / lanjut dialog
* Tombol UI — memilih aksi battle (Attack / Skill / Guard)

## 📦 Build \& Demo

* 🔗 Link build (download): `https://drive.google.com/drive/folders/14VSQ2We3YVO3Lie5mNCO5MoJRyY50mWP?usp=drive\\\_link`
* 🎥 Video gameplay: `https://drive.google.com/file/d/1fN6y39lbuOknzOgMqT73TWPItBLfADJi/view?usp=drive\\\_link`
* 📁 Folder Google Drive (pre-interview test): `https://drive.google.com/drive/folders/1UZCzPdo2Ii1EGnwsxGlKQtvf7jHHFt\\\_J?usp=drive\\\_link`

## 🛠️ Tools

* **Engine:** Unity (2D/3D hybrid, sprite dengan sorting berbasis posisi Z untuk efek 2.5D)
* **Bahasa:** C#
* **Design Pattern:** Strategy, Singleton, dan pemisahan data via ScriptableObject
* **Version Control:** Git

## 📌 Catatan Pengembangan

Beberapa hal yang disadari sebagai area pengembangan lanjutan (transparan untuk keperluan review):

* Sistem dialog saat ini memiliki dua implementasi paralel (`DialogueManager` \& `SimpleDialogueManager`) yang idealnya bisa disatukan agar tidak duplikatif.
* `EnemyAI` di `BattleManager` masih berbasis probabilitas sederhana, belum mempertimbangkan kondisi HP pemain.
* Referensi antar-GameObject beberapa masih menggunakan `GameObject.Find` berbasis nama (misal `"Enemy"`, `"Arena\_Gate"`) pada cutscene duel — cukup rawan bila nama object di scene berubah, idealnya diganti dengan reference langsung atau tagging.

\---

Dikembangkan sebagai bagian dari Pre-Interview Test Game Programmer — Toge Productions.

