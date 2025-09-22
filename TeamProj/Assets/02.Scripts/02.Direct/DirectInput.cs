using UnityEngine;
using UnityEngine.InputSystem;
public class DirectInput : BaseInput
{
	//���̷�Ʈ ����� input system
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
		//keyboard.wKey.wasPressedThisFrame;// ���� ����
		//keyboard.wKey.isPressed;//  ������ ��
		//keyboard.wKey.wasReleasedThisFrame;// �� ��
		//keyboard.dKey.ReadValue()//Ű�� ������ 1, �ƴϸ� 0
		float horizontal = keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
		float vertical = keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();

		dir.x = horizontal;
		dir.y = vertical;

		dir.Normalize();

	}
}
