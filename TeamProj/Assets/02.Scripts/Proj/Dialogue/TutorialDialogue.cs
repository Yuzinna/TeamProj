using UnityEngine;

public class TutorialDialogue : Dialogue
{

	public GameObject checklistText;
	public GameObject passportObject;
	public GameObject luggageScaleText;
	public GameObject blackButton;
	public GameObject greenButton;
	public GameObject yellowButton;
	public GameObject blueButton;
	public GameObject redButton;

	
	private void Awake()
	{
		Lines = new DialogueLine[]
	   {
			new DialogueLine("지윤",
				"안녕, 후배. 내가 네 업무를 알려줄 차지윤 선배야. 만나서 반가워.\n" +
				"이제 곧 진짜 손님들이 오실 텐데, 그 전에 내가 먼저 전체적인 업무 과정을 설명해 줄게."),

			new DialogueLine("지윤",
				"우리 일은 기본적으로 화면에 뜨는 [업무 체크리스트]를 따라서 진행하면 돼. 오른쪽의 화면을 눌러서 체크리스트를 확인해봐!")
			{
				 highlightObject = checklistText,   // 강조할 UI
			},

            // --- 여권 확인 단계 ---
            new DialogueLine("지윤",
				"첫 번째 체크리스트는 '여권 확인'이야. 손님이 데스크에 오면 여권을 받아야 해. 앞에 있는 파란색 버튼을 눌러서 여권을 받아봐")
			{
				highlightObject = blueButton
			},

			new DialogueLine("지윤",
				"여권을 받으면, 시스템 정보랑 실물이랑 다른 곳이 없는지 꼼꼼하게 대조해야 해. 특히 '사진', '이름', '만료 기간'은 꼭 확인해야 해.")
			{
				highlightObject = passportObject
			}
			,
            // --- 수화물 무게 확인 ---
            new DialogueLine("지윤",
				"두 번째는 '수화물 검사' 수화물을 컨베이어 벨트로 올려달라고 하기위해 노란 버튼을 클릭해봐!")
			{
				highlightObject = yellowButton
			},
            // --- X-ray 검사 ---
            new DialogueLine("지윤",
				"우리가 검사해야할 수화물 정보는 무게와, 금지물품의 포함여부야.. 무게가 초과하거나 금지물품이 포함되어있으면 무조건 거절이야")
			{
				highlightObject = luggageScaleText
			},
            // --- 거절 상황 ---
            new DialogueLine("지윤",
				"문제가 없는 경우엔 '통과' 버튼을 눌러서 가게 하면 되고,")
			{
				highlightObject = greenButton
			},// --- 거절 상황 ---
            new DialogueLine("지윤",
				"문제가 있는 경우엔 '거절' 버튼을 눌러서 가지 못하게해야 해.")
			{
				highlightObject = redButton
			},

            // --- 보안 호출 버튼 ---
            new DialogueLine("지윤",
				"힘들 땐 절대 혼자 해결하려고 하지 말고, [검정색 버튼]을 눌러.")
			{
				highlightObject = blackButton
			},

            // --- 김민준 등장 ---
            new DialogueLine("민준",
				"헉... 호출 받고 왔습니다! 무슨 일... 어? 아무 일 없으신가요?"),
			new DialogueLine("지윤",
				"아, 민준 씨. 놀라게 해서 미안해요. 이쪽은 오늘부터 일하게 된 후배예요."),
			new DialogueLine("민준",
				"안녕하세요! 보안팀 김민준입니다! 예를 들어 위험물품을 가져오거나 한 손님이 오거나하는 위험한 상황이 생기면 바로 눌러주세요!"),
			new DialogueLine("지윤",
				"봤지? 저 친구가 민준 씨야. 든든하지? 이제 진짜 첫 손님을 맞아볼까?")
	   };
	}

}
