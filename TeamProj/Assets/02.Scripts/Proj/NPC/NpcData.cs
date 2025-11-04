using UnityEngine;

/// <summary>
/// NPC(승객)의 심사에 필요한 데이터를 담는 Scriptable Object입니다.
/// </summary>
[CreateAssetMenu(fileName = "NewNpcData", menuName = "Scriptable Objects/NpcData")]
public class NpcData : ScriptableObject
{
    [Header("Passenger Information")]
    public string npcName = "John Doe";
    public Sprite photo; // 승객 사진
    public string dateOfBirth = "1990-01-01";
    public string country = "Republic of Kolechia";
    public string passportID = "ID123456";
    public string passportExpiryDate = "2028-12-31";

    [Header("Rule Violation Flags")]
    public bool hasForgedPassport = false; // 위조된 여권 소지 여부
    public bool hasContraband = false;   // 밀수품 소지 여부

    // 여기에 더 많은 데이터 (예: 입국 목적, 수하물 정보 등)를 추가할 수 있습니다.
}
