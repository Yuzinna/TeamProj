using UnityEngine;

public class LegacyInput : BaseInput
{
	private void Update()
	{
		dir = Vector3.zero;
		if (Input.GetKey(KeyCode.A))
		{
			dir.x = -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			dir.x = 1;
		}

		if (Input.GetKey(KeyCode.W))
		{
			dir.y = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			dir.y = -1;
		}

	}
}
