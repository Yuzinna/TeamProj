using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Barracuda;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class GameManager : MonoBehaviour
{

	//일단 게임매니저가 모든 매니저를 다 지니고 있게함
	public DialogueManager dialogueManager;
	public InputManager inputManager;
	public TutorialManager tutorialManager;

	
	[SerializeField]
	GameObject NPCPrefab;

	[SerializeField]
	private PlayerCtrl playerController;
	//public GamePhase CurrentPhase { get; private set; } = GamePhase.Boot;

	private List<NPC> entitys = new List<NPC>();
	private float spawnInterval = 3f;
	public int maxNpcCount = 2;
	private int currentCount = 0;


	public static GameManager Instance;

	public Dialogue curDialogue;
	private GamePhaseBase curPhase; //현재 게임의 상태

	public PlayerCtrl PlayerController { get => playerController; set => playerController = value; }
	public bool IsStartDialoge { get => isStartDialoge; set => isStartDialoge = value; }

	private bool isStartDialoge =false;

	
	private void Awake()
	{
		if (Instance != null) 
			Destroy(gameObject);
		else Instance = this;
	}
	private void Start()
	{
		curPhase = new TutorialState(this);//현재 게임상태를 튜토리얼로 초기화
		ChangePhase(curPhase);//현재 게임상태(튜토리얼)스타트
		//StartTutorial();
		//SpawnNpcOne();//튜토리얼용 한명 생성
		//StartCoroutine(SpawnNpcs());
	}
	
	public void SpawnNpcOne(Action action = null)
	{
		GameObject npcObj = Instantiate(NPCPrefab, new Vector3(-30,1,-40), Quaternion.identity);
		NPC entity = npcObj.GetComponent<NPC>();
		entity.Setup("Npc");
		entitys.Add(entity);
		entity.ChangeState(eNpcState.WalkToDesk);
		currentCount++;
		action?.Invoke();
	}
	IEnumerator SpawnNpcs()
	{
		while (currentCount<maxNpcCount)
		{
			SpawnNpcOne();
			
			yield return new WaitForSeconds(3.0f);
		}
	}
	private void Update()
	{
		EntityUpdate();
		
	
	}
	//현재 다이얼로그가 
	

	//오브젝트들의 업데이트를 실행
	private void EntityUpdate()
	{
		for (int i = 0; i < entitys.Count; i++)
		{
			entitys[i].updated();
		}
	}
	public NPC NearestNPC(Vector3 point)
	{
		if (entitys.Count == 0) return null;
		// 첫 NPC를 일단 기준으로 설정
		NPC nearest = entitys[0];
		float minDist = Vector3.Distance(nearest.transform.position, point);

		// 모든 NPC를 돌면서 가장 가까운 NPC 찾기
		foreach (var npc in entitys)
		{
			float dist = Vector3.Distance(npc.transform.position, point);
			if (dist < minDist)
			{
				nearest = npc;
				minDist = dist;
			}
		}
		return nearest;

	}
	public void CommandNearestNPC(Vector3 point,eCommandNpc ecommand)
	{
		// NPC가 하나도 없으면 실행 중단
		if (entitys.Count == 0) return;

		// 첫 NPC를 일단 기준으로 설정
		NPC nearest = entitys[0];
		float minDist = Vector3.Distance(nearest.transform.position, point);

		// 모든 NPC를 돌면서 가장 가까운 NPC 찾기
		foreach (var npc in entitys)
		{
			float dist = Vector3.Distance(npc.transform.position, point);
			if (dist < minDist)
			{
				nearest = npc;
				minDist = dist;
			}
		}

		// 찾은 NPC에게 이동 명령
		if (ecommand == eCommandNpc.WalkToEnter)
		{
			nearest.ChangeState(eNpcState.WalkToEnter);
		}
		else if (ecommand ==eCommandNpc.WalkToExit)
		{
			nearest.ChangeState(eNpcState.WalkToExit);
		}
		else if (ecommand == eCommandNpc.SubmitLuggage)
		{
			nearest.isSubmit = true;
			nearest.curSubmit = eSubmit.Luggage;
		}
		else if (ecommand == eCommandNpc.SubmitPassport)
		{
			nearest.isSubmit = true;
			nearest.curSubmit = eSubmit.Passport;
		}
		else if(ecommand == eCommandNpc.WalkWithPolice)
		{
			nearest.ChangeState(eNpcState.WalkWithPolice);
		}
		
	}

	public void ChangePhase(GamePhaseBase newPhase)
	{
		// 종료 처리 (이전 단계 종료용)
		//OnExitPhase(curPhase);

		curPhase?.Exit();
		curPhase = newPhase;
		Debug.Log($"[GameManager] Changed to phase: {curPhase}");

		// 시작 처리 (새 단계 초기화용)
		curPhase.Enter();
		//OnEnterPhase(CurrentPhase);
	}
	private void OnEnterPhase(GamePhase phase)
	{
		switch (phase)
		{
			case GamePhase.Tutorial:
				//tutorialManager.StartTutorial();
				break;

			case GamePhase.MainGame:
				//dialogueManager.StartDialogue("이제 본격적인 여권 검사를 시작합시다!");
				break;

			case GamePhase.Result:
				//uiManager.ShowResultUI();
				break;
		}
	}
	private void OnExitPhase(GamePhase phase)
	{
		switch (phase)
		{
			case GamePhase.Tutorial:
				//tutorialManager.EndTutorial();
				break;

			case GamePhase.MainGame:
				// 여권 검사 중단 등
				break;
		}
	}
}
