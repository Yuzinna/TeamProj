using UnityEngine;

public enum GamePhase
{
	Boot,
	Tutorial,
	MainGame,
	Pause,
	Result
}
public abstract class GamePhaseBase : MonoBehaviour
{
	protected GameManager gameManager;

	public GamePhaseBase(GameManager manager)
	{
		gameManager = manager;
	}

	// 각 상태가 공통으로 가져야 할 메서드
	public virtual void Enter() { }     // 상태 진입 시
	public virtual void Updated() { }    // 매 프레임 갱신
	public virtual void Exit() { }      // 상태 종료 시
}

