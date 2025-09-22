using UnityEngine;

public class TopDownMovement : BaseMovement
{
	 Rigidbody2D rb2d;
	protected override void Awake()
	{
		base.Awake();
		rb2d = GetComponent<Rigidbody2D>();
	}
	public override void Move()
	{
		base.Move();
		rb2d.linearVelocity = speed * input.dir;
	}
	public override void Anim()
	{
		base.Anim();
		animator.SetFloat("X", input.dir.x);
		animator.SetFloat("Y", input.dir.y);
	}
}
