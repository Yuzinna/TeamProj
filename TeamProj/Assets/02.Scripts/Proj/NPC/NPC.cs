using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가

/// <summary>
/// DOTween을 사용하여 NPC의 이동 및 상태를 관리합니다.
/// </summary>
public class NPC : MonoBehaviour
{
	// NPC의 정보 (이름, 국적 등)
	public NpcData npcData;

	// 이동할 목표 지점들 (Unity 에디터에서 할당)
	public Transform deskPosition;
	public Transform exitPosition;
	public Transform rejectedPosition; // 거절 시 이동할 위치

	// 이동 속도
	public float moveSpeed = 2.0f;

	// NPC의 현재 상태
	public enum State
	{
		Waiting,      // 생성 후 대기
		MovingToDesk, // 책상으로 이동 중
		AtDesk,       // 책상에 도착하여 심사 대기
		Approved,     // 승인됨
		Rejected      // 거절됨
	}
	public State currentState;

	void Start()
	{
		currentState = State.Waiting;
		if (npcData != null)
		{
			gameObject.name = $"NPC_{npcData.npcName}";
		}
	}

	/// <summary>
	/// NPC를 심사대로 이동시킵니다.
	/// </summary>
	public void GoToDesk()
	{
		if (deskPosition == null)
		{
			Debug.LogError("Desk Position이 할당되지 않았습니다!");
			return;
		}

		currentState = State.MovingToDesk;
		float distance = Vector3.Distance(transform.position, deskPosition.position);
		float duration = distance / moveSpeed;

		// DOMove를 사용하여 목표 지점까지 이동하고, 도착하면 상태를 변경합니다.
		transform.DOMove(deskPosition.position, duration)
			.SetEase(Ease.Linear) // 일정한 속도로 이동
			.OnComplete(() =>
			{
				currentState = State.AtDesk;
				Debug.Log($"{gameObject.name}이(가) 심사대에 도착했습니다.");
				// 여기에 도착 후 방향을 바라보게 하는 코드 추가 가능
				// transform.DORotate(deskPosition.rotation.eulerAngles, 0.5f);
			});
	}

	/// <summary>
	/// 심사 결과를 처리하고, 결과에 따라 이동시킵니다.
	/// </summary>
	public void ProcessDecision(bool isApproved)
	{
		if (isApproved)
		{
			GoToExit();
		}
		else
		{
			GoToRejected();
		}
	}

	private void GoToExit()
	{
		if (exitPosition == null)
		{
			Debug.LogError("Exit Position이 할당되지 않았습니다!");
			return;
		}

		currentState = State.Approved;
		float distance = Vector3.Distance(transform.position, exitPosition.position);
		float duration = distance / moveSpeed;

		transform.DOMove(exitPosition.position, duration)
			.SetEase(Ease.Linear)
			.OnComplete(() =>
			{
				Debug.Log($"{gameObject.name}이(가) 퇴장했습니다.");
				Destroy(gameObject); // 퇴장 후 오브젝트 파괴
			});
	}

	private void GoToRejected()
	{
		if (rejectedPosition == null)
		{
			Debug.LogError("Rejected Position이 할당되지 않았습니다!");
			return;
		}
		currentState = State.Rejected;
		float distance = Vector3.Distance(transform.position, rejectedPosition.position);
		float duration = distance / moveSpeed;

		transform.DOMove(rejectedPosition.position, duration)
			.SetEase(Ease.Linear)
			.OnComplete(() =>
			{
				Debug.Log($"{gameObject.name}이(가) 거절되어 퇴장했습니다.");
				Destroy(gameObject); // 퇴장 후 오브젝트 파괴
			});
	}
}

