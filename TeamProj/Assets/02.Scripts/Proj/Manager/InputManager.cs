using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public GenerateCSharpInput input;
	private static InputManager _instance;
	
	public static InputManager Instance
	{
		get { return _instance; }
		
	}
	

	private void Awake()
	{
		//input = new GenerateCSharpInput();
		input = GetComponent<GenerateCSharpInput>();
		if (_instance != null&& _instance!= this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}
	private void OnEnable()
	{
		
	}
	private void OnDisable()
	{
		
	}
	

}
