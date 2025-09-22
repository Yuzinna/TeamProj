using UnityEngine;
using UnityEngine.InputSystem;
public class DirectInput : BaseInput
{
	//다이렉트 방식의 input system
	Keyboard keyboard;
	Mouse mouse;
	Gamepad gamepad;
	private void Start()
	{
		keyboard = Keyboard.current;
		mouse = Mouse.current;
		gamepad = Gamepad.current;
	}
	private void Update()
	{
		//keyboard.wKey.wasPressedThisFrame;// 누른 순간
		//keyboard.wKey.isPressed;//  누르는 중
		//keyboard.wKey.wasReleasedThisFrame;// 땔 때
		//keyboard.dKey.ReadValue()//키가 눌리면 1, 아니면 0
		float horizontal = keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
		float vertical = keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();

		dir.x = horizontal;
		dir.y = vertical;

		dir.Normalize();

	}
}
