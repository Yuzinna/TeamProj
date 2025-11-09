using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PassengerSpawner : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject passengerPrefab;
    public Transform spawnPoint;
    public Transform deskPoint;
    public Transform exitPointRight;
    public Transform exitPointLeft;

    [Header("UI Elements")]
    public Button passButton;
    public Button rejectButton;
    public Button nextPassengerButton;
    public TextMeshProUGUI scoreText;
    public PassportUI passportUI;

    [Header("Passenger Data Templates")]
    public List<PassengerData> passengerTemplates;

    private GameObject currentPassenger;
    private int score = 0;

    void Start()
    {
        //버튼 액션 등록
        passButton?.onClick.AddListener(OnPassButtonClicked);
        rejectButton?.onClick.AddListener(OnRejectButtonClicked);
        nextPassengerButton?.onClick.AddListener(ProcessNextPassenger);

        passportUI?.Hide();

        SetJudgementButtonsInteractable(false);
        SetNextPassengerButtonInteractable(true);

        score = 0;
        UpdateScoreUI();
    }

    public void ProcessNextPassenger()
    {
        SetNextPassengerButtonInteractable(false);
        passportUI?.Hide();
        SpawnAndMovePassenger();
    }

    public void OnPassButtonClicked()
    {
        HandleJudgement(true);
    }

    public void OnRejectButtonClicked()
    {
        HandleJudgement(false);
    }

    void HandleJudgement(bool isPass)
    {
        if (currentPassenger == null) return;
        SetJudgementButtonsInteractable(false);

        PassengerController passengerController = currentPassenger.GetComponent<PassengerController>();
        if (passengerController == null || passengerController.data == null) return;

        PassengerData passengerData = passengerController.data;
        bool isExpired = passengerData.passportExpirationDate < DateTime.Now;

        if (isPass)
        {
            if (isExpired) { score -= 30; Debug.Log("실수! 만료된 여권 통과"); }
            else { score += 10; Debug.Log("정답! 유효한 여권 통과"); }
        }
        else
        {
            if (isExpired) { score += 10; Debug.Log("정답! 만료된 여권 거절"); }
            else { score -= 30; Debug.Log("실수! 유효한 여권 거절"); }
        }
        UpdateScoreUI();

        Transform exitPoint = isPass ? exitPointRight : exitPointLeft;
        passengerController.ExitTo(exitPoint.position, OnPassengerExited);
    }

    void OnPassengerExited()
    {
        if (currentPassenger != null)
        {
            Destroy(currentPassenger);
            currentPassenger = null;
        }
        SetNextPassengerButtonInteractable(true);
    }

    void SpawnAndMovePassenger()
    {
        if (passengerPrefab == null || spawnPoint == null || deskPoint == null) return;
        if (passengerTemplates == null || passengerTemplates.Count == 0)
        {
            Debug.LogError("PassengerSpawner에 연결된 승객 데이터 템플릿이 없습니다!");
            return;
        }

        currentPassenger = Instantiate(passengerPrefab, spawnPoint.position, Quaternion.identity);

        PassengerData selectedTemplate = passengerTemplates[UnityEngine.Random.Range(0, passengerTemplates.Count)];
        PassengerData runtimePassengerData = Instantiate(selectedTemplate);

        bool isExpired = UnityEngine.Random.value > 0.5f;
        if (isExpired)
        {
            runtimePassengerData.passengerName = "Expired Passport Holder";
            runtimePassengerData.passportExpirationDate = DateTime.Now.AddDays(-UnityEngine.Random.Range(1, 365 * 5));
        }
        else
        {
            runtimePassengerData.passengerName = "Valid Passport Holder";
            runtimePassengerData.passportExpirationDate = DateTime.Now.AddDays(UnityEngine.Random.Range(365, 365 * 5));
        }
        
        Debug.Log($"승객 생성됨. 이름: {runtimePassengerData.passengerName}, 만료일: {runtimePassengerData.passportExpirationDate.ToShortDateString()}");

        PassengerController passengerController = currentPassenger.GetComponent<PassengerController>();
        if (passengerController != null)
        {
            passengerController.data = runtimePassengerData;
            // 승객 도착 후 OnPassengerArrived 콜백을 호출합니다.
            passengerController.MoveTo(deskPoint.position, OnPassengerArrived);
        }
    }

    // 승객이 데스크에 도착했을 때 호출될 콜백 함수
    void OnPassengerArrived()
    {
        if (currentPassenger == null) return;

        PassengerController passengerController = currentPassenger.GetComponent<PassengerController>();
        if (passengerController == null || passengerController.data == null) return;

        PassengerData passengerData = passengerController.data;

        if (passportUI != null)
        {
            passportUI.UpdateInfo(passengerData);
            passportUI.StartPeek(); // Show() 대신 StartPeek() 호출
        }

        SetJudgementButtonsInteractable(true);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void SetJudgementButtonsInteractable(bool isInteractable)
    {
        if (passButton != null)
        {
            passButton.interactable = isInteractable;
        }
        if (rejectButton != null)
        {
            rejectButton.interactable = isInteractable;
        }
    }

    void SetNextPassengerButtonInteractable(bool isInteractable)
    {
        if (nextPassengerButton != null)
        {
            nextPassengerButton.interactable = isInteractable;
        }
    }
}