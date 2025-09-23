using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformPlayerInput : BaseInput
{
    PlayerInput pI;

	InputActionMap playerMap;
	InputAction moveAction;
	InputAction jumpAction;
	private void Awake()
	{
        pI = GetComponent<PlayerInput>();
	}
	private void OnEnable()
	{
		pI.actions.FindActionMap("Player");
		moveAction = pI.actions.FindAction("Move");
		jumpAction = pI.actions.FindAction("Jump");

		moveAction.performed += OnMove;
		moveAction.started += OnJump;
		moveAction.canceled += StopMove;
	}
	private void OnDisable()
	{
		
	}
	public void OnMove(InputAction.CallbackContext value)
    {
        dir.x = value.ReadValue<float>();
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        _jumpEvent?.Invoke();
    }
	public void StopMove(InputAction.CallbackContext value)
	{
		dir.x = 0;
	}

}
