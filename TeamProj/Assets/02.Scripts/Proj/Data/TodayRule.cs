using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
public struct GameDate
{
	public int DateDay;
	public int DateMon;
	public int DateYear;
}
public class TodayRule : MonoBehaviour
{

	RuleManager ruleManager;
	[Header("오늘의 금지 물품 (영문 ID)")]
	public Dictionary<string,string> todayBannedItems = new Dictionary<string,string>();
	public List<string> todayBannedKeys = new List<string>();
	public Dictionary<string,string> todayCountrys = new Dictionary<string,string>();
	public List<string> todayCountyKeys = new List<string>();


	//오늘무게의 최대값
	public float todayMaxweight;

	//오늘 날짜들
	
	public GameDate date;
	//
	private void Start()
	{
		ruleManager = RuleManager.Instance;
		todayMaxweight = Random.Range(ruleManager.maxWeight / 2, ruleManager.maxWeight);//최대 무게 초기화

		for (int i = 0; i < ruleManager.itemBanedNames.Count; i++)//금지물품 초기화
		{
			int ran = Random.Range(0, 2);
			if (ran == 1)
			{
				todayBannedKeys.Add(ruleManager.itemBanedkeys[i]);
				todayBannedItems.Add(ruleManager.itemBanedkeys[i], ruleManager.itemBanedNames[ruleManager.itemBanedkeys[i]]);
			}
		}
		for (int i = 0; i < ruleManager.CountryNames.Count; i++)//여행나라 초기화
		{
			int ran = Random.Range(0, 2);
			if (ran == 1)
			{
				todayCountyKeys.Add(ruleManager.Countykeys[i]);
				todayCountrys.Add(ruleManager.Countykeys[i], ruleManager.CountryNames[ruleManager.Countykeys[i]]);
			}
			//만약에 끝까지 갔는데도 아무것도 안채워졌다면..
			if(i== ruleManager.CountryNames.Count&& todayCountyKeys.Count==0)
			{
				int ran2 = Random.Range(0, ruleManager.CountryNames.Count-1);
				todayCountyKeys.Add(ruleManager.Countykeys[ran2]);
				todayCountrys.Add(ruleManager.Countykeys[ran2], ruleManager.CountryNames[ruleManager.Countykeys[ran2]]);


			}
		}



		date.DateDay = 01;
		date.DateMon = 04;
		date.DateYear = 2025;
	}
	private void OnEnable()
	{
		
	}
	public string GetDisplayText()
	{
		string totalText="";
		string DateText = $"오늘 날짜는 {date.DateYear}년{date.DateMon}월{date.DateDay}일\n";
		string weightText = $"수하물의 최대 무게는 : {todayMaxweight}kg\n";
		string todayBannedItemsText = $"오늘의 금지물품 : ";
		string todayCountyText = $"오늘 갈수있는 나라: ";
		foreach (var item in todayBannedItems)
		{
			todayBannedItemsText += item.Value +", ";
		}
		todayBannedItemsText += "\n";
		foreach (var item in todayCountrys)
		{
			todayCountyText += item.Value +", ";
		}
		todayCountyText += "\n--------------------";
		

		totalText+= DateText + weightText + todayBannedItemsText+ todayCountyText;

		return totalText;
		}
}
