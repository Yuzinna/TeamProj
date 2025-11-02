using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   
	private static UIManager _instance;
	public static UIManager Instance { get { return _instance; } }

	[Header("Canvas References")]
	public Canvas dialogueCanvas;   // 대화 UI 캔버스
	public Canvas camCanvas;        // 카메라/효과용 캔버스

	[Header("Fade UI")]
	[SerializeField] private Image fadePanel; // 페이드용 이미지
	[SerializeField] private float fadeDuration = 1.2f; // 페이드 속도

	private void Awake()
	{
		// 싱글톤 설정
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
	}
	// 페이드 인 (화면 밝게)
	public void FadeIn(Action onComplete = null)
	{
		StartCoroutine(FadeRoutine(1f, 0f, onComplete));
	}

	// 페이드 아웃 (화면 어둡게)
	public void FadeOut(Action onComplete = null)
	{
		StartCoroutine(FadeRoutine(0f, 1f, onComplete));
	}
	// 실제 페이드 처리 코루틴
	private IEnumerator FadeRoutine(float from, float to, Action onComplete)
	{
		if (fadePanel == null)
		{
			Debug.LogWarning("[UIManager] Fade panel not assigned!");
			yield break;
		}

		float elapsed = 0f;
		Color color = fadePanel.color;
		color.a = from;
		fadePanel.color = color;

		fadePanel.gameObject.SetActive(true);

		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
			color.a = alpha;
			fadePanel.color = color;
			yield return null;
		}

		color.a = to;
		fadePanel.color = color;

		if (to == 0f)
			fadePanel.gameObject.SetActive(false);

		onComplete?.Invoke();
	}

	// 특정 Canvas 켜기/끄기
	public void ShowDialogueCanvas(bool show)
	{
		if (dialogueCanvas != null)
			dialogueCanvas.enabled = show;
	}

	public void ShowCamCanvas(bool show)
	{
		if (camCanvas != null)
			camCanvas.enabled = show;
	}
}
