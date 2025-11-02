using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
public enum eNpcState { Idle, WalkToDesk, WalkToEnter, WalkToExit,WalkWithPolice }//npc가 할수있는 행동
public enum eSubmit {Passport,Luggage,None }//npc가 제출할 오브젝트

public class NPC : BaseEntity
{

    private State[] states;
    [SerializeField]
    public State currentState;

    public SplineAnimate splineAnimate;
	public SplineContainer curSpline;
    public Animator anim;
	public GameObject passportPrefab;//손에생성할 여권
	public GameObject luggagePrefab;//손에생성할 수하물
	
	public GameObject heldPassport;//진짜 손에 생성한 여권
	public GameObject heldLuggage;//진짜 손에생성한 수하물

	public GameObject passportUsePrefab;//사용할 여권
	public GameObject LuggageUsePrefab;//사용할 수하물

	

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
		//앞사람과의 충돌체크
		//충돌한 npc의 id
		if (other.CompareTag("Npc"))//일단 충돌한게 npc여야만 하고
		{
			int otherNpcId = other.gameObject.GetComponent<NPC>().Id;//충돌한 npc의 id
			if (Id>otherNpcId)//내 id가 상대보다 더 크면? 충돌한건 앞사람
			{
				isBlocked = true;
			}
		}
			
		
	}
	private void OnTriggerExit(Collider other)
	{
		//앞사람과의 충돌체크
		//충돌한 npc의 id
		if (other.CompareTag("Npc"))//일단 충돌한게 npc여야만 하고
		{
			int otherNpcId = other.gameObject.GetComponent<NPC>().Id;//충돌한 npc의 id
			if (Id > otherNpcId)//내 id가 상대보다 더 크면? 충돌한건 앞사람
			{
				isBlocked = false;
			}
		}
	}

	#region Passport Transform Function
	public void AttachPassportToHand()
	{
		// Humanoid 리그 기준
		Transform handBone = anim.GetBoneTransform(HumanBodyBones.RightHand);

		if (handBone == null)
		{
			Debug.LogError("손 본을 찾을 수 없습니다!");
			return;
		}

		if (heldPassport != null)
			Destroy(heldPassport);
		// 프리팹을 손 위치에 생성
		heldPassport = Instantiate(passportPrefab, handBone);
		
		// 손의 위치를 기준으로 오브젝트의 상대 위치 초기화
		heldPassport.transform.localPosition = Vector3.zero;
		heldPassport.transform.localRotation = Quaternion.identity;

		// 필요하면 offset 조정 (예: 손바닥 중심 보정)
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
		// Humanoid 리그 기준
		Transform handBone = anim.GetBoneTransform(HumanBodyBones.RightHand);

		if (handBone == null)
		{
			Debug.LogError("손 본을 찾을 수 없습니다!");
			return;
		}

		if (heldLuggage != null)
			Destroy(heldLuggage);
		// 프리팹을 손 위치에 생성
		heldLuggage = Instantiate(luggagePrefab, handBone);

		// 손의 위치를 기준으로 오브젝트의 상대 위치 초기화
		heldLuggage.transform.localPosition = Vector3.zero;
		heldLuggage.transform.localRotation = Quaternion.identity;

		// 필요하면 offset 조정 (예: 손바닥 중심 보정)
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
