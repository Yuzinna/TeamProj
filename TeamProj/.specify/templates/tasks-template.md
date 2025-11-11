---

description: "Task list template for feature implementation"
---

# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-feature-name]/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: The examples below include test tasks. Tests are OPTIONAL - only include them if explicitly requested in the feature specification.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Unity Project**: All paths are relative to the `Assets/` folder (e.g., `02.Scripts/Proj`, `Prefabs/`).
- Paths shown below assume the standard Unity project structure defined in `plan.md`.

<!-- 
  ============================================================================
  IMPORTANT: The tasks below are SAMPLE TASKS for illustration purposes only.
  
  The /speckit.tasks command MUST replace these with actual tasks based on:
  - User stories from spec.md (with their priorities P1, P2, P3...)
  - Feature requirements from plan.md
  - Entities from data-model.md
  - Endpoints from contracts/
  
  Tasks MUST be organized by user story so each story can be:
  - Implemented independently
  - Tested independently
  - Delivered as an MVP increment
  
  DO NOT keep these sample tasks in the generated tasks.md file.
  ============================================================================
-->

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create folder structure inside `Assets/` per implementation plan
- [ ] T002 Initialize project settings (e.g., Graphics, Physics)
- [ ] T003 [P] Configure linting and formatting tools for C#

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

Examples of foundational tasks (adjust based on your project):

- [ ] T004 Setup base scene and scene management system
- [ ] T005 [P] Implement a generic state machine or event manager
- [ ] T006 [P] Setup input management system
- [ ] T007 Create base classes or interfaces (e.g., `IDamageable`)
- [ ] T008 Configure error handling and logging infrastructure
- [ ] T009 Setup environment configuration management (e.g., for different build targets)

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - [Title] (Priority: P1) 🎯 MVP

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 1 (OPTIONAL - only if tests requested) ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T010 [P] [US1] Create PlayMode test script in `Assets/02.Scripts/Editor/Tests/`
- [ ] T011 [P] [US1] Create EditMode test script in `Assets/02.Scripts/Editor/Tests/`

### Implementation for User Story 1

- [ ] T012 [P] [US1] Create C# script `[ScriptName].cs` in `Assets/02.Scripts/Proj/`
- [ ] T013 [P] [US1] Create Prefab `[PrefabName].prefab` in `Assets/Prefabs/`
- [ ] T014 [US1] Implement logic in `[ScriptName].cs` (depends on T012)
- [ ] T015 [US1] Attach script to Prefab and configure it
- [ ] T016 [US1] Add validation and error handling in the script
- [ ] T017 [US1] Add logging for user story 1 operations

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - [Title] (Priority: P2)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 2 (OPTIONAL - only if tests requested) ⚠️

- [ ] T018 [P] [US2] Create PlayMode test for US2 in `Assets/02.Scripts/Editor/Tests/`
- [ ] T019 [P] [US2] Create EditMode test for US2 in `Assets/02.Scripts/Editor/Tests/`

### Implementation for User Story 2

- [ ] T020 [P] [US2] Create C# script `[ScriptName2].cs` in `Assets/02.Scripts/Proj/`
- [ ] T021 [US2] Implement service or manager class in `Assets/02.Scripts/Proj/`
- [ ] T022 [US2] Implement feature logic
- [ ] T023 [US2] Integrate with User Story 1 components (if needed)

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - [Title] (Priority: P3)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 3 (OPTIONAL - only if tests requested) ⚠️

- [ ] T024 [P] [US3] Create PlayMode test for US3 in `Assets/02.Scripts/Editor/Tests/`
- [ ] T025 [P] [US3] Create EditMode test for US3 in `Assets/02.Scripts/Editor/Tests/`

### Implementation for User Story 3

- [ ] T026 [P] [US3] Create C# script `[ScriptName3].cs` in `Assets/02.Scripts/Proj/`
- [ ] T027 [US3] Implement service or manager class
- [ ] T028 [US3] Implement feature logic

**Checkpoint**: All user stories should now be independently functional

---

[Add more user story phases as needed, following the same pattern]

---

## Phase N: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] TXXX [P] Documentation updates (e.g., code comments, tooltips)
- [ ] TXXX Code cleanup and refactoring
- [ ] TXXX Performance optimization across all stories
- [ ] TXXX [P] Additional unit tests (if requested) in `Assets/02.Scripts/Editor/Tests/`
- [ ] TXXX Security hardening (if applicable)
- [ ] TXXX Run quickstart.md validation

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - May integrate with US1 but should be independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - May integrate with US1/US2 but should be independently testable

### Within Each User Story

- Tests (if included) MUST be written and FAIL before implementation
- Create scripts and prefabs before implementing logic
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, all user stories can start in parallel (if team capacity allows)
- All tests for a user story marked [P] can run in parallel
- Scripts/Prefabs within a story marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together (if tests requested):
Task: "Create PlayMode test script in Assets/02.Scripts/Editor/Tests/"
Task: "Create EditMode test script in Assets/02.Scripts/Editor/Tests/"

# Launch all asset creation for User Story 1 together:
Task: "Create C# script [ScriptName].cs in Assets/02.Scripts/Proj/"
Task: "Create Prefab [PrefabName].prefab in Assets/Prefabs/"
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
