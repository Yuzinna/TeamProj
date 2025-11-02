using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum item { Knife,cash,}
public class LuggageData : MonoBehaviour
{
    public float weight;
	
	public Dictionary<string, string> Items = new Dictionary<string, string>();
	public List<string> itemKeys = new List<string>();//금지아이템 + 일반 아이템 키
	public TextMeshPro dectitemsText;// 금지아이템 + 일반아이템
	public string totalText;
	NPC owner;
	public TodayRule todayRule;
	private void Awake()
	{
		todayRule = RuleManager.Instance.gameObject.GetComponent<TodayRule>();
	}
	private void Start()
	{
		weight = Random.Range(10f, RuleManager.Instance.maxWeight);
		
		int count = todayRule.todayBannedKeys.Count + RuleManager.Instance.itemNormalNames.Count;//오늘의 금지아이템 + 일반템
		
		for (int i = 0; i < count; i++)//금지물품 초기화
		{

			//카운트가 금지물품의 수보다 작으면
			if(i< todayRule.todayBannedItems.Count)
			{
				int ran = Random.Range(0, count);
				if (ran == 0)
				{
					itemKeys.Add(todayRule.todayBannedKeys[i]);
					Items.Add(todayRule.todayBannedKeys[i], todayRule.todayBannedItems[todayRule.todayBannedKeys[i]]);
				}
			}
			if (i >= todayRule.todayBannedItems.Count)
			{
				int ran = Random.Range(0,3);
				if (ran == 0)
				{
					itemKeys.Add(RuleManager.Instance.itemNormalkeys[i- todayRule.todayBannedItems.Count]);
					Items.Add(RuleManager.Instance.itemNormalkeys[i- todayRule.todayBannedItems.Count], RuleManager.Instance.itemNormalNames[RuleManager.Instance.itemNormalkeys[i- todayRule.todayBannedItems.Count]]);
				}
			}


		}
		dectitemsText = GameObject.Find("detectInfo").GetComponent<TextMeshPro>() ;
	}
	public void SetOwner(NPC npc)
	{
		owner = npc;

	}
	public NPC GetOwner()
	{
		return owner;
	}
	public void InitDisplayText()
	{
		
		string ItemsText = $"\n\n탐지된 물품 : ";
		string weightText = $"\n수하물 무게 : {weight}";
		foreach (var item in Items)
		{
			ItemsText += $"{item.Value}, ";
		}
		totalText += ItemsText +"기타물품.."+"\n"+ weightText;
		dectitemsText.text= totalText;
	}
}
