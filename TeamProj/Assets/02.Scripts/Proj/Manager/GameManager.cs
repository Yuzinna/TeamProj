using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 추가

/// <summary>
/// 게임의 전체적인 상태(GameState), 시간, 점수, 규칙 및 NPC 흐름을 총괄합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴: 다른 스크립트에서 GameManager.Instance로 쉽게 접근할 수 있도록 합니다.
    public static GameManager Instance;

    [Header("Game State")]
    // 게임의 현재 상태 (예: DayStarting, Playing, DayEnding)
    public GameState currentState;
    // 현재 몇 번째 날인지 추적합니다.
    public int currentDay = 1;

    [Header("UI Elements")]
    // UI에 시간을 표시할 텍스트
    public TextMeshProUGUI dayTimerText;
    // UI에 점수를 표시할 텍스트
    public TextMeshProUGUI scoreText;
    // '하루 시작'을 알리는 UI 패널
    public GameObject dayStartPanel;
    // '하루 종료'를 알리는 UI 패널
    public GameObject dayEndPanel;

    [Header("Time & Score")]
    // 하루의 총 근무 시간 (초)
    public float secondsPerDay = 120f; 
    // 남은 시간을 추적하는 내부 변수
    private float dayTimer;
    // 현재 점수를 저장하는 변수
    private int score;

    [Header("NPC Settings")]
    // 생성할 NPC의 원본 프리팹
    public GameObject npcPrefab;
    // NPC가 처음 생성될 위치
    public Transform npcSpawnPoint;
    // NPC가 이동해서 멈출 심사대 위치
    public Transform deskPosition;
    // 승인된 NPC가 퇴장할 위치
    public Transform exitPosition;
    // 거절된 NPC가 퇴장할 위치
    public Transform rejectedPosition;
    // 현재 심사 중인 NPC를 가리킵니다.
    private NPC currentNpc;

    // 게임의 주요 상태를 명확하게 정의합니다.
    public enum GameState
    {
        DayStarting, // 하루 시작 (UI 표시)
        Playing,     // 게임 플레이 중
        Paused,      // 일시 정지
        DayEnding,   // 하루 종료 (결과 정산)
        GameOver     // 게임 오버
    }

    // 게임 오브젝트가 처음 생성될 때 호출됩니다.
    private void Awake()
    {
        // 만약 이미 다른 GameManager 인스턴스가 있다면 이 오브젝트는 파괴하고, 없다면 자신을 인스턴스로 설정합니다.
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // 첫 프레임이 업데이트되기 전에 호출됩니다.
    private void Start()
    {
        // 게임이 시작되면 '하루 시작' 상태부터 시작합니다.
        ChangeState(GameState.DayStarting);
    }

    // 매 프레임마다 호출됩니다.
    private void Update()
    {
        // 게임이 'Playing' 상태일 때만 근무 시간 타이머를 감소시킵니다.
        if (currentState == GameState.Playing)
        {
            UpdateDayTimer();
        }
    }

    /// <summary>
    /// 게임의 상태를 변경하고, 상태에 따른 초기화 작업을 수행합니다.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"Game State Changed to: {currentState}");

        // 새로운 상태에 따라 다른 작업을 수행합니다.
        switch (currentState)
        {
            case GameState.DayStarting:
                StartCoroutine(DayStartSequence());
                break;
            case GameState.Playing:
                CallNextNpc();
                break;
            case GameState.Paused:
                // TODO: 게임 일시 정지 로직 (예: Time.timeScale = 0;)
                break;
            case GameState.DayEnding:
                StartCoroutine(DayEndSequence());
                break;
            case GameState.GameOver:
                // TODO: 게임 오버 로직
                break;
        }
    }

    /// <summary>
    /// '하루 시작' 연출을 위한 코루틴입니다.
    /// </summary>
    IEnumerator DayStartSequence()
    {
        dayStartPanel.SetActive(true);
        yield return new WaitForSeconds(3f); // 3초 대기
        dayStartPanel.SetActive(false);
        
        // 타이머와 점수를 초기화합니다.
        dayTimer = secondsPerDay;
        score = 0;
        UpdateScore(0);

        // 'Playing' 상태로 전환하여 실제 게임을 시작합니다.
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// '하루 종료' 연출 및 정산을 위한 코루틴입니다.
    /// </summary>
    IEnumerator DayEndSequence()
    {
        if (currentNpc != null)
        {
            Destroy(currentNpc.gameObject);
            currentNpc = null;
        }

        dayEndPanel.SetActive(true);
        Debug.Log($"Day {currentDay} ended. Final Score: {score}");

        yield return new WaitForSeconds(5f); // 5초 대기
        
        dayEndPanel.SetActive(false);
        currentDay++; // 다음 날로
        ChangeState(GameState.DayStarting); // 다시 '하루 시작' 상태로
    }

    /// <summary>
    /// 근무 시간 타이머를 업데이트하고 UI에 표시합니다.
    /// </summary>
    void UpdateDayTimer()
    {
        dayTimer -= Time.deltaTime;
        
        if (dayTimerText != null)
        {
            int minutes = Mathf.FloorToInt(dayTimer / 60);
            int seconds = Mathf.FloorToInt(dayTimer % 60);
            dayTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (dayTimer <= 0)
        {
            ChangeState(GameState.DayEnding);
        }
    }

    /// <summary>
    /// 점수를 업데이트하고 UI에 표시합니다.
    /// </summary>
    public void UpdateScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    // --- NPC 관리 로직 ---

    /// <summary>
    /// 다음 NPC를 생성하고 심사대로 이동시킵니다.
    /// </summary>
    public void CallNextNpc()
    {
        // 다른 NPC가 있거나, 게임이 'Playing' 상태가 아니면 실행하지 않습니다.
        if (currentNpc != null || currentState != GameState.Playing) return;

        GameObject npcObject = Instantiate(npcPrefab, npcSpawnPoint.position, npcSpawnPoint.rotation);
        NPC newNpc = npcObject.GetComponent<NPC>();
        currentNpc = newNpc;
        newNpc.deskPosition = this.deskPosition;
        newNpc.exitPosition = this.exitPosition;
        newNpc.rejectedPosition = this.rejectedPosition;
        newNpc.GoToDesk();
    }

    /// <summary>
    /// 심사 결정을 처리하고 점수를 반영한 후, NPC를 퇴장시킵니다.
    /// </summary>
    public void ProcessCurrentNpcDecision(bool isApproved, bool isCorrectDecision)
    {
        if (currentNpc == null || currentNpc.currentState != NPC.State.AtDesk) return;

        if (isCorrectDecision)
        {
            UpdateScore(10);
        }
        else
        {
            UpdateScore(-5);
        }

        currentNpc.ProcessDecision(isApproved);
        currentNpc = null;

        if (currentState == GameState.Playing)
        {
            StartCoroutine(CallNextNpcAfterDelay(2.0f));
        }
    }

    /// <summary>
    /// 다음 NPC를 부르기 전 잠시 지연시간을 줍니다.
    /// </summary>
    private IEnumerator CallNextNpcAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CallNextNpc();
    }
}

