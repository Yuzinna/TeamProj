using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionAsset3D : PrBase3DInput
{
	public InputActionAsset inputActionAssets;
	private InputActionMap playerMap;
	private InputActionMap carMap;
	private InputActionMap changerMap;
	private InputActionMap uiMap;
	private bool isPlaying;

	private InputAction moveAction;
	private InputAction lookAction;
	private InputAction jumpAction;
	private InputAction sprintAction;

	public Canvas ui;
	private void Awake()
	{
		playerMap = inputActionAssets.FindActionMap("Player");

		moveAction = playerMap.FindAction("Move");
		lookAction = playerMap.FindAction("Look");
		jumpAction = playerMap.FindAction("Jump");
		sprintAction = playerMap.FindAction("Sprint");	



		

		//moveAction = playerMap.FindAction("Move");
		//lookAction = playerMap.FindAction("Look");
		//jumpAction = playerMap.FindAction("Jump");
		//sprintAction = playerMap.FindAction("Sprint");
	}

	private void OnEnable()
	{
		playerMap = inputActionAssets.FindActionMap("Player");
		playerMap.Enable();
		moveAction = playerMap.FindAction("Move");
		lookAction = playerMap.FindAction("Look");
		jumpAction = playerMap.FindAction("Jump");
		sprintAction = playerMap.FindAction("Sprint");

		moveAction.performed += OnMove;
		lookAction.performed += OnLook;
		jumpAction.performed += OnJump;
		sprintAction.performed += OnSprint;
		sprintAction.canceled += (InputAction.CallbackContext value) => { sprint = false; };

		moveAction.canceled += StopMove;
		lookAction.canceled += (InputAction.CallbackContext value) => { look = Vector2.zero; };

		carMap = inputActionAssets.FindActionMap("Car");
		carMap.FindAction("Move").performed += OnMove;
		carMap.FindAction("Move").canceled += StopMove;
		carMap.FindAction("Look").performed += OnLook;
		carMap.FindAction("Look").canceled += (InputAction.CallbackContext value) => { look = Vector2.zero; };

		changerMap = inputActionAssets.FindActionMap("Changer");
		changerMap.FindAction("Change").started += OnChange;
		changerMap.Enable();

		uiMap = inputActionAssets.FindActionMap("UI");
	}

	public void OnChange(InputAction.CallbackContext value)
	{
		isPlaying = !isPlaying;
		if (!isPlaying)
		{
			uiMap.Enable();
			playerMap.Disable();
			
			ui.gameObject.SetActive(true);
			StartCoroutine(NextFrame());

		}
		else
		{
			uiMap.Disable();
			playerMap.Enable();
			ui.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}
	IEnumerator NextFrame()
	{
		yield return null;
		Time.timeScale = 0;
	}
	public void OnMove(InputAction.CallbackContext value)
	{
		move = value.ReadValue<Vector2>();
	}

	public void StopMove(InputAction.CallbackContext value)
	{
		move = Vector2.zero;
	}

	public void OnLook(InputAction.CallbackContext value)
	{
		look = value.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext value)
	{
		jump = value.performed;
	}

	public void OnSprint(InputAction.CallbackContext value)
	{
		sprint = value.performed;
	}
}
