using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public InputGenerateCSharp input;
	private static InputManager _instance;

	public static InputManager Instance
	{
		get { return _instance; }
		
	}
	private _3DControl _3DControl;

	private void Awake()
	{
		_3DControl = new _3DControl();
		input = GetComponent<InputGenerateCSharp>();
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
		_3DControl.Player.Enable();
	}
	private void OnDisable()
	{
		_3DControl.Disable();
	}
	public Vector2 GetPlayerMovement()
	{
		input.dir= _3DControl.Player.Move.ReadValue<Vector2>();
		Debug.Log($"{input.dir.x},{input.dir.y}");
		return input.move;
	}
	public Vector2 GetMouseDelta()
	{
		input.look =_3DControl.Player.Look.ReadValue<Vector2>();
		Debug.Log($"{input.look.x},{input.look.y}");
		return input.look; 
	}
	public bool PlayerJumpedThisFrame()
	{
		input.jump = _3DControl.Player.Jump.triggered;
		return input.jump;
	}

}
