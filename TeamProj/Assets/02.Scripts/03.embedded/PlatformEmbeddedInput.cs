using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformEmbeddedInput : BaseInput
{
    public InputAction moveAction;
	public InputAction jumpAction;

	private void OnEnable()
	{
		//moveAction.started;//������ �����Ҷ�
		//moveAction.performed;//��������
		//moveAction.canceled;//����
		//inputaction�� ����Ѵٰ� ��������� �����������

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
