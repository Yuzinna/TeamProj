<!--
Sync Impact Report:
- Version change: 1.0.0 -> 1.1.0
- Modified Principles:
  - Added: VI. Script File Location
- Templates requiring updates:
  - .specify/templates/plan-template.md (pending)
  - .specify/templates/tasks-template.md (pending)
-->
# TeamProj Constitution

## Core Principles

### I. C# Script Usage
All game logic and behaviors must be implemented using C# scripts. This ensures consistency and leverages the full power of the Unity engine.

### II. Prefab-based Development
All game objects should be created as prefabs. This allows for reusability, easier scene management, and streamlined updates.

### III. Scene Management
Each scene should be self-contained and represent a specific level or menu. A clear scene loading/unloading strategy must be in place.

### IV. Asset Naming Convention
All assets (scripts, materials, textures, models, etc.) must follow a consistent naming convention. (e.g., `Type_Name_Variant`, like `Script_Player_Movement`).

### V. Version Control with Git
The project must be managed using Git. All changes should be committed with clear and descriptive messages.

### VI. Script File Location
New scripts must always be placed in an appropriate subfolder within the `Assets/02.Scripts/Proj` folder. This maintains a clean and organized project structure.

## Development Workflow

All new features or bug fixes should be developed in a separate branch and merged into the main branch after review and approval.

## Quality Gates

Before merging, all code must be reviewed by at least one other team member. The feature must be testable and not break existing functionality.

## Governance

This constitution is the single source of truth for development practices. Any amendments require team discussion and approval.

**Version**: 1.1.0 | **Ratified**: 2025-11-10 | **Last Amended**: 2025-11-10
