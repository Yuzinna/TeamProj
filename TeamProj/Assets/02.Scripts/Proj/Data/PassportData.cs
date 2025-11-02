using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassportData : MonoBehaviour
{
    //갈나라 
    //여권의 유효기간 
    //이름
    List<string> names = new List<string>()
    {
        "김XX",
        "이XX",
        "박XX",
        "최XX",
        "남궁XX",
        "신XX"
    };
    
    string npcName;    //이름
    public GameDate dueDate;   //기한 날짜
    public string countryName; //나라 이름 
    [SerializeField]
    NPC owner; //여권의 주인

	public TextMeshPro PassportText;

	public GameDate DueDate { get => dueDate; set => dueDate = value; }
	public string CountryName { get => countryName; set => countryName = value; }

	//public NPC Owner { get => Owner; set => Owner = value; }

	private void Start()
	{
        int ran = Random.Range(0, names.Count);
		npcName = names[ran];

        dueDate.DateYear = Random.Range(2022, 2030);
        dueDate.DateMon  = Random.Range(1, 13);
        dueDate.DateDay = Random.Range(1, 30);
        ran = Random.Range(0, RuleManager.Instance.Countykeys.Count);

		countryName = RuleManager.Instance.Countykeys[ran];

        Setup();
	}
    public void SetOwner(NPC npc)
    {
        owner = npc;

	}
    public NPC GetOwner()
    {
        return owner;
    }
    public void Setup()
    {
        PassportText.text = TextInit();
        
	}
    public string TextInit()
    {
        string totalText = "";

        string NpcnameText = $"이름 : {npcName}\n";
        string dueDateText = $"여권 기한 : {dueDate.DateYear}년 {dueDate.DateMon}월 {dueDate.DateDay}일 까지\n";

        string CountryNameText = $"목적지 {countryName}\n";
        

        totalText = NpcnameText + dueDateText + CountryNameText;

        return totalText;
        

    }
}
