using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public enum eCheckType { Enter, Exit, Police}

public struct Item
{
	public string name;
	public float weight;
}
public class RuleManager : MonoBehaviour
{
	public static RuleManager Instance { get; private set; }

	//검출에 사용할 여권
	public GameObject UsePassport { get => usePassport; set => usePassport = value; }

	//검출에 사용할 수하물
	public GameObject UseLuggage { get => useLuggage; set => useLuggage = value; }

	[Header("수하물 최대 허용 무게 (kg)")]
	public float maxWeight = 90f;

	//public GameObject gameControllerObj;
	//GameManager gameController;
	[Header("아이템 한글 이름 매핑")]//특수아이템 목록
	public Dictionary<string, string> itemBanedNames = new Dictionary<string, string>()
	{
		
		{"lighter", "라이터"},
		{"knife", "칼"},
		{"alcohol", "술"},
		{"drone", "드론"},
		{"drug", "정체불명 약"},
		
	};


	public List<string> itemBanedkeys = new List<string>
	{
		"lighter",
		"knife",
		"alcohol",
		"drone",
		"drug"
	};
	//일반아이템 목록
	public Dictionary<string, string> itemNormalNames = new Dictionary<string, string>()
	{
	{"document"     ,"서류" },
	{"laptop"       ,"노트북" },
	{"camera"       ,"카메라" },
	{"shoes"        ,"신발" },
	{"charger"      , "충전기"},
	{"magazine"     ,"잡지" },
	{"umbrella"     ,"우산" },
	};
	public List<string> itemNormalkeys = new List<string>
	{
	"document",
	"laptop",
	"camera",
	"shoes",
	"charger",
	"magazine",
	"umbrella"
	};

	//갈수있는 나라 목록
	public Dictionary<string, string> CountryNames = new Dictionary<string, string>()
	{
		{"America", "미국"},
		{"Ghana", "가나"},
		{"Japan", "일본"},
		{"Canada", "캐나다"},
		{"France", "프랑스"},
	};
	public List<string> Countykeys = new List<string>
	{
		"America",
		"Ghana",
		"Japan",
		"Canada",
		"France",
	};

	public TextMeshPro scoreText;


	[SerializeField] private GameObject usePassport;
	[SerializeField] private GameObject useLuggage;
	private TodayRule todayRule;
	static int goodScore = 0;
	static int badScore = 0;
	public static int curNpc ;
	private void Awake()
	{
		// 싱글톤 패턴
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		todayRule = GetComponent<TodayRule>();
		
		//gameController = gameControllerObj.GetComponent<GameManager>(); 싱글톤으로 구조 바꿈
		//curNpc = gameController.maxNpcCount;
	}
	private void Start()
	{
		curNpc = 20;
		displayScoreText();
	}
	private void Update()
	{
		if (curNpc == 0)
		{
			scoreText.text = $"\n\n오늘 일정 종료";
		}	
	}
	//통과 시키거나 거부시킬때의 체킹
	public void DetectScore(eCheckType type)
	{
		//통과상태일때 체크
		//여권: 목적지 나라 ,여권 만료 가한, 
		//수하물: 무게 , 금지물품

		//거부상태일때 체크
		//경찰호출상태일때 체크
		bool isweight = CompareWeight();
		bool isitem = CompareItem();
		bool iscountry = CompareCountry();
		bool isDate = CompareDate();
		
		switch (type)
		{
			//통과 상태가 맞는거면 무게는 낮아야하고 , 목적지도 존재, 여권기한이 오늘보다 큼, 금지물품 없어야함
			case eCheckType.Enter:
				if(isweight==false &&iscountry == true&&isDate==true && isitem==false)
				{
					goodScore += 1;
				}
				else
				{
					badScore += 1;
				}

				break;
			//거절상태라면 무게는 높거나 목적지 존재하지 않거나 여권기한이 오늘보다 작음, 금지물품은 없어야함
			case eCheckType.Exit:
				if (isweight == true || iscountry == false || isDate == false && isitem == false)
				{
					goodScore += 1;
				}
				else
				{
					badScore += 1;
				}

				break;

			//경찰호출상태라면 무게는 높거나 목적지 존재하지 않거나 여권은 만료하지 않거나는 상관없고 금지물품이 있어야함
			case eCheckType.Police:
				if ( isitem == true)
				{
					goodScore += 1;
				}
				else
				{
					badScore += 1;
				}
					break;
			default:
				break;
		}
	}

	//리턴값은 오늘갈수있는 나라와 여권이 겹치면 true
	public bool CompareCountry()
	{
		//isCountry 오늘의 갈수있는 나라가 여권에 적힌 갈나라에 존재하는가
		bool isCountry = false;
		PassportData curPassport = usePassport.GetComponent<PassportData>();
		string curCounty = curPassport.CountryName;
		foreach (var item in todayRule.todayCountyKeys)
		{
			if (curCounty == item)
			{
				isCountry = true;
			}
		}

		return isCountry;

	}
	//리턴값은 여권기한이 오늘날짜보다 크면 true
	public bool CompareDate()
	{
		PassportData curPassport = usePassport.GetComponent<PassportData>();
		GameDate PassportDate = curPassport.DueDate;
		GameDate todayDate = todayRule.date;

		//isdate는 여권기한이 오늘날짜보다 크면 true
		bool isDate = true;
		//여권 만료 년도가 커야함
		if (PassportDate.DateYear > todayDate.DateYear)
		{
			isDate = true;
			return isDate;
		}
		if (PassportDate.DateMon > todayDate.DateMon)
		{
			isDate = true;
			return isDate;
		}
		if (PassportDate.DateDay > todayDate.DateDay)
		{
			isDate = true;
			return isDate;
		}

		return false;
	}
	//현재 수하물이 무게가 오늘 지정한 무게보다 크면 true
	public bool CompareWeight()
	{
		//isWeight는 현재 수하물이 무게가 오늘 지정한 무게보다 큰가
		bool isWeight = false;
		LuggageData curLuggage = useLuggage.GetComponent<LuggageData>();

		//현재 수하물 무게가 더 크면 
		if(curLuggage.weight > todayRule.todayMaxweight)
		{
			//수하물이 초과함
			isWeight = true;
		}
		else
		{
			//수하물이 정상임
			isWeight = false;
		}
		return isWeight;
	}

	//오늘의 금지아이템들과 수하물 내부의 아이템이 존재하면 참
	public bool CompareItem()
	{
		//isitem은 금지아이템이 있는가?
		bool isitem = false;
		//오늘의 금지아이템들과 수하물 내부의 아이템을 비교함
		LuggageData curLuggage =useLuggage.GetComponent<LuggageData>();
		//현재 수하물의 아이템들을
		foreach (var item in curLuggage.itemKeys)//키여서 "bag" 과 같이 영어로 나옴
		{
			//모든 키에대해서 모든 금지아이템을 비교함
			for (int i = 0; i < todayRule.todayBannedKeys.Count; i++)
			{
				if(item == todayRule.todayBannedKeys[i])
				{
					//금지아이템 존재
					isitem = true;
				}
			}
		}
		//금지아이템 없음
		return isitem;
	}

	public void displayScoreText()
	{
		string totalText = $"성공 : {goodScore} \n실패: {badScore} \n남은 승객: {curNpc}";
		scoreText.text = totalText;
	}
}

