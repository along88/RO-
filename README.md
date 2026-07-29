# Ring Out!

A Unity arena-fighting game prototype featuring local multiplayer matches, momentum-based combat, character-specific attacks, hype themes, ring-outs, and best-of-three match sets.

## Project Status

**Version:** v0.2.1  
**Status:** Active development / playable prototype  
**Engine:** Unity 2019.2.0f1

The current build supports complete matches, rematches, character-specific attacks and special attacks, character-specific music, menu navigation, victory sequences, and scene transitions.

## Current Features

- Local multiplayer game mode
- Best-of-three match sets
- Ring-out and momentum-based victories
- Character-specific attacks and special attacks
- Character-specific hype music
- Full-match victory themes
- Blocking, dashing, stamina, and attack systems
- Pause and match-over menus
- Keyboard and controller navigation
- Playable fighters including Marie and Dukez

## Recent Changes

### v0.2.1

- Restored character-specific hype music
- Added winner theme playback after a full match-set victory
- Separated menu sound effects from background music
- Added a dedicated UI audio manager
- Separated menu navigation logic from match logic
- Consolidated match state through `MainGameManager`
- Cleaned and consolidated the repository branch structure

## Known Issues

- The victory theme may begin after a short delay.
- Some older systems are still being refactored.
- Additional testing is needed across all fighter combinations and scene transitions.

## Controls

| Action | Input |
|---|---|
| Menu navigation | Configured `NavV2` axis |
| Confirm | Return / configured `Submit` input |
| Pause | Configured pause input |
| Player controls | See the in-game controls or Unity Input Manager |

## Downloading the Game

Playable builds are available from the repository's **Releases** page.

For Windows, download the installer or portable build associated with the latest release.

### Windows Installer

1. Download the latest `RingOut-Setup.exe` release asset.
2. Run the installer.
3. Follow the installation prompts.
4. Launch Ring Out! from the installed shortcut.

Because the project is independently distributed and may not yet be digitally signed, Windows may display an unknown-publisher warning.

### Portable Build

1. Download the Windows portable ZIP.
2. Extract the complete ZIP.
3. Keep the executable, DLL files, and `_Data` folder together.
4. Run `RingOutProject.exe`.

Do not remove the executable from its accompanying files or run it directly from inside the ZIP.

## Running the Unity Project

1. Clone the repository.
2. Open the project using Unity `2019.2.0f1`.
3. Allow Unity to import the project assets.
4. Open the Main Menu scene.
5. Enter Play Mode.

```bash
git clone https://github.com/along88/RO-.git```
Using a significantly different Unity version may trigger package, scene, prefab, or asset upgrades.

## Repository Branches

- `master` contains tested milestone versions.
- `develop` contains the current integrated development version.
- New work should be developed in temporary `feature/*` branches created from `develop`.

## Releases

Versioned milestones are available from the repository's **Releases** page.

Release packages may include:

- Windows installer
- Portable Windows build
- Game manual
- Release notes
- Automatically generated source-code archives

GitHub's automatically generated source-code ZIP and TAR files contain the Unity project source. They are not standalone playable builds.

## Credits

| Role | Contributor |
|---|---|
| Executive Producer | Miguel Bugarin |
| Lead Game Designer | Alfred Long |
| Lead Programmer | Alfred Long |
| Programmer | Josh Mond |
| Lead 3D Artist | Shawn Latini |
| Lead VFX Artist | Alfred Long |
| Lead Audio Designer | Alfred Long |
| Lead QA | Alfred Long |
| QA | Miguel Bugarin |

### Original Music

Featuring original music by:

- **Alberto “Birdie” Velazquez**
- **Hutch The Rad**

Ring Out! was created through the combined production, design, programming, art, visual effects, audio, and quality-assurance contributions of the people listed above.

## Development Notes

Ring Out! is an older Unity project currently undergoing restoration, debugging, and modernization.

Some code and assets retain their original project structure while major systems are gradually being separated into clearer responsibilities.

Current refactoring priorities include:

- Audio-system separation
- Menu and navigation architecture
- Match-state management
- Scene lifecycle reliability
- Fighter and animation cleanup
- General code maintainability

## License

No license has currently been specified.

Unless a license is added, the source code, music, artwork, game assets, and other project materials should not be assumed to be available for reuse, modification, or redistribution.
