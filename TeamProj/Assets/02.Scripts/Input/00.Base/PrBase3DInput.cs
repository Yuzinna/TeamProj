using UnityEngine;
using UnityEngine.InputSystem;

public class PrBase3DInput : PrBaseInput
{
	[Header("Character Input Values")]
	public Vector2 move { get { return dir; } set { dir = value; } }
	public Vector2 look;
	public Vector2 screenMousePos;
	public Mouse CurrentMouse;
	public bool jump;
	public bool sprint;

	public bool analogMovement;
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;


}
