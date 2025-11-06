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
			
			
		});

	}     // 상태 진입 시
	public override void Updated() 
	{

	}    // 매 프레임 갱신
	public override void Exit() 
	{
	
	}      // 상태 종료 시

}
