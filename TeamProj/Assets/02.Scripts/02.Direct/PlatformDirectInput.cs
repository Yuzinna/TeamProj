using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformDirectInput : BaseInput
{
    Keyboard keyboard;

	private void Start()
	{
		keyboard = Keyboard.current;
	}
	private void Update()
	{
		dir = Vector3.zero;
		if (keyboard.aKey.isPressed)
		{
			dir += Vector3.left;
		}
		if(keyboard.dKey.isPressed)
		{
			dir += Vector3.right;
		}
	}
}
