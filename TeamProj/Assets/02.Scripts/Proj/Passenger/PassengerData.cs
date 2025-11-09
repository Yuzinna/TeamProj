using UnityEngine;
using System;

// [CreateAssetMenu] 속성을 사용하면 Unity 에디터의 Assets/Create 메뉴에서 이 클래스의 인스턴스(에셋)를 직접 생성할 수 있습니다.
// 이를 통해 다양한 유형의 승객 데이터 템플릿을 미리 만들어 둘 수 있습니다.
[CreateAssetMenu(fileName = "NewPassengerData", menuName = "Passenger/Passenger Data")]
// ScriptableObject는 MonoBehaviour와 달리 게임 오브젝트에 붙일 필요가 없는, 데이터 컨테이너 역할을 하는 클래스입니다.
// 승객의 이름, 여권 정보 등 순수 데이터를 저장하기에 적합합니다.
public class PassengerData : ScriptableObject
{
	// public으로 선언된 필드들은 Unity 에디터의 인스펙터 창에서 값을 설정할 수 있습니다.
	public string passengerName;
    public bool hasForgedDocuments; // This can be refactored later into more specific forged flags
    public DateTime passportExpirationDate;

    // New fields for passport details
    public string passportNumber;
    public string nationality;
    public Texture2D photoTexture; // For the passenger's photo on the passport
    public Texture2D signatureTexture; // For the signature on the passport
    public DateTime issueDate;

    // New flags for specific forged conditions
    public bool isSignatureForged;
    public bool isPhotoBackgroundWrong;

   
}
