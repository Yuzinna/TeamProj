using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Base3DInput
{

	private static InputManager _instance;

	public static InputManager Instance
	{
		get { return _instance; }
		
	}
	private _3DControl _3DControl;

	private void Awake()
	{
		_3DControl = new _3DControl();
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
		dir= _3DControl.Player.Move.ReadValue<Vector2>();
		Debug.Log($"{dir.x},{dir.y}");
		return move;
	}
	public Vector2 GetMouseDelta()
	{
		look=_3DControl.Player.Look.ReadValue<Vector2>();
		Debug.Log($"{look.x},{look.y}");
		return look; 
	}
	public bool PlayerJumpedThisFrame()
	{
		jump= _3DControl.Player.Jump.triggered;
		return jump;
	}

}
