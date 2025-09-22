using UnityEngine;
using UnityEngine.Events;

public class BaseInput : MonoBehaviour
{
	public Vector3 dir;
	protected UnityAction _jumpEvent;
	public event UnityAction jumpEvent
	{
		add
		{
			_jumpEvent += value;
		}
		remove
		{
			_jumpEvent -= value;
		}
	}
	public bool canJump;
}
