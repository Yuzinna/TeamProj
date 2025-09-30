using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerClick : MonoBehaviour
{
    InputManager inputManager;
	public GameObject CamRoot;
	public GameObject particle;

	Vector2 mousepos;

	Vector3 mousePosWorld;
	Ray ray;
	private void Start()
	{
		//input 초기화
		inputManager = InputManager.Instance;
		inputManager.input.input.Player.Click.performed += OnClick;

		mousepos = inputManager.input.CurrentMouse.position.ReadValue();

		
		ray = Camera.main.ScreenPointToRay(mousepos);
	}
	private void Update()
	{
		//레이캐스트 확인용 레이저
		ray = Camera.main.ScreenPointToRay(mousepos);
		Debug.DrawRay(ray.origin,ray.direction * 10f, Color.red);	

	}
	public void OnClick(InputAction.CallbackContext value)
	{

		mousepos= inputManager.input.CurrentMouse.position.ReadValue();
		
		

		Ray ray = Camera.main.ScreenPointToRay(mousepos);
		RaycastHit hitinfo = new RaycastHit();
		if(Physics.Raycast(ray,out hitinfo,5000f,7))
		{
			Debug.Log("success");
			Instantiate(particle,hitinfo.point,Quaternion.identity);
		}
	}

}
