# 빠른 시작 가이드: 다이얼로그 시스템

이 가이드는 Unity 에디터에서 다이얼로그 시스템을 설정하고 사용하는 방법을 설명합니다.

## 설정 단계

1.  **다이얼로그 UI 프리팹(Prefab) 생성**:
    - 씬에 새로운 `Canvas`를 생성합니다 (`GameObject -> UI -> Canvas`). 이름을 `DialogueCanvas`로 지정합니다.
    - `Canvas Scaler`를 `Scale With Screen Size`로 설정하여 다른 해상도에서도 일관되게 보이도록 합니다.
    - 화면 하단 중앙에 다이얼로그 박스 배경을 위한 `Panel`을 추가합니다.
    - 패널에 두 개의 `TextMeshPro - Text` 오브젝트를 추가합니다: 하나는 화자 이름을 위한 것(`NameText`), 다른 하나는 대화 내용을 위한 것(`DialogueText`).
    - `DialogueUI.cs` 스크립트를 루트 `DialogueCanvas` 오브젝트에 첨부합니다.
    - 인스펙터(Inspector)에서 `DialogueUI` 스크립트의 해당 공개 필드에 `NameText`와 `DialogueText` 오브젝트를 드래그하여 연결합니다.
    - 이 `DialogueCanvas`로 프리팹을 만들어 `Assets/03.Prefab/UI/`에 저장합니다. 매니저가 인스턴스화할 것이므로 씬에서는 삭제해도 됩니다.

2.  **다이얼로그 매니저 생성**:
    - 메인 씬에 새로운 빈 `GameObject`를 생성하고 이름을 `DialogueManager`로 지정합니다.
    - `DialogueManager.cs` 스크립트를 여기에 첨부합니다.
    - `DialogueManager` 스크립트의 인스펙터에서, 이전 단계에서 생성한 `DialogueCanvas` 프리팹을 할당합니다. 매니저가 UI를 생성하고 파괴하는 역할을 담당합니다.

3.  **대화 데이터 준비**:
    - `Assets/04.StreamingAssets/Dialogue/`에 새로운 JSON 파일을 생성합니다 (예: `passenger_arrival.json`).
    - `data-model.md`에 정의된 구조에 따라 파일을 작성합니다.
    
    ```json
    {
      "lines": [
        { "speaker": "승객", "text": "제 서류입니다." },
        { "speaker": "심사관", "text": "모든 것이 순서대로 잘 정리되어 있군요." }
      ]
    }
    ```

4.  **트리거(Trigger) 설정**:
    - 다이얼로그는 승객이 도착하면 자동으로 트리거됩니다. 승객의 도착을 제어하는 스크립트는 `DialogueTrigger` 컴포넌트에 대한 참조를 가져야 합니다.
    - `DialogueTrigger.cs` 스크립트를 승객 `GameObject`(또는 전용 트리거 오브젝트)에 첨부합니다.
    - `DialogueTrigger`의 인스펙터에서 `Dialogue Json` 필드에 `passenger_arrival.json` 파일을 드래그합니다.
    - 승객의 도착 로직이 완료되면, 해당 `DialogueTrigger` 컴포넌트의 `TriggerDialogue()` 메소드를 호출해야 합니다.

## 작동 방식

1.  승객의 도착 스크립트가 `TriggerDialogue()`를 호출합니다.
2.  `DialogueTrigger`는 JSON 데이터를 읽고, `DialogueSequence` 객체로 역직렬화(deserializes)합니다.
3.  그런 다음 `DialogueManager.Instance.StartDialogue(sequence)`를 호출합니다.
4.  `DialogueManager`는 게임을 일시정지(`Time.timeScale = 0`)하고, 다이얼로그 UI 프리팹을 인스턴스화하며, `DialogueUI`에 첫 번째 대사를 표시하라고 지시합니다.
5.  `DialogueManager`는 타자기 효과를 위한 코루틴을 실행하여 `DialogueUI.UpdateDialogue()`를 반복적으로 호출합니다.
6.  플레이어가 `Space` 키를 누르면 `DialogueManager.AdvanceDialogue()`가 호출됩니다.
7.  다이얼로그가 끝나면, `DialogueManager`는 게임을 재개하고 UI를 파괴하며, 게임이 계속됩니다.