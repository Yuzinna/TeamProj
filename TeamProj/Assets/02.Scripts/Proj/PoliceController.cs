using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using System.Threading.Tasks;
public class PoliceController : MonoBehaviour
{
	Animator anim;
	public Transform SpwanTr;
	SplineAnimate splineAnim;
	public bool isFinSpline = false;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		splineAnim = GetComponent<SplineAnimate>();
		transform.position = SpwanTr.position;
		transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
	}
	private void Start()
	{
		anim.SetBool("IsWalk", true);
		transform.position = SpwanTr.position;
		gameObject.SetActive(false);

	}
	private void OnEnable()
	{
		transform.position = SpwanTr.position;
		transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
	}
	private void OnDisable()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
	}
	private void Update()
	{

		//스플라인이 끝까지 가면
		if (splineAnim.ElapsedTime >= splineAnim.Duration&&!isFinSpline)
		{
			//그러면 초기화
			isFinSpline = true;
			transform.position = SpwanTr.position;
			
			gameObject.SetActive(false);
		}
	}
	public void  CallPolice()
	{
		
		anim.SetBool("IsWalk", true);
		Vector3 targetPosition = new Vector3(4.5f, 0f, -36f);
		// 2초 동안 이동 (시간은 자유롭게 조정 가능)
		transform.DOMove(targetPosition, 2f)
			.SetEase(Ease.Linear).OnComplete(() =>{
				// 이동이 끝난 뒤 회전 애니메이션 idle
				anim.SetBool("IsWalk", false);
				transform.DORotate(new Vector3(0, -153, 0), 0.5f, RotateMode.Fast).OnComplete(() => 
				{
					splineAnim.Restart(true);
					
					splineAnim.Play();
					isFinSpline = false;
					anim.SetBool("IsWalk", true);
				});
			});
		//await Task.Delay(2500);
		//splineAnim.Play();
		//anim.SetBool("IsWalk", true);

	}
	public void  CallPoliceTuto()
	{
		
		anim.SetBool("IsWalk", true);
		Vector3 targetPosition = new Vector3(4.5f, 0f, -36f);
		// 2초 동안 이동 (시간은 자유롭게 조정 가능)
		transform.DOMove(targetPosition, 2f)
			.SetEase(Ease.Linear).OnComplete(() =>{
				// 이동이 끝난 뒤 회전 애니메이션 idle
				anim.SetBool("IsWalk", false);
				transform.DORotate(new Vector3(0, -153, 0), 0.5f, RotateMode.Fast).OnComplete(() => 
				{
				
				});
			});
		
	}
}
