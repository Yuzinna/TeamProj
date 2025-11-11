# 데이터 모델: 다이얼로그 시스템

이 문서는 다이얼로그 시스템의 데이터 구조를 C#과 해당 JSON 형식 모두에 대해 정의합니다.

## 1. C# 데이터 구조

다음 C# 클래스들은 `Assets/02.Scripts/Proj/Dialogue/DialogueData.cs` 파일에 생성됩니다. 이 클래스들은 Unity의 `JsonUtility`에 의해 쉽게 직렬화/역직렬화(serialized/deserialized)되도록 설계되었습니다.

### DialogueLine (대화 한 줄)

대화의 한 줄을 나타냅니다.

```csharp
[System.Serializable]
public class DialogueLine
{
    public string speaker; // 화자
    public string text;    // 대사
}
```

- **`speaker` (string)**: 말하는 캐릭터의 이름입니다. UI에 표시됩니다.
- **`text` (string)**: 표시될 대화 내용입니다.

### DialogueSequence (대화 시퀀스)

특정 이벤트나 NPC를 위한 완전한 대화 시퀀스를 나타냅니다.

```csharp
[System.Serializable]
public class DialogueSequence
{
    public DialogueLine[] lines;
}
```

- **`lines` (`DialogueLine` 배열)**: 대화를 구성하는, 순서가 지정된 대화 줄의 배열입니다.

## 2. JSON 파일 구조

대화 데이터는 `Assets/StreamingAssets/Dialogue/` 디렉토리 내의 JSON 파일에 저장됩니다. 각 파일은 단일 `DialogueSequence`를 나타냅니다.

### 예시: `passenger_01.json`

승객의 대화 파일이 어떻게 보일지에 대한 예시입니다.

```json
{
  "lines": [
    {
      "speaker": "승객",
      "text": "안녕하세요, 여기 제 여권입니다."
    },
    {
      "speaker": "심사관",
      "text": "감사합니다. 스캐너 위에 올려주세요."
    },
    {
      "speaker": "승객",
      "text": "네. 문제없나요?"
    },
    {
      "speaker": "심사관",
      "text": "잠시만요... 네, 모두 정확한 것 같군요. 통과하셔도 좋습니다."
    }
  ]
}
```

### JSON 스키마(Schema)

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Dialogue Sequence",
  "description": "NPC 상호작용을 위한 대화 라인 시퀀스입니다.",
  "type": "object",
  "properties": {
    "lines": {
      "description": "대화 라인의 배열입니다.",
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "speaker": {
            "description": "화자의 이름입니다.",
            "type": "string"
          },
          "text": {
            "description": "대화 라인의 내용입니다.",
            "type": "string"
          }
        },
        "required": ["speaker", "text"]
      }
    }
  },
  "required": ["lines"]
}
```