using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.FilterWindow;

public class PlayerController : MonoBehaviour
{

	private float playerSpeed = 5.0f;
	private float jumpHeight = 1.5f;
	private float gravityValue = -9.81f;
	[SerializeField]
	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private InputManager inputmanager;

	[Header("Input Actions")]
	public InputActionReference moveAction; // expects Vector2
	public InputActionReference jumpAction; // expects Button
	
	private void Awake()
	{
		controller = gameObject.GetComponent<CharacterController>();
		inputmanager = InputManager.Instance;
	}

	private void OnEnable()
	{
		moveAction.action.Enable();
		jumpAction.action.Enable();
	}

	private void OnDisable()
	{
		moveAction.action.Disable();
		jumpAction.action.Disable();
	}

	void Update()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		// Read input
		Vector2 moveMent = inputmanager.GetPlayerMovement();
		Vector3 move = new Vector3(moveMent.x, 0,moveMent.y);
		move = Vector3.ClampMagnitude(move, 1f);

		if (move != Vector3.zero)
		{
			transform.forward = move;
		}

		// Jump
		if (inputmanager.PlayerJumpedThisFrame() && groundedPlayer)
		{
			playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
		}

		// Apply gravity
		playerVelocity.y += gravityValue * Time.deltaTime;

		// Combine horizontal and vertical movement
		Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
		controller.Move(finalMove * Time.deltaTime);
	}
}

