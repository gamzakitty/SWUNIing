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
	public string content;                       // 이벤트 내용

	public Sprite eventImage;                    // 이벤트 이미지
	public List<int> nextEventIds = new();       // 다음 이벤트 ID 목록

	[Header("Dialogue 이벤트 전용")]
	[Tooltip("Dialogue 이벤트일 경우 입력할 선택지 목록")]
	public List<string> choices = new();         // 선택지 목록 (Dialogue 전용)

	[Header("Quiz 이벤트 전용")]
	[Tooltip("Quiz 이벤트일 경우 표시할 팝업 내용")]
	public string popupContent;                  // 팝업 콘텐츠 (Quiz 전용)

	[Tooltip("Quiz 이벤트일 경우 입력할 정답")]
	public string correctAnswer;                 // 퀴즈 정답 (Quiz 전용)
}

public enum EventType
{
	Dialogue,
	Quiz,
	AR
}
