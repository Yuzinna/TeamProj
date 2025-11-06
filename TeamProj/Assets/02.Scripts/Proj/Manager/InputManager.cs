using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public GenerateCSharpInput input;
	private static InputManager _instance;

	//다른 스크립트들에게 완료됨을 전달함
	public static event Action OnInputReady;
	public static InputManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindAnyObjectByType<InputManager>();
			return _instance;
		}

	}

	private void Start()
	{
		OnInputReady?.Invoke();
	}
	private void Awake()
	{
		//이거 한번 실험 해보기
		//input = new GenerateCSharpInput();
		input = GetComponent<GenerateCSharpInput>();
		if (_instance != null&& _instance!= this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
		
	}
	private void OnEnable()
	{
		
	}
	private void OnDisable()
	{
		
	}
	

}
