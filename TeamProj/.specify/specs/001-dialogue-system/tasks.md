# 작업 목록: 다이얼로그 시스템

**입력**: `/specs/001-dialogue-system/`의 설계 문서
**전제 조건**: plan.md (필수), spec.md (사용자 스토리 필수), research.md, data-model.md, contracts/

**구성**: 작업들은 각 사용자 스토리의 독립적인 구현과 테스트를 가능하게 하기 위해 사용자 스토리별로 그룹화됩니다.

## 형식: `[ID] [P?] [스토리] 설명`

- **[P]**: 병렬 실행 가능 (서로 다른 파일, 미완료 작업에 대한 의존성 없음)
- **[스토리]**: 이 작업이 속한 사용자 스토리 (예: US1, US2, US3)
- 설명에 정확한 파일 경로 포함

## 경로 규칙

- **Unity 프로젝트**: 모든 경로는 `Assets/` 폴더 기준입니다 (예: `02.Scripts/Proj`, `03.Prefab/`).
- 아래 경로들은 `plan.md`에 정의된 표준 Unity 프로젝트 구조를 따릅니다.

---

## 1단계: 설정 (공유 인프라)

**목표**: 다이얼로그 시스템을 위한 프로젝트 초기화 및 기본 구조 설정.

- [ ] T001 다이얼로그 관련 모든 스크립트를 위한 `Assets/02.Scripts/Proj/Dialogue` 폴더 생성
- [ ] T002 [P] UI 관련 프리팹을 위한 `Assets/03.Prefab/UI` 폴더 생성
- [ ] T003 [P] JSON 데이터 파일을 위한 `Assets/04.StreamingAssets/Dialogue` 폴더 생성

---

## 2단계: 기반 작업 (선행 조건)

**목표**: 사용자 스토리 구현 전에 반드시 완료되어야 하는 핵심 인프라 구축.

