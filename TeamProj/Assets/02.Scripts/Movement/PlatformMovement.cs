using UnityEngine;

public class PlatformMovement : BaseMovement
{
    Rigidbody2D rb2d;
	SpriteRenderer sr;
	protected override void Awake()
	{
		base.Awake();
		rb2d = GetComponent<Rigidbody2D>();
		sr = GetComponentInChildren<SpriteRenderer>();
	}
	public override void Move()
	{
		base.Move();
		rb2d.AddForce(input.dir * speed);
	}
	public override void Anim()
	{
		base.Anim();
		
		sr.flipX = input.dir.x < 0;
	}
}
