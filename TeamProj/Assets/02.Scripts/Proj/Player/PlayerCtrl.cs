using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditorInternal.ReorderableList;

public class PlayerCtrl : MonoBehaviour
{
	public Vector2 look;
	InputManager inputManager;


	[Header("Movement")]
	[SerializeField] private float moveSpeed = 2f; // 자동 이동 속도
	[SerializeField] private Transform targetTransform;
	private bool isWalking = false;
	private Action onReachedTarget;

	[Header("Head Bobbing")]
	[SerializeField] Transform cameraRoot;

	[SerializeField] private float bobSpeed = 8f;       // 흔드는 속도
	[SerializeField] private float bobAmount = 3f;   // 흔드는 크기
	private float defaultY;
	private float bobTimer = 0f;

	private void Awake()
	{
		defaultY = cameraRoot.localPosition.y;
		//cameraTransform.localPosition.y;
	}
	private void Start()
	{
		
		inputManager = InputManager.Instance;
		inputManager.input.inputAsset.CameraCtrl.CameraRot.performed += OnLook;
		inputManager.input.inputAsset.CameraCtrl.CameraRot.canceled += (InputAction.CallbackContext value) => { look = Vector2.zero; };
		

	}
	private void Update()
	{
		HandleAutoWalk();
	}
	private void HandleAutoWalk()
	{
		if (!isWalking || targetTransform == null) return;

		// 목표까지 이동
		transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);

		if (isWalking)
		{
			bobTimer += Time.deltaTime * bobSpeed;
			float newY = defaultY + Mathf.Sin(bobTimer) * bobAmount;
			cameraRoot.localPosition = new Vector3(cameraRoot.localPosition.x, newY, cameraRoot.localPosition.z);
		}
		else
		{
			cameraRoot.localPosition = new Vector3(cameraRoot.localPosition.x, defaultY, cameraRoot.localPosition.z);
		}
		// 도착 체크
		if (Vector3.Distance(transform.position, targetTransform.position) < 0.05f)
		{
			isWalking = false;
			bobTimer = 0f;
			if (cameraRoot.transform != null)
				cameraRoot.transform.localPosition = new Vector3(cameraRoot.transform.localPosition.x, defaultY, cameraRoot.transform.localPosition.z);

			onReachedTarget?.Invoke(); // 도착 시 콜백 호출
		}
	}
	public void StartWalkToDesk(Action onComplete = null)
	{
		//targetPosition = target;
		onReachedTarget = onComplete;
		isWalking = true;
	}
	public void OnLook(InputAction.CallbackContext value)
	{
		look = value.ReadValue<Vector2>();
	}
	
}
