# Quantum Platformer Demo

A 3D multiplayer platformer game built with Unity and Photon Quantum 3, demonstrating deterministic physics-based gameplay for up to 2 players.

## Original Task Requirements

### Main Requirements
- Using Quantum 3
- Create a 3D mini-game for 2 players
- 2 cubes to go around, 1 platform to jump on, finish at the end
- Create a character with simple animation (Mixamo can be used)
- Movement (preferably KCC or Physics), jump and camera following the character (optional, can be top-down side view or whatever is convenient)
- Transition to level 2, which is a different scene with 2 boxes and 2 platforms for variety
- Make at the end of level 2 on the 2nd platform a Cube (1-2) that can be pushed
- Execute in Unity 6.2+
- Upload results to git

### Additional
- Should also be able to push other characters similar to cubes (each other)

## Technical Stack

- **Unity**: 6.2.10f1 (URP)
- **Photon Quantum**: 3.0 (Deterministic Simulation)
- **Photon Realtime**: Networking
- **C#**: Primary programming language
- **Quantum DSL**: .qtn files for component definitions

## Features Implemented

### Core Gameplay
- 3D Platformer Movement: Physics-based character controller with jumping
- Multiplayer Support: Deterministic 2-player gameplay
- Level Progression: Two distinct levels with increasing difficulty
- Interactive Objects: Pushable cubes and characters
- Finish Zones: Level completion triggers

### Character Systems
- Animation Integration: Movement speed and grounded state animations
- Camera Following: Third-person camera that follows the local player
- Input Handling: WASD/Arrow key movement with spacebar jump
- Respawn System: Automatic respawn when falling out of bounds

### Technical Features
- Quantum Simulation: Deterministic physics simulation
- Network Synchronization: Proper multiplayer state synchronization
- Component Architecture: Clean separation of simulation and view logic
- Code Documentation: Comprehensive XML documentation

## Project Architecture

### Quantum Simulation Layer (Assets/Scripts/Simulation/)
Handles all game logic that needs to be deterministic and synchronized across network.

- PlatformerSystem.cs: Main movement and physics system
- LevelFinishSystem.cs: Level transition logic
- PlayerSpawnSystem.cs: Player entity spawning
- Input.qtn: Input button definitions
- Components/: Quantum component definitions
  - PlayerCharacter.qtn: Character state tracking
  - FinishZone.qtn: Level completion markers
  - PlayerLink.qtn: Player-entity associations

### Unity View Layer (Assets/Scripts/View/)
Handles visual representation and Unity-specific features.

- CharacterAnimator.cs: Animation synchronization
- CharacterAnchor.cs: Rotation locking
- PlatformerInput.cs: Unity input polling
- PlayerCameraController.cs: Multiplayer camera management

## Controls

| Action | Keyboard |
|--------|----------|
| Move Left | A / ← |
| Move Right | D / → |
| Move Forward | W / ↑ |
| Move Backward | S / ↓ |
| Jump | Space |

## Project Structure
```
Quantum-Platformer-Demo/
├── Assets/
│   ├── Photon/              # Photon Quantum SDK
│   ├── QuantumUser/         # Generated Quantum files
│   │   ├── Simulation/      # Deterministic game logic
│   │   ├── View/           # Unity view components
│   │   └── Resources/      # Quantum configurations
│   ├── Scripts/            # Custom game scripts
│   │   ├── Simulation/     # Quantum systems
│   │   └── View/          # Unity view scripts
│   └── Scenes/            # Unity scenes (Level1, Level2)
├── Packages/               # Unity package dependencies
└── ProjectSettings/       # Unity project settings
```

## Development Notes

### Quantum Architecture
This project follows Quantum's recommended architecture:
- Simulation Layer: All game logic runs in deterministic Quantum simulation
- View Layer: Unity handles visual representation and interpolation
- Input Layer: Unity polls input and feeds it to Quantum

### Key Technical Decisions

1. Physics-Based Movement: Uses Quantum's 3D physics for realistic character movement and interactions
2. Deterministic Simulation: All gameplay logic is deterministic for fair multiplayer experience
3. Component-Based Design: Clean separation using Quantum components and Unity view components
4. Push Mechanics: Handled by Quantum physics engine - no custom implementation needed

### Performance Considerations
- Quantum systems use efficient filtering to process only relevant entities
- View components minimize Unity physics queries
- Animation parameters cached for performance

### Multiplayer Implementation
- Each player controls one character entity
- Camera only active for local player
- Input synchronized through Quantum's deterministic input system
- Physics interactions work across network automatically

## Code Quality

- Documentation: Comprehensive XML documentation on all public methods
- Code Style: Consistent C# naming conventions
- Architecture: Clean separation of concerns
- Comments: Inline comments explaining complex logic and magic numbers

## Acknowledgments

- Built with Photon Quantum - Deterministic multiplayer framework
- Unity game engine
- Mixamo animations (if used)
- Original task specification provided for development assessment
