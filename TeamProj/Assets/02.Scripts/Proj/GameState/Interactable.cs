using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
	//public UnityEvent onClick;
	public bool isActiveForTutorial = true; // 튜토리얼에서 활성화 여부
	public bool isTutorialMode = false;     // 현재 튜토리얼 모드인지 직접 설정

	public void OnClick(InputAction.CallbackContext value)
	{
		if (!isActiveForTutorial)
			return; // 튜토리얼 중인데 활성화되지 않은 오브젝트면 무시

		// 기존 클릭 이벤트 처리
		Debug.Log($"{gameObject.name} clicked!");
	}
}
