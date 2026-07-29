# Ring Out!

A Unity arena-fighting game prototype featuring local matches, momentum-based combat, character-specific hype themes, ring-outs, and best-of-three match sets.

## Project Status

**Version:** v0.2.1  
**Status:** Active development / playable prototype  
**Engine:** Unity 2019.2.0f1

The current build supports complete matches, rematches,character-specific attacks and special attacks, character-specific music, menu navigation, victory sequences, and scene transitions.

## Current Features

- local multiplayer game modes
- Best-of-three match sets
- Ring-out and momentum-based victories
- Character-specific hype music
- Full-match victory themes
- Blocking, dashing, stamina, and attack systems
- Pause and match-over menus
- Keyboard and controller navigation
- Playable fighters including Marie and Dukez

## Recent Changes

### v0.2.1

- Restored character hype music
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
| Player controls | See the in-game controls or project Input Manager |

## Running the Project

1. Clone the repository.
2. Open the project using Unity `2019.2.0f1`.
3. Allow Unity to import the project assets.
4. Open the Main Menu scene.
5. Enter Play Mode.

```bash
git clone <repository-url>