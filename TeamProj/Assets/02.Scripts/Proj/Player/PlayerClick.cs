using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public enum eCommandNpc {SubmitPassport, SubmitLuggage, WalkToEnter, WalkToExit,WalkWithPolice }
public class PlayerClick : MonoBehaviour
{
    public InputManager inputManager;
	public LayerMask rotatableLayer;
	public TodayRuleDisplay ruleDisplay;

	private bool isRotating = false;
	public float rotationSpeed = 2f;
	public float targetX;
	public float PretargetX;
	public GameObject passport;
	Vector2 mousepos;
	public GameObject first;
	NPC firstNpc;
	Ray ray;
	public GameManager gameController;
	//public bool isEnter = false;
	public bool IsDetect = false;//npc를 찾았나

	private float cooldownTime = 0.5f;//연타방지를 위한 클릭 간격 최소 시간
	private bool canClick = true;//클릭 가능여부
	[SerializeField]
	private float pushCooldownTime=2.4f;
	private bool canPush;
	private bool isCooldownRunning = false;

	public bool isPushYellow = false;
	public bool isPushBlue = false;
	public GameObject police;


	public float pressDistance = 0.2f;  // 버튼 눌리는 거리
	public float pressDuration = 0.1f;  // 내려가는 시간
	public float releaseDuration = 0.1f; // 올라오는 시간

