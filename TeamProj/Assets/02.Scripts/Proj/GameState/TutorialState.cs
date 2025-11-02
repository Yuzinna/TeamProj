using System.Threading.Tasks;
using UnityEngine;

public class TutorialState : GamePhaseBase
{

	private int tutorialStep = 0;
	public TutorialState(GameManager manager) : base(manager)
	{
	}
	public override void Enter()  
	{
		// 화면 페이드인
		UIManager.Instance.FadeIn(async () =>
		{
			Debug.Log("Fade-in 완료, 플레이어 이동 시작");

			// 플레이어가 데스크까지 걸어오게 하기
			//Vector3 deskPosition = gameManager.deskTransform.position; // 데스크 위치
			gameManager.PlayerController.StartWalkToDesk(async() =>
			{
				Debug.Log("플레이어 데스크 도착");

				// 도착 후 지윤등장

				gameManager.SpawnNpcOne();
				//여기 부분을 6초뒤에 실행
				await Task.Delay(6000);
				gameManager.dialogueManager.StartDialogue(gameManager.curDialogue.Lines);
				gameManager.IsStartDialoge = true;
			});
		});

	}     // 상태 진입 시
	public override void Updated() 
	{

	}    // 매 프레임 갱신
	public override void Exit() 
	{
	
	}      // 상태 종료 시

}
