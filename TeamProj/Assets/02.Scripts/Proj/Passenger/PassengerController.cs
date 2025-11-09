using DG.Tweening; // DOTween 라이브러리를 사용하기 위해 필요합니다.
using UnityEngine;
using System; // Action을 사용하기 위해 필요합니다.

// 승객 게임 오브젝트의 움직임을 제어하는 스크립트입니다.
// 승객이 스폰 지점에서 데스크로 이동하는 등의 애니메이션을 담당합니다.
public class PassengerController : MonoBehaviour
{
	[Header("Movement Settings")]
	public float moveDuration = 3.0f;   // 승객이 데스크로 이동하는 데 걸리는 시간
	public float exitMoveDuration = 1.5f; // 승객이 퇴장하는 데 걸리는 시간
	public Ease easeType = Ease.Linear; // 승객 이동 애니메이션의 타입 (DOTween 사용)

	// 이 승객 게임 오브젝트가 가지고 있는 PassengerData (ScriptableObject 인스턴스)입니다.
	// PassengerSpawner에서 런타임에 생성된 데이터가 여기에 할당됩니다.
	public PassengerData data;

	// 스크립트가 비활성화되거나 게임 오브젝트가 파괴될 때 호출됩니다.
	// DOTween 트윈을 안전하게 종료하여 메모리 누수를 방지합니다.
	void OnDestroy()
	{
		transform.DOKill(); // 이 게임 오브젝트에 연결된 모든 DOTween 트윈을 중지합니다.
	}

	/// <summary>
	/// 승객을 데스크로 이동시키고, 완료 시 콜백을 실행합니다.
	/// </summary>
	/// <param name="destination">승객이 이동할 최종 위치</param>
    /// <param name="onComplete">도착 애니메이션이 끝난 후 호출될 함수</param>
	public void MoveTo(Vector3 destination, Action onComplete)
	{
		// DOTween을 사용하여 현재 위치에서 목적지까지 moveDuration 시간 동안 이동합니다.
		transform.DOMove(destination, moveDuration)
		.SetEase(easeType) // 설정된 Ease 타입으로 애니메이션을 적용합니다.
		.OnComplete(() => // 이동이 완료된 후 실행될 콜백 함수입니다.
		{
		   Debug.Log("승객이 데스크에 도착했습니다.");
           onComplete?.Invoke();
		});
	}

    /// <summary>
    /// 승객을 지정된 목적지로 퇴장시키고, 완료 시 콜백을 실행합니다.
    /// </summary>
    /// <param name="destination">퇴장할 최종 위치</param>
    /// <param name="onComplete">퇴장 애니메이션이 끝난 후 호출될 함수</param>
    public void ExitTo(Vector3 destination, Action onComplete)
    {
        transform.DOMove(destination, exitMoveDuration)
        .SetEase(easeType)
        .OnComplete(() =>
        {
            // onComplete 콜백이 null이 아니면 실행합니다.
            onComplete?.Invoke();
        });
    }
}
