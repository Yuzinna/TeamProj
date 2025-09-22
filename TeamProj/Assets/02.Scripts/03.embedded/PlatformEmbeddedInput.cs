using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformEmbeddedInput : BaseInput
{
    public InputAction moveAction;

	private void Start()
	{
		//moveAction.started;//누르기 시작할때
		//moveAction.performed;//누르는중
		//moveAction.canceled;//땔때
		moveAction.performed += Move;
	}
	void Move(InputAction.CallbackContext value)
	{

	}
}