	private Vector3 originalPosition; //버튼의 원래 포지션
	private void Start()
	{
		//input 초기화
		inputManager = InputManager.Instance;
		inputManager.input.inputAsset.CameraCtrl.Click.started+= OnClick;
		//mousepos = inputManager.input.inputAsset.CameraCtrl.CameraRot.ReadValue<Vector2>();
		ray = Camera.main.ScreenPointToRay(mousepos);
		targetX = -10f;
		PretargetX = -90f;
		//플레이어의 레이어를 무시
		rotatableLayer = ~(1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Npc")));
		canPush = true;
	}
	private void Update()
	{
		RotatingPassport();
		//Debug.Log($"canpush : {canPush}");
		if (firstNpc!=null&&firstNpc.gameObject.activeSelf == false)
		{
			firstNpc = null;
			isPushYellow = false;
			isPushBlue = false;
		}
		
	}
	public void RotatingPassport() 
	{
		//레이캐스트 확인용 레이저
		ray = Camera.main.ScreenPointToRay(mousepos);
		Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

		//
		if (isRotating)
		{
			Vector3 curEuler = passport.transform.rotation.eulerAngles;

			float newX = Mathf.LerpAngle(curEuler.x, targetX, Time.deltaTime * rotationSpeed);
			passport.transform.rotation = Quaternion.Euler(newX, curEuler.y, curEuler.z);
			float angleDiff = Mathf.Abs(Mathf.DeltaAngle(curEuler.x, targetX));
			
			if (angleDiff < 0.1f)
			{
				// 거의 도달했으면 정확히 고정
				passport.transform.rotation = Quaternion.Euler(curEuler);

				
				isRotating = false;
			}

		}
	}
	public void OnClick(InputAction.CallbackContext value)
	{
		//인풋에 시작할때만 반응
		if (!value.started) return;

		if (!canClick) return;

		canClick = false;
		
		StartCoroutine(ClickCooldown());
		mousepos = inputManager.input.inputAsset.CameraCtrl.MousePos.ReadValue<Vector2>(); //마우스 좌표 받아오기
		Ray ray = Camera.main.ScreenPointToRay(mousepos);
		
		if (Physics.Raycast(ray, out RaycastHit hit,100f, rotatableLayer))
		{
			if (hit.collider.CompareTag("Passport"))
			{
				Debug.Log("Passport");
				isRotating = true;
				passport = hit.collider.gameObject;

				float temp = targetX;
				targetX = PretargetX;
				PretargetX = temp;
			}
			if (hit.collider.gameObject.layer == 9)
			{
				ruleDisplay = GameObject.Find("RuleManager").GetComponent<TodayRuleDisplay>();
				ruleDisplay.Setup();
			}
			if (IsDetect== true)
			{
				//언제 누를수 있는가 레드버튼을 맨앞에 있던npc의 처리가 완전히 끝날때
				//플레이어가 버튼을 누르자마자 정보의 처리가 일어남, 표시는 npc가 끝에 갔을때
				if (hit.collider.CompareTag("RedSiren")&&RuleManager.Instance.UsePassport&& RuleManager.Instance.UseLuggage)
				{
					if (isPushBlue && isPushYellow)
					{
						Debug.Log("RedSiren");
						//gameController
						gameController.CommandNearestNPC(transform.position, eCommandNpc.WalkToExit);
						RuleManager.Instance.DetectScore(eCheckType.Exit);
						originalPosition = hit.transform.localPosition;
						hit.transform.DOLocalMoveY(originalPosition.y - pressDistance, pressDuration)
						.OnComplete(() =>
						{
							// 다시 올라오는 애니메이션
							hit.transform.DOLocalMoveY(originalPosition.y, releaseDuration);
						});
					}
					
				}
				else if (hit.collider.CompareTag("GreenSiren") && RuleManager.Instance.UsePassport && RuleManager.Instance.UseLuggage)
				{
					if (isPushBlue && isPushYellow)
					{
						Debug.Log("GreenSiren");
						gameController.CommandNearestNPC(transform.position, eCommandNpc.WalkToEnter);
						RuleManager.Instance.DetectScore(eCheckType.Enter);
						originalPosition = hit.transform.localPosition;
						hit.transform.DOLocalMoveY(originalPosition.y - pressDistance, pressDuration)
						.OnComplete(() =>
						{
							// 다시 올라오는 애니메이션
							hit.transform.DOLocalMoveY(originalPosition.y, releaseDuration);
						});

					}
						
				}
				else if (hit.collider.CompareTag("BlackSiren") && RuleManager.Instance.UsePassport && RuleManager.Instance.UseLuggage)
				{

					if (isPushBlue && isPushYellow)
					{
						//police.gameObject.SetActive(true);
						RuleManager.Instance.DetectScore(eCheckType.Police);
						GameObject.Find("PoliceParent").transform.GetChild(0).gameObject.SetActive(true);
						//gameController.CommandNearestNPC(transform.position, eCommandNpc.WalkWithPolice);//일반 
						//gameController.CommandNearestNPC(transform.position, eCommandNpc.WalkWithPolice); // 튜토
						//police.GetComponent<PoliceController>().CallPolice();//일반
						police.GetComponent<PoliceController>().CallPoliceTuto();//튜토
						originalPosition = hit.transform.localPosition;
						hit.transform.DOLocalMoveY(originalPosition.y - pressDistance, pressDuration)
						.OnComplete(() =>
						{
							// 다시 올라오는 애니메이션
							hit.transform.DOLocalMoveY(originalPosition.y, releaseDuration);
						});
					}

				}

				else if ((hit.collider.CompareTag("YellowSiren") || hit.collider.CompareTag("BlueSiren")))
				{
					StartCoroutine(PushCooldown());

					if (hit.collider.CompareTag("YellowSiren")&&canPush)
					{
						canPush = false;
						isPushYellow = true;
						//Debug.Log("YellowSiren");
						gameController.CommandNearestNPC(transform.position, eCommandNpc.SubmitLuggage);
						originalPosition = hit.transform.localPosition;
						hit.transform.DOLocalMoveY(originalPosition.y - pressDistance, pressDuration)
						.OnComplete(() =>
						{
							// 다시 올라오는 애니메이션
							hit.transform.DOLocalMoveY(originalPosition.y, releaseDuration);
						});
					}
					else if(hit.collider.CompareTag("BlueSiren") && canPush)
					{
						canPush = false;
						isPushBlue = true;
						Debug.Log("BlueSiren");
						gameController.CommandNearestNPC(transform.position, eCommandNpc.SubmitPassport);
						originalPosition = hit.transform.localPosition;
						hit.transform.DOLocalMoveY(originalPosition.y - pressDistance, pressDuration)
						.OnComplete(() =>
						{
							// 다시 올라오는 애니메이션
							hit.transform.DOLocalMoveY(originalPosition.y, releaseDuration);
						});
					}

					if (isPushBlue && isPushYellow)
					{
						firstNpc = gameController.NearestNPC(transform.position);
					}
				}



				else if (hit.collider.CompareTag("EraseSiren"))
				{
					//없애야하는조건 만약에 현재 여권의 주인 npc가 눈앞에 없을때 exit이나 enter로 같을때 그러면 완전히 없앰 
					//그렇지 아니하면(주인이 데스크 앞에 있을때) 감추기 or 반응안하기
					NPC nearest=gameController.NearestNPC(transform.position);
					//제일 가까운 npc가 현재 여권의 주인이면 지우는 버튼을 눌러도 반응안함
					NPC	Owner=passport.GetComponent<PassportData>().GetOwner();
					if (nearest == Owner)
					{
						return;
					}
					isRotating = false;
					Debug.Log("EraseSiren");
					Destroy(passport);
					passport = null;
				}
				
			}
			 
		}
		

	}
	private IEnumerator PushCooldown()
	{
		yield return new WaitForSeconds(pushCooldownTime);
		canPush = true;
	}
	private IEnumerator ClickCooldown()
	{
		yield return new WaitForSeconds(cooldownTime);
		canClick = true;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Npc"))
		{
			IsDetect = true;
		}
		if (other.gameObject.CompareTag("Passport"))
		{
			passport = other.gameObject;
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Npc"))
		{
			IsDetect = true;
		}
		if (other.gameObject.CompareTag("Passport"))
		{
			passport = other.gameObject;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Npc"))
		{
			IsDetect = false;
		}
		if (other.gameObject.CompareTag("Passport"))
		{
			//	passport = null;
		}
	}
}
