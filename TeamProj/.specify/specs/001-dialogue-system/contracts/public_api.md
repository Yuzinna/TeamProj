# 공개 API 계약: 다이얼로그 시스템

이 문서는 다이얼로그 시스템의 핵심 클래스에 대한 공개 메소드(public methods)와 속성(properties)을 설명합니다. 다른 시스템은 여기에 정의된 API를 통해서만 이 컴포넌트들과 상호작용해야 합니다.

## 1. `DialogueManager.cs`

**역할**: 전체 다이얼로그 생명주기를 제어하는 싱글톤(Singleton).
**위치**: `Assets/02.Scripts/Proj/Dialogue/DialogueManager.cs`

```csharp
public class DialogueManager : MonoBehaviour
{
    // 전역 접근을 위한 싱글톤 인스턴스
    public static DialogueManager Instance { get; private set; }

    // 다이얼로그 시작 또는 종료 시 알림을 위한 이벤트
    public event System.Action<bool> OnDialogueStateChanged;

    /// <summary>
    /// 새로운 다이얼로그 시퀀스를 시작합니다.
    /// </summary>
    /// <param name="sequence">표시할 다이얼로그 시퀀스입니다.</param>
    public void StartDialogue(DialogueSequence sequence);

    /// <summary>
    /// UI 또는 플레이어 입력에 의해 다음 대사로 넘어갈 때 호출됩니다.
    /// </summary>
    public void AdvanceDialogue();
}
```

### 공개 멤버 (Public Members)

- **`Instance` (static property)**: `DialogueManager`에 대한 전역 접근을 제공합니다.
- **`OnDialogueStateChanged` (event)**: 다이얼로그가 시작(`true`)되거나 끝날(`false`) 때 발생하는 이벤트입니다. 다른 시스템(예: 플레이어 움직임, NPC AI)이 이 이벤트를 구독하여 자체 로직을 일시정지/재개할 수 있습니다.
- **`StartDialogue(DialogueSequence sequence)` (method)**: 다이얼로그를 시작합니다. JSON에서 로드된 `DialogueSequence` 객체를 매개변수로 받습니다. 이 메소드는 게임을 일시정지(`Time.timeScale = 0`)하고 첫 번째 대사를 표시합니다.
- **`AdvanceDialogue()` (method)**: 대화를 진행시킵니다. 타자기 효과가 실행 중이면 즉시 완료합니다. 대사가 완료된 상태라면 다음 대사를 보여줍니다. 시퀀스의 끝이라면 다이얼로그를 종료하고, 게임을 재개하며, UI를 숨깁니다.

## 2. `DialogueUI.cs`

**역할**: 다이얼로그 박스의 모든 UGUI 컴포넌트를 관리합니다.
**위치**: `Assets/02.Scripts/Proj/Dialogue/DialogueUI.cs`

```csharp
public class DialogueUI : MonoBehaviour
{
    /// <summary>
    /// UI에 한 줄의 대화를 표시합니다.
    /// </summary>
    /// <param name="speakerName">화자의 이름입니다.</param>
    /// <param name="dialogueText">표시할 텍스트입니다 (타자기 효과를 위해 부분적일 수 있음).</param>
    public void UpdateDialogue(string speakerName, string dialogueText);

    /// <summary>
    /// 전체 다이얼로그 UI 캔버스를 보여주거나 숨깁니다.
    /// </summary>
    /// <param name="show">true이면 표시, false이면 숨김.</param>
    public void SetVisible(bool show);
}
```

### 공개 멤버 (Public Members)

- **`UpdateDialogue(string speakerName, string dialogueText)` (method)**: 화면의 화자 이름과 다이얼로그 텍스트 요소를 업데이트합니다. `DialogueManager`가 타자기 효과를 위해 이 메소드를 반복적으로 호출합니다.
- **`SetVisible(bool show)` (method)**: 메인 다이얼로그 캔버스의 가시성을 제어합니다. 대화 시작과 끝에 `DialogueManager`에 의해 호출됩니다.

## 3. `DialogueTrigger.cs`

**역할**: 게임 이벤트에 기반하여 다이얼로그 시퀀스를 시작합니다.
**위치**: `Assets/02.Scripts/Proj/Dialogue/DialogueTrigger.cs`

```csharp
public class DialogueTrigger : MonoBehaviour
{
    // 트리거될 다이얼로그 시퀀스.
    public TextAsset dialogueJson;

    /// <summary>
    /// 다이얼로그를 시작하는 공개 메소드입니다.
    /// 다른 스크립트나 애니메이션 이벤트에서 호출될 수 있습니다.
    /// </summary>
    public void TriggerDialogue();
}
```

### 공개 멤버 (Public Members)
- **`dialogueJson` (public field)**: 재생할 다이얼로그가 포함된 JSON 파일(`TextAsset` 형식)에 대한 참조입니다. Unity 인스펙터에서 할당됩니다.
- **`TriggerDialogue()` (method)**: 호출되면, 이 메소드는 `dialogueJson` 파일을 파싱하고 결과 `DialogueSequence` 객체를 `DialogueManager.Instance.StartDialogue()`에 전달합니다. 명세서에 따라, 이 메소드는 승객 NPC가 데스크에 도착했을 때 자동으로 호출됩니다.