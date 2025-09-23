using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformEmbeddedInput : BaseInput
{
    public InputAction moveAction;
	public InputAction jumpAction;

	private void OnEnable()
	{
		//moveAction.started;//누르기 시작할때
		//moveAction.performed;//누르는중
		//moveAction.canceled;//땔때
		//inputaction은 사용한다고 명시적으로 선언해줘야함

		moveAction.Enable();
		moveAction.performed += Move;
		moveAction.canceled += StopMove;

		jumpAction.Enable();
		jumpAction.started += Jump;
	}
	private void OnDisable()
	{
		moveAction.Disable();
		jumpAction.Disable();
		moveAction.performed -= Move;
		moveAction.canceled -= StopMove;
		jumpAction.started -= Jump;
	}
	void Move(InputAction.CallbackContext value)
	{
		dir.x = value.ReadValue<float>();
	}
	void StopMove(InputAction.CallbackContext value)
	{
		dir.x = 0;
	}
	void Jump(InputAction.CallbackContext value)
	{
		_jumpEvent?.Invoke();
	}
}