/*
// [OLD CODE - Spline Version]

using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
public enum eNpcState { Idle, WalkToDesk, WalkToEnter, WalkToExit,WalkWithPolice }//npc가 가질수있는 행동
public enum eSubmit {Passport,Luggage,None }//npc가 제출할 아이템

public class NPC : BaseEntity
{

    private State[] states;
    [SerializeField]
    public State currentState;

    public SplineAnimate splineAnimate;
	public SplineContainer curSpline;
    public Animator anim;
	public GameObject passportPrefab;//손에 들고있을 여권
	public GameObject luggagePrefab;//손에 들고있을 수하물
	
	public GameObject heldPassport;//현재 들고있는 여권 오브젝트
	public GameObject heldLuggage;//현재 들고있는 수하물 오브젝트

	public GameObject passportUsePrefab;//플레이어가 사용할 여권
	public GameObject LuggageUsePrefab;//플레이어가 사용할 수하물

	

	public bool isBlocked = false;
	public bool isFinSpline = false;

	public bool isSubmit = false;//제출해야하는 상태인가
	public bool wasSubmitPassport = false;//여권을 제출했는가
	public bool wasSubmitLuggage = false;//수하물을 제출했는가
	public eSubmit curSubmit = eSubmit.None;
	#region Passport Transform prop
	public Vector3 passportPositionOffset;
	public Vector3 passportRotationOffset;
	public Vector3 passportScaleOverride = Vector3.one;
	#endregion

	#region Luggage Transform prop
	public Vector3 luggagePositionOffset;
	public Vector3 luggageRotationOffset;
	public Vector3 luggageScaleOverride = Vector3.one;
	public bool isRot = false;
	public float RotAngle = 60f;
	#endregion



	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
        anim = GetComponent<Animator>();
		splineAnimate = GetComponent<SplineAnimate>();


	}
	void Start()
    {

		#region passportInitTr
		passportPositionOffset = new Vector3(-0.07f, -0.06f, 0);
		passportRotationOffset = new Vector3(0, -180f, 90f);
		passportScaleOverride = new Vector3(0.13f, 0.05f, 0.02f);
		#endregion
		#region LuggageInitTr
		luggagePositionOffset = new Vector3(0.2f,-0.3f,0.35f);
		 luggageRotationOffset= new Vector3(-70f,-10f,30f);
		 luggageScaleOverride = new Vector3(0.3f,0.3f,0.3f);
	

	#endregion

		splineAnimate.Container = curSpline;
		ChangeState(eNpcState.WalkToDesk);

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	
	public override void Setup(string name)
	{
		base.Setup(name);

        gameObject.name = $"id : {Id} {name}";
        states = new State[5];
        states[0] = new NpcAllStats.Idle();
        states[0].stateName = "Idle";
		states[0].state = eNpcState.Idle;
        states[1] = new NpcAllStats.WalkToDesk();
		states[1].stateName = "WalkToDesk";
		states[1].state = eNpcState.WalkToDesk;
		states[2] = new NpcAllStats.WalkToEnter();
		states[2].stateName = "WalkToEnter";
		states[2].state = eNpcState.WalkToEnter;
		states[3] = new NpcAllStats.WalkToExit();
		states[3].stateName = "WalkToExit";
		states[3].state = eNpcState.WalkToExit;
		states[4] = new NpcAllStats.WalkWithPolice();
		states[4].stateName = "WalkWithPolice";
		states[4].state = eNpcState.WalkWithPolice;
		curSpline = SplineManager.Instance.splineList[0];
		splineAnimate.Container = curSpline;
		ChangeState(eNpcState.Idle);
	}
	public override void updated()
	{
		if(currentState!= null)
        {
            currentState.UpdateState(this);
           // Debug.Log($"curState {currentState.stateName}");
        }
	}
    public void ChangeState(eNpcState newState)
    {
        if (states[(int)newState] == null)
        {
            return;
        }
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = states[(int)newState];
        currentState.Enter(this);
    }
	private void OnTriggerEnter(Collider other)
	{
		//뒷사람과의 충돌체크
		//충돌한 npc의 id
		if (other.CompareTag("Npc"))//다른 충돌한게 npc라면 일단
		{
			int otherNpcId = other.gameObject.GetComponent<NPC>().Id;//충돌한 npc의 id
			if (Id>otherNpcId)//내 id가 충돌한 애보다 더 크면? 충돌한 애가 뒷사람
			{
				isBlocked = true;
			}
		}
			
		
	}
	private void OnTriggerExit(Collider other)
	{
		//뒷사람과의 충돌체크
		//충돌한 npc의 id
		if (other.CompareTag("Npc"))//다른 충돌한게 npc라면 일단
		{
			int otherNpcId = other.gameObject.GetComponent<NPC>().Id;//충돌한 npc의 id
			if (Id > otherNpcId)//내 id가 충돌한 애보다 더 크면? 충돌한 애가 뒷사람
			{
				isBlocked = false;
			}
		}
	}

	#region Passport Transform Function
	public void AttachPassportToHand()
	{
		// Humanoid 뼈대 찾기
		Transform handBone = anim.GetBoneTransform(HumanBodyBones.RightHand);

		if (handBone == null)
		{
			Debug.LogError("손 뼈대를 찾을 수 없습니다!");
			return;
		}

		if (heldPassport != null)
			Destroy(heldPassport);
		// 여권 프리팹을 손 뼈대의 자식으로 생성
		heldPassport = Instantiate(passportPrefab, handBone);
		
		// 기본 위치를 부모 오브젝트에 대한 로컬 위치로 초기화
		heldPassport.transform.localPosition = Vector3.zero;
		heldPassport.transform.localRotation = Quaternion.identity;

		// 필요하면 offset 적용 (예: 여권의 중심 맞추기)
		heldPassport.transform.localPosition = new Vector3(0, 0.05f, 0.1f);

		heldPassport.transform.localPosition = passportPositionOffset;
		heldPassport.transform.localEulerAngles = passportRotationOffset;
		heldPassport.transform.localScale = passportScaleOverride;
	}
	public void Passport(GameObject obj)
	{
		AttachPassportToHand();
	}
	public void TransferPassport()
	{
		if (heldPassport != null)
		{
			Destroy(heldPassport);
			heldPassport = null;
		}
		Transform tr=GameObject.Find("SpawnPassport").transform;

		if (RuleManager.Instance.UsePassport)
		{
			Destroy(RuleManager.Instance.UsePassport);
			RuleManager.Instance.UsePassport = null;
		}
		GameObject obj = Instantiate(passportUsePrefab, tr.position, tr.rotation);
		RuleManager.Instance.UsePassport = obj;
		RuleManager.Instance.UsePassport.GetComponent<PassportData>().SetOwner(this);
		
	}
	#endregion
	public void AttachLuggageToHand()
	{
		// Humanoid 뼈대 찾기
		Transform handBone = anim.GetBoneTransform(HumanBodyBones.RightHand);

		if (handBone == null)
		{
			Debug.LogError("손 뼈대를 찾을 수 없습니다!");
			return;
		}

		if (heldLuggage != null)
			Destroy(heldLuggage);
		// 수하물 프리팹을 손 뼈대의 자식으로 생성
		heldLuggage = Instantiate(luggagePrefab, handBone);

		// 기본 위치를 부모 오브젝트에 대한 로컬 위치로 초기화
		heldLuggage.transform.localPosition = Vector3.zero;
		heldLuggage.transform.localRotation = Quaternion.identity;

		// 필요하면 offset 적용 (예: 여권의 중심 맞추기)
		heldLuggage.transform.localPosition = new Vector3(0, 0.05f, 0.1f);

		heldLuggage.transform.localPosition = luggagePositionOffset;
		heldLuggage.transform.localEulerAngles = luggageRotationOffset;
		heldLuggage.transform.localScale = luggageScaleOverride;
	}
	public void Luggage()
	{
		AttachLuggageToHand();
	}
	public void TransferLuggage()
	{
		if (heldLuggage != null)
		{
			Destroy(heldLuggage);
			heldLuggage = null;
		}
		Transform tr = GameObject.Find("SpawnLuggage").transform;
		

		if (RuleManager.Instance.UseLuggage)
		{
			Destroy(RuleManager.Instance.UseLuggage);
			RuleManager.Instance.UseLuggage = null;
		}
		RuleManager.Instance.UseLuggage = Instantiate(LuggageUsePrefab, tr.position, tr.rotation);
		RuleManager.Instance.UseLuggage.GetComponent<LuggageData>().SetOwner(this);

	}
}
*/