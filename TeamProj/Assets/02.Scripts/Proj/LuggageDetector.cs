using UnityEngine;

public class LuggageDetector : MonoBehaviour
{
	public Light alertLight; // 경고 표시용 (씬에 빨간색 Point Light 등)
	public AudioSource alertSound; // 경고음 (선택)
	public float alertDuration = 2f;

	private void OnTriggerEnter(Collider other)
	{
		Luggage luggage = other.GetComponent<Luggage>();
		if (luggage != null)
		{
			if (luggage.isSuspicious)
			{
				Debug.Log($"[DETECTOR] 이상 물품 감지: {luggage.name}");
				StartCoroutine(Alert());
			}
		}
	}

	private System.Collections.IEnumerator Alert()
	{
		if (alertLight != null) alertLight.enabled = true;
		if (alertSound != null) alertSound.Play();

		yield return new WaitForSeconds(alertDuration);

		if (alertLight != null) alertLight.enabled = false;
	}
}
