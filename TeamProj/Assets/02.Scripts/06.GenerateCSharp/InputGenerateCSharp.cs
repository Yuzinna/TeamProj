using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputGenerateCSharp : Base3DInput
{

    _3DControl input;
	private bool isPlaying;

	private void Awake()
	{
		input = new _3DControl();
		
		
	}

	public Canvas ui;
	

	private void OnEnable()
	{

		
		input.Player.Enable();
		input.Player.Move.performed += OnMove;
		input.Player.Move.canceled += StopMove;
		input.Player.Look.performed += OnLook;
		input.Player.Look.canceled += (InputAction.CallbackContext value)=> { look = Vector2.zero; };
		input.Player.Jump.performed += OnJump;
		input.Player.Sprint.performed += OnSprint;
		input.Player.Sprint.canceled += (InputAction.CallbackContext value) => { sprint = false; };

		input.Changer.Change.started += OnChange;
		input.Changer.Enable();
		
	}

	public void OnChange(InputAction.CallbackContext value)
	{
		isPlaying = !isPlaying;
		if (!isPlaying)
		{
			input.UI.Enable();
			input.Player.Disable();

			ui.gameObject.SetActive(true);
			StartCoroutine(NextFrame());

		}
		else
		{
			input.UI.Disable();
			input.Player.Enable();
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
