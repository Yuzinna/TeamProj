using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenerateCSharpInput : BaseInput
{
	public ProjControl inputAsset;
	

	private void Awake()
	{
		inputAsset = new ProjControl();
	}
	private void OnEnable()
	{
		inputAsset.Enable();
		
	}
	

}
