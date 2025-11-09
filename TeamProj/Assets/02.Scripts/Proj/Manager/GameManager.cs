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
        
    }

    // 매 프레임마다 호출됩니다.
    private void Update()
    {
        
    }
    public void UpdateScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