/*
[OLD CODE]

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

	//하나의 게임매니저에 모든 매니저를 다 연결하고 쓰고싶음
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
	//일일 다이얼로그가 
	

	//엔티티들의 업데이트를 실행
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
		// 첫 NPC를 일단 가장 가까운 것으로 설정
		NPC nearest = entitys[0];
		float minDist = Vector3.Distance(nearest.transform.position, point);

		// 모든 NPC를 순회하면서 가장 가까운 NPC 찾기
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
		// NPC가 하나도 없으면 명령 취소
		if (entitys.Count == 0) return;

		// 첫 NPC를 일단 가장 가까운 것으로 설정
		NPC nearest = entitys[0];
		float minDist = Vector3.Distance(nearest.transform.position, point);

		// 모든 NPC를 순회하면서 가장 가까운 NPC 찾기
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
		// 사전 처리 (이전 단계 종료자)
		//OnExitPhase(curPhase);

		curPhase?.Exit();
		curPhase = newPhase;
		Debug.Log($"[GameManager] Changed to phase: {curPhase}");

		// 사후 처리 (새 단계 초기화자)
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
				//dialogueManager.StartDialogue("오늘부터 당신은 입국 심사관으로 일하게 되었소!");
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
				// 입국 심사 종료 등
				break;
		}
		}
}

*/