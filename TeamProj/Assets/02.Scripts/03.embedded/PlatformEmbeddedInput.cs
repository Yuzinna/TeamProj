using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformEmbeddedInput : BaseInput
{
    public InputAction moveAction;

	private void Start()
	{
		//moveAction.started;//������ �����Ҷ�
		//moveAction.performed;//��������
		//moveAction.canceled;//����
		moveAction.performed += Move;
	}
	void Move(InputAction.CallbackContext value)
	{

	}
}
