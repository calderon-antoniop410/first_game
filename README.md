# First Game

A 2D game where the player fights slimes, earns kills, and tries to beat the saved high score.

## Scripts

- `CameraFollow.cs` - Smoothly follows the player.
- `HighScoreDisplay.cs` - Shows the saved high score in the main menu.
- `MainMenu.cs` - Starts the game, quits, and displays the controls page.
- `PauseMenu.cs` - Pauses, resumes, returns to the main menu, or quits.
- `PlayerAudio.cs` - Plays the player's sword sound.
- `PlayerHealth.cs` - Tracks player health and returns to the main menu after death.
- `PlayerMovement.cs` - Reads player movement and attack input, moves the player, and activates the sword hitbox.
- `ScoreManager.cs` - Tracks kills and saves the best score with PlayerPrefs.
- `SlimeEnemy.cs` - Moves slimes toward the player, attacks, takes damage, and dies.
- `SlimeSpawner.cs` - Spawns slimes just outside the camera view and limits how many are alive.
- `SwordHitbox.cs` - Damages each slime once per sword swing using a trigger.

## Scenes and key GameObjects

- **MainMenu** - The Canvas contains Play, Settings, Quit, and Highscore UI. `MenuManager` (`MainMenu.cs`) starts `Gameplay_1`, opens the controls page, and quits the game. `HighScoreDisplay` reads the saved best score.
- **Gameplay_1** - `Player` uses a `PlayerInput`, `PlayerMovement`, `PlayerHealth`, and `PlayerAudio`. The `Main Camera` follows the player. `Ground` and `Top` Tilemaps form the level and use Tilemap Collider 2D components. `EnemySlimeSpawner` creates enemies, which chase and attack the tagged Player. The sword hitbox damages slimes through a trigger. `ScoreManager` updates kills and saves the best score. `PauseManager` controls the `PausePanel`; its buttons resume, return to the menu, or quit. A Global Light 2D lights the scene.

## Assignment requirements

1. **Scenes and scene management** - MainMenu's Play button loads `Gameplay_1`; the pause menu and player-death flow can return to MainMenu.
2. **Input System player control** - The Player has a `PlayerInput` using an Input Actions asset with Move and Attack actions; `PlayerMovement` receives movement input and handles attacks.
3. **Tilemap level layout** - The Gameplay scene has Ground and Top Tilemaps with Tilemap Collider 2D components; Top has a non-trigger collider for collision.
4. **Prefab pooling** - **Not currently satisfied:** `SlimeSpawner` uses `Instantiate`, and `SlimeEnemy` uses `Destroy`; enemies are not pooled or reused.
5. **Tags, Layers, and triggers** - The Player is tagged `Player`; the sword hitbox uses `OnTriggerEnter2D` to damage slimes.
6. **Pause menu** - The Gameplay Canvas pause menu sets `Time.timeScale` to pause and resume, with buttons to return to MainMenu or quit.
7. **PlayerPrefs persistence** - `ScoreManager` saves the best kill count to PlayerPrefs, and the main menu displays it after relaunch.
8. **Lighting or material/shader touch** - Gameplay_1 includes an active Global Light 2D.

## Third-party assets, packages, and tutorials

- The project uses Unity packages including Input System, Universal Render Pipeline (2D lighting), Tilemap, and TextMesh Pro. It also includes NavMeshPlus from `https://github.com/h8man/NavMeshPlus.git` for 2D slime navigation.
- Imported sprite, tile, and audio files are in `Assets/Sprites`, `Assets/Tilemaps`, and `Assets/Audio`. Free assets obtained online.
- Tutorials on how navmesh works and used AI for documentation and in creating the settings / controls button.
