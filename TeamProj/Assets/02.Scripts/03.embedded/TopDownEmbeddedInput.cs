using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownEmbeddedInput : BaseInput
{
    public InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
	private void OnEnable()
	{
		moveAction.Enable();
		moveAction.performed += Move;
		moveAction.canceled += StopMove;
	}
	private void OnDisable()
	{
		moveAction.Disable();
		moveAction.performed -= Move;
		moveAction.canceled -= StopMove;
	}
	// Update is called once per frame
	void Update()
    {
        
    }
    void Move(InputAction.CallbackContext value)
    {
       dir = moveAction.ReadValue<Vector2>();
    }
    void StopMove(InputAction.CallbackContext value)
    {
        dir = Vector2.zero;
    }
}
