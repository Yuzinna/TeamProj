using UnityEngine;
using UnityEngine.Splines;

public class Luggage : MonoBehaviour
{
	public bool isSuspicious = false;  // 이상 물품 여부
	public Renderer rend;
	public SplineAnimate splineAnim;
	
	//모니터에 출력한적이 있다면
	public bool isdisplayed = false;
	private void Awake()
	{
		splineAnim = GetComponent<SplineAnimate>();
		
	}
	private void Start()
	{
		if (rend == null)
			rend = GetComponent<Renderer>();
		
		splineAnim.Container = SplineManager.Instance.splineList[3];
		splineAnim.Play();
		UpdateColor();
	}
	private void Update()
	{
		//애니메이트가 끝까지 재생되었다면
		if (splineAnim.ElapsedTime >= splineAnim.Duration&&!isdisplayed)
		{
			gameObject.GetComponent<LuggageData>().InitDisplayText();
			isdisplayed = true;
		}
	}
	public void SetSuspicious(bool value)
	{
		isSuspicious = value;
		UpdateColor();
	}

	private void UpdateColor()
	{
		// 색상으로 표시 (정상: 회색, 이상: 빨간색)
		//rend.material.color = isSuspicious ? Color.red : Color.gray;
	}
}
