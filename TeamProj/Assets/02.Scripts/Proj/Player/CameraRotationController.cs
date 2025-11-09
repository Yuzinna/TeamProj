using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotationController : MonoBehaviour
{
	public Transform cameraRoot;

	private Quaternion initialRotation;
	private Quaternion startRotation;
	private Quaternion targetRotation;

	private float rotationDuration = 1.5f;
	private float rotationElapsed = 0f;
	private bool isRotating = false;

	private void Awake()
	{
		if (cameraRoot != null)
		{
			initialRotation = cameraRoot.localRotation;
			targetRotation = initialRotation;
		}
	}

	private void Update()
	{
		UpdateRotation();
	}

	private void OnEnable()
	{
		InputManager.OnInputReady += BindInput;
		if (InputManager.Instance?.input?.inputAsset != null)
			BindInput();
	}

	private void OnDisable()
	{
		
	}

	private void StartRotationToWorldY(float worldYAngle)
	{
		if (cameraRoot == null || isRotating) return;

		Quaternion currentRotation = cameraRoot.rotation;
		Quaternion desiredRotation = Quaternion.Euler(0, worldYAngle, 0);

		// 현재 회전과 목표 회전이 거의 같으면 무시
		if (Quaternion.Angle(currentRotation, desiredRotation) < 1f)
			return;

		startRotation = cameraRoot.rotation;
		targetRotation = desiredRotation;
		rotationElapsed = 0f;
		isRotating = true;
	}

	private void UpdateRotation()
	{
		if (!isRotating || cameraRoot == null) return;

		rotationElapsed += Time.deltaTime;
		float linearT = Mathf.Clamp01(rotationElapsed / rotationDuration);
		float easedT = Mathf.SmoothStep(0f, 1f, linearT);

		cameraRoot.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

		if (linearT >= 1f)
		{
			isRotating = false;
		}
	}

	private void OnCameraRotationLeft(InputAction.CallbackContext context)
	{
		StartRotationToWorldY(-90f); // 왼쪽
	}

	private void OnCameraRotationRight(InputAction.CallbackContext context)
	{
		StartRotationToWorldY(90f); // 오른쪽
	}

	private void OnCameraRotationBack(InputAction.CallbackContext context)
	{
		StartRotationToWorldY(180f); // 뒤
	}

	private void OnCameraRotationReset(InputAction.CallbackContext context)
	{
		StartRotationToWorldY(0f); // 앞
	}
	private void BindInput()
	{
		var input = InputManager.Instance.input.inputAsset.CameraCtrl;
		input.Enable();
		input.CameraRotationLeft.performed += OnCameraRotationLeft;
		input.CameraRotationRight.performed += OnCameraRotationRight;
		input.CameraRotationReset.performed += OnCameraRotationReset;
		input.CameraRotationBack.performed += OnCameraRotationBack;
	}

	private void UnbindInput()
	{
		
	}
}
