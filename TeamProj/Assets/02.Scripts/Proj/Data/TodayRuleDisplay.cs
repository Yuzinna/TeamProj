using TMPro;
using UnityEngine;

public class TodayRuleDisplay : MonoBehaviour
{
    public TodayRule todayRule;

    public TextMeshPro monitorText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		
	}
    public void Setup()
    {
		if (todayRule == null || monitorText == null)
		{
			Debug.LogWarning("TodayRule 또는 TextMeshPro가 연결되지 않았습니다.");
			return;
		}

		monitorText.text = todayRule.GetDisplayText();
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
