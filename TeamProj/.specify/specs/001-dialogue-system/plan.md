# 구현 계획서: 다이얼로그 시스템

**브랜치**: `001-dialogue-system` | **날짜**: 2025년 11월 11일 | **명세서**: [spec.md](./spec.md)
**입력**: `/specs/001-dialogue-system/spec.md` 로부터의 기능 명세서

## 요약

UI 기반의 다이얼로그(Dialogue) 시스템을 구현합니다. 이 시스템의 핵심은 `DialogueManager`라는 싱글톤(Singleton) C# 클래스이며, 대화 흐름, UI 표시, 게임 상태(Time.timeScale)를 관리합니다. 대화 내용은 외부 JSON 파일에서 로드하고, UI는 Unity의 UGUI를 사용하여 구축합니다. 승객(NPC)이 데스크에 도착하면 대화가 자동으로 시작됩니다.

## 기술 컨텍스트

**언어/버전**: C# (Unity 2021.3 LTS 이상)
**주요 의존성**: Unity Engine, Unity UI (UGUI)
**저장소**: `StreamingAssets` 폴더 내 JSON 파일
**테스팅**: Unity Test Framework
**타겟 플랫폼**: PC (프로젝트 컨텍스트에 따름)
**프로젝트 타입**: 3D 여권 심사 게임
**성능 목표**: 대화 중 게임 성능 저하 없음 (UI 렌더링 외 오버헤드 최소화)
**제약 조건**: 외부 라이브러리 사용을 최소화하고 Unity 내장 기능을 우선적으로 활용합니다.
**확장/범위**: 초기 구현은 단일 대화 흐름에 집중하며, 분기나 선택지는 포함하지 않습니다.

## 규칙(Constitution) 확인

*게이트: 0단계(Phase 0) 리서치 전에 통과해야 함. 1단계(Phase 1) 설계 후 재확인.*

*   [X] **C# 스크립트 사용**: 모든 새 코드는 C#으로 작성됩니다.
*   [X] **프리팹(Prefab) 기반 개발**: 다이얼로그 UI는 프리팹으로 제작될 것입니다.
*   [ ] **씬(Scene) 관리**: 이 기능은 특정 씬에 종속되지 않으나, 씬 관리 전략을 위반하지 않습니다.
*   [X] **에셋(Asset) 네이밍 컨벤션**: 모든 에셋은 `Type_Name_Variant` 규칙을 따를 것입니다.
*   [X] **Git을 사용한 버전 관리**: 모든 변경사항은 `001-dialogue-system` 브랜치에 커밋됩니다.
*   [X] **스크립트 파일 위치**: 모든 스크립트는 `Assets/02.Scripts/Proj/Dialogue` 폴더에 위치할 것입니다.

## 프로젝트 구조

### 문서 (이 기능)

```text
specs/001-dialogue-system/
├── plan.md              # 이 파일
├── research.md          # 0단계 결과물
├── data-model.md        # 1단계 결과물
├── quickstart.md        # 1단계 결과물
└── contracts/           # 1단계 결과물 (공개 API 정의)
```

### 소스 코드 (저장소 루트)

```text
Assets/
├── 02.Scripts/
│   └── Proj/
│       └── Dialogue/
│           ├── DialogueManager.cs
│           ├── DialogueUI.cs
│           ├── DialogueTrigger.cs
│           └── DialogueData.cs
├── 03.Prefab/
│   └── UI/
│       └── DialogueCanvas.prefab
└── 04.StreamingAssets/
    └── Dialogue/
        └── passenger_01.json
```

**구조 결정**: `Constitution`에 명시된 대로, 모든 스크립트는 `Assets/02.Scripts/Proj` 하위의 기능별 폴더(`Dialogue`)에 생성합니다. 대화 데이터는 `StreamingAssets`에 위치하여 빌드 후에도 쉽게 접근하고 수정할 수 있도록 합니다.

## 복잡도 추적

> **규칙(Constitution) 위반 사항이 있고, 그에 대한 정당화가 필요한 경우에만 작성**

| 위반 사항 | 필요한 이유 | 더 간단한 대안을 거부한 이유 |
|-----------|-------------|-----------------------------|
| N/A       | -           | -                           |