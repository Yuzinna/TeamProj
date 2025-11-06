using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
	[Header("UI Elements")]
	public GameObject dialoguePanel; // 대화 UI 패널
	public TMP_Text nameText;        // 화자 이름 텍스트
	public TMP_Text dialogueText;    // 대사 텍스트

	[Header("Highlight Particle")]
	public GameObject particlePrefab;  // 생성할 파티클 프리팹
	private GameObject currentParticle; // 현재 활성화된 파티클

	private List<DialogueLine> allLines = new List<DialogueLine>(); // 전체 대사 저장
	private int currentIndex = 0;   // 현재 표시 중인 대사 인덱스
	private bool isTyping = false;  // 타이핑 효과 진행 중 여부

	public DialogueLine curLine;
	public DialogueLine CurLine { get => allLines[currentIndex]; set => curLine = value; }

	void Awake()
	{
		dialoguePanel.SetActive(false); // 시작 시 대화 패널 숨김
	}

	private void Start()
	{
		// Input System 이벤트 등록
		InputManager.Instance.input.inputAsset.Dialogue.Next.performed += OnNext;
		InputManager.Instance.input.inputAsset.Dialogue.Prev.performed += OnPrev;

		InputManager.Instance.input.inputAsset.Dialogue.Enable();
	}

	private void OnDestroy()
	{
		// 이벤트 해제
		//InputManager.Instance.input.inputAsset.Dialogue.Disable();
	}

	// 대화 시작
	public void StartDialogue(DialogueLine[] dialogueLines)
	{
		dialoguePanel.SetActive(true); // 대화 패널 표시
		allLines.Clear();
		allLines.AddRange(dialogueLines); // 배열을 리스트로 복사
		currentIndex = 0;                 // 첫 번째 대사부터 시작
		DisplayCurrentSentence();         // 첫 대사 표시
	}

	// 다음 대사
	public void OnNext(InputAction.CallbackContext value)
	{
		if (!dialoguePanel.activeSelf) return;

		if (isTyping)
		{
			// 타이핑 중이면 문장 전체 표시
			StopAllCoroutines();
			dialogueText.text = allLines[currentIndex].line;
			isTyping = false;
		}
		else
		{
			// 현재 대사 끝 액션 호출
			allLines[currentIndex].onLineEnd?.Invoke();

			if (currentIndex < allLines.Count - 1)
			{
				currentIndex++;
				DisplayCurrentSentence();
			}
			else
			{
				EndDialogue();
			}
		}
	}

	// 이전 대사
	public void OnPrev(InputAction.CallbackContext value)
	{
		if (!dialoguePanel.activeSelf) return;

		if (isTyping)
		{
			StopAllCoroutines();
			dialogueText.text = allLines[currentIndex].line;
			isTyping = false;
		}
		else
		{
			allLines[currentIndex].onLineEnd?.Invoke();

			if (currentIndex > 0)
			{
				currentIndex--;
				DisplayCurrentSentence();
			}
		}
	}

	// 현재 대사 표시
	private void DisplayCurrentSentence()
	{
		DialogueLine line = allLines[currentIndex];
		nameText.text = line.speaker;

		// 🔹 highlightObject 위치에 파티클 생성 및 이전 파티클 제거
		HandleHighlightParticle(line);

		StopAllCoroutines();
		StartCoroutine(TypeSentence(line.line));

		// 대사 시작 액션 호출
		line.onLineStart?.Invoke();
	}

	// 타이핑 효과
	private IEnumerator TypeSentence(string sentence)
	{
		isTyping = true;
		dialogueText.text = "";

		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(0.03f);
		}

		isTyping = false;
	}

	// highlightObject 위치에 파티클 생성
	private void HandleHighlightParticle(DialogueLine line)
	{
		// 이전 파티클 제거
		if (currentParticle != null)
		{
			Destroy(currentParticle);
			currentParticle = null;
		}

		// highlightObject가 있으면 새 위치에 파티클 생성
		if (line.highlightObject != null && particlePrefab != null)
		{
			Vector3 spawnPos = line.highlightObject.transform.position;
			currentParticle = Instantiate(particlePrefab, spawnPos, Quaternion.identity);
			currentParticle.GetComponent<ParticleSystem>().Play();
		}
	}

	// 대화 종료
	private void EndDialogue()
	{
		dialoguePanel.SetActive(false);

		// 남은 파티클 제거
		if (currentParticle != null)
		{
			Destroy(currentParticle);
			currentParticle = null;
		}
	}
}