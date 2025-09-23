using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformPlayerInput : BaseInput
{
    PlayerInput pI;

	InputActionMap playerMap;
	InputAction moveAction;
	InputAction jumpAction;
	InputAction dashAction;
	private void Awake()
	{
        pI = GetComponent<PlayerInput>();
	}
	private void OnEnable()
	{
		pI.actions.FindActionMap("Player");
		moveAction = pI.actions.FindAction("Move");
		jumpAction = pI.actions.FindAction("Jump");
		dashAction = pI.actions.FindAction("Dash");

		moveAction.performed += OnMove;
		jumpAction.started += OnJump;
		jumpAction.started += (InputAction.CallbackContext value)=> { Debug.Log("started!"); };
		jumpAction.performed+= (InputAction.CallbackContext value)=> { Debug.Log("performed!"); };
		jumpAction.canceled += (InputAction.CallbackContext value)=> { Debug.Log("cancled!"); };
		moveAction.canceled += StopMove;
		dashAction.performed += Ondash;
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
	public void Ondash(InputAction.CallbackContext value)
	{
		Debug.Log("대쉬 발동!");
	}

}
