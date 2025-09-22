using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    
    protected Animator animator;
    protected BaseInput input;


	protected virtual void Awake()
	{
        input = GetComponent<BaseInput>();
        animator = GetComponent<Animator>();
	}
	protected virtual void Update()
	{
        Anim();
        Move();
	}
	public virtual void Anim()
    {

    }
    
    public virtual void Move()
    {

    }

}
