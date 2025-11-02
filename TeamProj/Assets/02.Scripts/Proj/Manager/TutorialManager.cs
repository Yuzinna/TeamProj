using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class TutorialManager : MonoBehaviour
{
	private static TutorialManager _instance;

	
	private GameObject currentHighright;
	public static TutorialManager Instance
	{
		get { return _instance; }
	}
	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}
	//현재 대화에 하이라이트 오브젝트가 존재한다면 그걸 클릭해야 다음 텍스트로 넘어가 아니면 방향키로 텍스트를 넘겼다면 넘어가

}
