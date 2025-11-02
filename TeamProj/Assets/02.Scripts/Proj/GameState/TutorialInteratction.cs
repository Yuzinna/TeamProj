using UnityEngine;

public class TutorialInteratction : MonoBehaviour
{
	public void EnableForTutorial(GameObject target)
	{
		var interactable = target.GetComponent<Interactable>();
		if (interactable != null)
			interactable.isActiveForTutorial = true;

		Highlight(target, true);
	}

	public void DisableForTutorial(GameObject target)
	{
		var interactable = target.GetComponent<Interactable>();
		if (interactable != null)
			interactable.isActiveForTutorial = false;

		Highlight(target, false);
	}

	private void Highlight(GameObject target, bool enable)
	{
		var renderer = target.GetComponent<Renderer>();
		if (renderer != null)
			renderer.material.color = enable ? Color.yellow : Color.white;
	}
}
