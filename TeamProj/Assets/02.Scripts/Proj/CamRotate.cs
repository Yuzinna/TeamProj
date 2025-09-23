using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CamRotate : MonoBehaviour
{
	public InputAction rotateAction;
	public Vector2 mousePos;
	private void OnEnable()
	{
		rotateAction.Enable();
		rotateAction.performed += Rotate;
	}
	private void OnDisable()
	{
		
	}
	// Update is called once per frame
	void Update()
    {
        
    }
	void Rotate(InputAction.CallbackContext value)
	{
		mousePos=rotateAction.ReadValue<Vector2>();
	}
}
