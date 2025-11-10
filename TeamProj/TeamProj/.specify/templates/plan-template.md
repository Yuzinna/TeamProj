# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

**Language/Version**: C# (Unity)
**Primary Dependencies**: Unity Engine
**Storage**: N/A
**Testing**: Unity Test Framework
**Target Platform**: PC, Mobile
**Project Type**: Unity Game
**Performance Goals**: 60 FPS
**Constraints**: TBD
**Scale/Scope**: TBD

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

*   [ ] **C# Script Usage**: All new code is written in C#.
*   [ ] **Prefab-based Development**: Game objects are created as prefabs.
*   [ ] **Scene Management**: Scene structure is logical and follows the loading strategy.
*   [ ] **Asset Naming Convention**: Asset names follow the convention.
*   [ ] **Version Control with Git**: Changes are committed to a separate branch.
*   [ ] **Script File Location**: New scripts are placed in the correct subfolder within `Assets/02.Scripts/Proj`.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
Assets/
├── Scenes/
├── 02.Scripts/
│   ├── Editor/
│   └── Proj/
├── Prefabs/
├── Materials/
├── Textures/
├── Models/
└── Audio/
```

**Structure Decision**: The project will follow the standard Unity project structure within the `Assets` folder, with scripts located under `Assets/02.Scripts/Proj`.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
