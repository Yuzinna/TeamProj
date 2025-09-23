using UnityEngine;
using UnityEngine.Events;

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
	private void Start()
	{
		input.jumpEvent += Jump;
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
	void Jump()
	{
		if (!input.canJump)
			return;
		input.canJump = false;
		rb2d.AddForce(Vector2.up* 4, ForceMode2D.Impulse);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 6)
			input.canJump = true;
	}
}
