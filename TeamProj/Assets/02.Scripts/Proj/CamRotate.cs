using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Cinemachine;
public class CamRotate : Base3DInput
{
	public CinemachinePanTilt pantilt;
	private InputActionReference lookAction;
	private float sensitivity;

	[SerializeField] private float minTilt = -80f;
	[SerializeField] private float maxTilt = 80f;
	public float rotspeed = 200f;

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
		pantilt.PanAxis.Value += look.x * sensitivity;

		pantilt.TiltAxis.Value -= look.y * sensitivity;
		pantilt.TiltAxis.Value = Mathf.Clamp(pantilt.TiltAxis.Value, minTilt, maxTilt);
    }
	
	


}