- [ ] T004 `Assets/02.Scripts/Proj/Dialogue/DialogueData.cs` C# 스크립트 생성 및 `DialogueLine`, `DialogueSequence` 클래스 정의
- [ ] T005 [P] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs` C# 스크립트를 기본 싱글톤 구조로 생성
- [ ] T006 [P] `Assets/02.Scripts/Proj/Dialogue/DialogueUI.cs` C# 스크립트를 빈 메소드 스텁으로 생성
- [ ] T007 [P] `Assets/02.Scripts/Proj/Dialogue/DialogueTrigger.cs` C# 스크립트를 빈 메소드 스텁으로 생성
- [ ] T008 캔버스(Canvas), 패널(Panel), 화자 및 텍스트를 위한 TextMeshPro 오브젝트를 포함하는 기본 UI 프리팹 `Assets/03.Prefab/UI/DialogueCanvas.prefab` 생성
- [ ] T009 `DialogueUI.cs`를 프리팹에 연결하고 인스펙터(Inspector)에서 TextMeshPro 오브젝트들 연결

**체크포인트**: 기반 작업 준비 완료 - 이제 사용자 스토리 구현을 시작할 수 있습니다.

---

## 3단계: 사용자 스토리 1 - 기본 대화 출력 (우선순위: P1) 🎯 MVP

**목표**: 승객(NPC)이 데스크에 도착하면 대화창이 자동으로 나타나고 텍스트가 타자기 효과로 출력됩니다. 게임 시간은 이 동안 멈춥니다.

**독립 테스트**: 승객이 데스크에 도착하는 이벤트를 시뮬레이션했을 때, 대화창이 나타나고 첫 번째 대사가 타자기 효과로 출력되는지 확인합니다. `Time.timeScale`이 0으로 설정되었는지 확인합니다.

### 사용자 스토리 1 구현

- [ ] T010 [US1] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`에 게임 일시 정지(`Time.timeScale = 0`) 및 UI 표시를 처리하는 `StartDialogue` 메소드 구현
- [ ] T011 [US1] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`에 타자기 효과 코루틴(Coroutine) 구현
- [ ] T012 [US1] `Assets/02.Scripts/Proj/Dialogue/DialogueUI.cs`에 UI 요소를 제어하는 `UpdateDialogue` 및 `SetVisible` 메소드 구현
- [ ] T013 [US1] `Assets/02.Scripts/Proj/Dialogue/DialogueTrigger.cs`에 `DialogueManager`를 호출하는 `TriggerDialogue` 메소드 구현 (JSON 파싱은 일단 모의(mock) 처리하거나 비워둠)
- [ ] T014 [US1] 테스트 씬을 생성하고 승객 도착 이벤트를 구성하여 `TriggerDialogue`를 호출하고 흐름을 검증

**체크포인트**: 이 시점에서 사용자 스토리 1은 완전히 작동하고 독립적으로 테스트 가능해야 합니다.

---

## 4단계: 사용자 스토리 2 - 대화 넘기기 및 스킵 (우선순위: P2)

**목표**: 플레이어가 'Space' 키를 사용하여 타자기 효과를 스킵하거나 다음 대화로 넘어갈 수 있습니다.

**독립 테스트**: 대화가 출력되는 동안 'Space' 키를 누르면 텍스트 전체가 즉시 표시되는지 확인합니다. 텍스트가 모두 표시된 후 'Space' 키를 누르면 다음 대사로 넘어가거나 대화가 종료되는지 확인합니다.

### 사용자 스토리 2 구현

- [ ] T015 [US2] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`의 `Update` 루프에 'Space' 키 입력을 감지하는 로직 추가
- [ ] T016 [US2] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`에 `AdvanceDialogue` 메소드 구현
- [ ] T017 [US2] `DialogueManager.cs`의 타자기 코루틴을 스킵 가능하도록 수정
- [ ] T018 [US2] `AdvanceDialogue`에 다음 대사로 넘어가거나 대화를 종료하고 `Time.timeScale`을 복원하는 로직 추가

**체크포인트**: 이 시점에서 사용자 스토리 1과 2 모두 독립적으로 작동해야 합니다.

---

## 5단계: 사용자 스토리 3 - 외부 파일에서 대화 데이터 로드 (우선순위: P3)

**목표**: 대화 내용이 외부 JSON 파일에서 로드되어 게임에 표시됩니다.

**독립 테스트**: `Assets/04.StreamingAssets/Dialogue/` 폴더에 테스트용 JSON 파일을 생성하고, `DialogueTrigger`가 이 파일을 올바르게 로드하여 대화를 시작하는지 확인합니다.

### 사용자 스토리 3 구현

- [ ] T019 [US3] `Assets/02.Scripts/Proj/Dialogue/DialogueTrigger.cs`에 `04.StreamingAssets`에서 `dialogueJson` 파일을 읽는 로직 구현
- [ ] T020 [US3] `DialogueTrigger.cs`에서 `JsonUtility.FromJson`을 사용하여 JSON 데이터를 `DialogueSequence` 및 `DialogueLine` 객체로 파싱
- [ ] T021 [US3] 명세서에 따라 JSON 파일이 없거나 형식이 잘못된 경우에 대한 오류 처리를 `DialogueTrigger.cs`에 추가
- [ ] T022 [US3] `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`의 `StartDialogue` 메소드가 `DialogueSequence` 객체를 올바르게 받도록 확인

**체크포인트**: 모든 사용자 스토리가 이제 독립적으로 작동해야 합니다.

---

## 6단계: 폴리싱 및 공통 관심사

**목표**: 여러 사용자 스토리에 영향을 미치는 개선 작업.

- [ ] T023 생성된 스크립트의 모든 공개 필드 및 메소드에 C# 헤더 주석과 툴팁(Tooltip) 추가
- [ ] T024 [P] 모든 코드의 명확성, 성능, 규칙 준수 여부 검토
- [ ] T025 [P] `quickstart.md` 가이드를 실행하여 전체 흐름 검증
- [ ] T026 개발에 사용된 임시 테스트 씬이나 오브젝트 삭제
- [ ] T027 `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`에 `OnDisable()` 또는 `OnDestroy()` 메소드를 구현하여 `Time.timeScale`을 정상 값으로 복원

---

## 의존성 및 실행 순서

### 단계별 의존성

- **설정 (1단계)**: 의존성 없음.
- **기반 작업 (2단계)**: 설정(1단계) 완료에 의존. 모든 사용자 스토리를 블로킹함.
- **사용자 스토리 (3-5단계)**: 기반 작업(2단계) 완료에 의존.
- **폴리싱 (6단계)**: 모든 원하는 사용자 스토리 완료에 의존.

### 사용자 스토리 의존성

- **사용자 스토리 1 (P1)**: 기반 작업(2단계) 후 시작 가능.
- **사용자 스토리 2 (P2)**: 사용자 스토리 1에 의존.
- **사용자 스토리 3 (P3)**: 사용자 스토리 1에 의존.

### 병렬 실행 기회

- 기반 작업(2단계)이 완료되면, 사용자 스토리 2와 3 작업은 기술적으로 병렬 시작이 가능합니다 (각각 입력 처리와 데이터 로딩이라는 다른 부분을 수정하므로). 그러나 단일 개발자의 경우 순차적(US1 -> US2 -> US3)으로 완료하는 것을 권장합니다.
- 각 단계 내에서 [P]로 표시된 작업은 병렬 실행이 가능합니다.

## 구현 전략

### MVP 우선 (사용자 스토리 1만)

1.  1단계: 설정 완료
2.  2단계: 기반 작업 완료 (중요 - 모든 스토리를 블로킹함)
3.  3단계: 사용자 스토리 1 완료
4.  **중지 및 검증**: 사용자 스토리 1을 독립적으로 테스트합니다. 이는 핵심 대화 표시 기능을 제공합니다.

### 점진적 배포

1.  MVP (US1) 완료.
2.  사용자 스토리 2 (입력 처리) 추가. 조합 테스트.
3.  사용자 스토리 3 (데이터 로딩) 추가. 전체 시스템 테스트.
4.  폴리싱 단계 완료.
