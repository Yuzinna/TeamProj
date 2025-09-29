using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Cinemachine;
using System.Collections.Generic;
public class CamRotate : Base3DInput, Unity.Cinemachine.IInputAxisOwner
{
	public CinemachinePanTilt pantilt;
	private InputActionReference lookAction;

	

	float mx = 0;
	float my = 0;
	private void OnEnable()
	{
		
		
	}
	private void OnDisable()
	{
		
	}
	private void Awake()
	{
		
		
	}
	// Update is called once per frame
	void Update()
    {
	 }

	public void GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
	{
		throw new System.NotImplementedException();
	}
}
