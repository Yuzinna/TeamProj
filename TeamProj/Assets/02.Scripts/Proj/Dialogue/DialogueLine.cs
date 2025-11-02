using System;
using UnityEngine;


[System.Serializable]
public class DialogueLine
{
	public string speaker;  // 화자 이름
	[TextArea(2, 5)]
	public string line;     // 대사 내용
	public GameObject highlightObject; // 강조할 UI 또는 오브젝트
	public Action onLineStart;         // 대사 시작 시 실행
	public Action onLineEnd;           // 대사 끝나면 실행
	public DialogueLine(string speaker, string line, GameObject highlightObject = null, Action onLineStart = null,Action onLineEnd = null)
	{
		this.speaker = speaker;
		this.line = line;
		this.highlightObject = highlightObject;
		this.onLineStart = onLineStart;
		this.onLineEnd = onLineEnd;
	}
}
