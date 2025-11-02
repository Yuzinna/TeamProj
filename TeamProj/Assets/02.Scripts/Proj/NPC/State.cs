using UnityEngine;

[System.Serializable]
public abstract class State
{
	public eNpcState state = eNpcState.Idle;
	public string stateName;
	public abstract void Enter(NPC entity);//시작

	public abstract void UpdateState(NPC entity);//업데이트

	public abstract void Exit(NPC entity);//끝
}
