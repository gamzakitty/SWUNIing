using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hall50thData
{
	public EventType eventType;                  // 이벤트 유형

	[Header("공통 필드")]
	public string characterName;                 // 캐릭터 이름

	[TextArea(1, 4)]
	public string content;                       // 본문 내용

	public Sprite eventImage;                    // 이벤트 이미지

	public List<int> nextEventIds = new();       // 다음 이벤트 ID 목록

	[Header("Dialogue 이벤트 전용")]
	[Tooltip("Dialogue 이벤트일 경우 입력할 선택지 목록")]
	public List<string> choices = new();         // 선택지 목록 (Dialogue 전용)

	[Header("Quiz 이벤트 전용")]
	[Tooltip("Quiz 이벤트일 경우 표시할 팝업 내용")]
	public string quizPopupContent;              // 팝업 콘텐츠 (Quiz 전용)

	[Tooltip("Quiz 이벤트일 경우 입력할 정답")]
	public string correctAnswer;                 // 퀴즈 정답 (Quiz 전용)

	[Tooltip("퀴즈에 대한 힌트 (있을 경우)")]
	public string hintText;                      // 이벤트에 대한 힌트

	[Header("AR 이벤트 전용")]
	public string ARPopupContent;                // 팝업 콘텐츠 (AR 전용)

	//[Header("Message 이벤트 전용")]
	//public List<string> messageSequence = new(); // 메시지 순서
	//public List<string> highlightedWords = new(); // 강조할 단어 목록
	//public Dictionary<string, int> responseNextEventMap = new(); // 응답에 따른 다음 이벤트 매핑
}

public enum EventType
{
	Dialogue,
	Quiz,
	AR,
	Message
}
