using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputInvokeCSharpEvents : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    public bool analogMovement;
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public PlayerInput PlayerInput;
    private InputActionMap playerMap;
    private InputActionMap carMap;
    private InputActionMap changerMap;

    private bool onDrive;
        
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions["Move"];
        lookAction = PlayerInput.actions["Look"];
        jumpAction = PlayerInput.actions["Jump"];
        sprintAction = PlayerInput.actions["Sprint"];

        //moveAction = playerMap.FindAction("Move");
        //lookAction = playerMap.FindAction("Look");
        //jumpAction = playerMap.FindAction("Jump");
        //sprintAction = playerMap.FindAction("Sprint");
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        lookAction.performed += OnLook;
        jumpAction.performed += OnJump;
        sprintAction.performed += OnSprint;
        sprintAction.canceled += (InputAction.CallbackContext value) => { sprint = false; };

        moveAction.canceled += StopMove;
        lookAction.canceled += (InputAction.CallbackContext value) => { look = Vector2.zero; };

        carMap = PlayerInput.actions.FindActionMap("Car");
        carMap.FindAction("Move").performed += OnMove;
        carMap.FindAction("Move").canceled += StopMove;
        carMap.FindAction("Look").performed += OnLook;
        carMap.FindAction("Look").canceled += (InputAction.CallbackContext value) => { look = Vector2.zero; };

        changerMap = PlayerInput.actions.FindActionMap("Changer");
        changerMap.FindAction("Change").started += OnChange;
        changerMap.Enable();
    }

    public void OnChange(InputAction.CallbackContext value)
    {
        onDrive = !onDrive;
        if (onDrive)
        {
            PlayerInput.SwitchCurrentActionMap("Car");
        }
        else
        {
            PlayerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }

    public void StopMove(InputAction.CallbackContext value)
    {
        move = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        look = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        jump = value.performed;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        sprint = value.performed;
    }
}
