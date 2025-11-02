using UnityEngine;
using UnityEngine.Splines;

public class SplineManager : MonoBehaviour
{
	public static SplineManager Instance { get; private set; }

	public SplineContainer[] splineList;

	private void Awake()
	{
		if(Instance!= null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	public SplineContainer GetSpline(int index)
	{
		if (index < 0 || index >= splineList.Length)
		{
			
			Debug.LogError($"SplineManager: ÀÎµ¦½º {index}°¡ ¹üÀ§¸¦ ¹þ¾î³µ½À´Ï´Ù!");
			return null;
		}
		return splineList[index];
	}
}
