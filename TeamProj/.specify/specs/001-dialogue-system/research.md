# 리서치 및 결정 사항: 다이얼로그 시스템

## 1. JSON 파싱(Parsing) 전략

- **결정**: Unity의 내장 기능인 `JsonUtility`를 사용하여 다이얼로그 데이터를 파싱합니다.
- **근거**:
  - 다이얼로그 데이터 구조(화자, 텍스트를 포함한 대사 목록)는 C# 객체에 직접 매핑될 수 있을 만큼 간단합니다.
  - `JsonUtility`는 빠르고 효율적이며, 외부 라이브러리가 필요 없어 의존성을 최소화하라는 프로젝트 제약 조건에 부합합니다.
  - Unity가 지원하는 모든 타겟 플랫폼에서 원활하게 작동합니다.
- **고려했던 대안**:
  - **Newtonsoft.Json (Json.NET)**: 더 강력하고 유연하지만, 오버헤드를 추가하는 외부 의존성입니다. 현재 범위에서는 LINQ to JSON과 같은 고급 기능이 불필요합니다.

## 2. DialogueManager 싱글톤(Singleton) 구현

- **결정**: `DialogueManager`에 정적 인스턴스(static instance) 패턴을 구현합니다.
  ```csharp
  public class DialogueManager : MonoBehaviour
  {
      public static DialogueManager Instance { get; private set; }

      private void Awake()
      {
          if (Instance != null && Instance != this)
          {
              Destroy(gameObject);
          }
          else
          {
              Instance = this;
              DontDestroyOnLoad(gameObject); // 씬 전환 시에도 매니저가 유지되어야 할 경우 선택적으로 사용
          }
      }
  }
  ```
- **근거**: 이 패턴은 다른 스크립트에서 다이얼로그를 시작하고 제어할 수 있는 간단한 전역 접근 지점을 제공합니다. 성능에 부담을 줄 수 있는 `FindObjectOfType` 호출을 피할 수 있습니다. `Awake` 메소드는 단 하나의 인스턴스만 존재하도록 보장합니다.
- **고려했던 대안**:
  - **서비스 로케이터(Service Locator) 패턴**: 설정 및 유지가 더 복잡합니다. 단일 매니저의 경우, 간단한 정적 싱글톤이 더 직관적입니다.
  - **의존성 주입(Dependency Injection)**: MonoBehaviour의 의존성이 주로 인스펙터(Inspector)를 통해 관리되는 Unity 컨텍스트 내에서, 이 기능의 범위에 비해 과도한 설계입니다.

## 3. UI 관리 및 텍스트 렌더링

- **결정**: UI는 UGUI로 구축합니다. 텍스트 렌더링에는 `TextMeshPro - Text` 컴포넌트를 권장합니다.
- **근거**: `TextMeshPro`는 기존 `UI.Text` 컴포넌트에 비해 우수한 텍스트 렌더링 품질, 고급 스타일링 옵션(굵게, 기울임, 색상 등), 그리고 더 나은 성능을 제공합니다. Unity에서 텍스트를 다루는 표준이 되었습니다.
- **고려했던 대안**:
  - **레거시 `UI.Text`**: 기본 텍스트에는 더 간단하지만, 고급 기능이 부족하고 시각적 품질이 떨어집니다. 장기적으로 `TextMeshPro`가 더 확장성 있습니다.

## 4. 타자기 효과(Typewriter Effect) 구현

- **결정**: 코루틴(Coroutine)을 사용하여 타자기 효과를 구현합니다.
- **근거**: 코루틴은 Unity에서 시간의 흐름에 따른 작업을 처리하는 표준 메커니즘입니다. 코루틴은 문자열의 문자를 하나씩 반복하며, 각 문자 사이에 `yield return new WaitForSeconds()` 지연을 두고 텍스트 컴포넌트에 추가할 수 있습니다. 이는 매우 효율적이고 제어(시작, 중지, 스킵)하기 쉽습니다.
- **고려했던 대안**:
  - **`Update()` 메소드와 타이머 사용**: `Update` 루프를 복잡하게 만들고, 자체 관리되는 코루틴에 비해 관리하기 어렵습니다. 타이머, 플래그 등 수동 상태 관리가 필요하며, 이는 코루틴이 암시적으로 처리해주는 부분입니다.