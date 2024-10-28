using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hall50thData
{
	public EventType eventType;                  // �̺�Ʈ ����

	[Header("���� �ʵ�")]
	public string characterName;                 // ĳ���� �̸�

	[TextArea(1, 4)]
	public string content;                       // �̺�Ʈ ����

	public Sprite eventImage;                    // �̺�Ʈ �̹���
	public List<int> nextEventIds = new();       // ���� �̺�Ʈ ID ���

	[Header("Dialogue �̺�Ʈ ����")]
	[Tooltip("Dialogue �̺�Ʈ�� ��� �Է��� ������ ���")]
	public List<string> choices = new();         // ������ ��� (Dialogue ����)

	[Header("Quiz �̺�Ʈ ����")]
	[Tooltip("Quiz �̺�Ʈ�� ��� ǥ���� �˾� ����")]
	public string popupContent;                  // �˾� ������ (Quiz ����)

	[Tooltip("Quiz �̺�Ʈ�� ��� �Է��� ����")]
	public string correctAnswer;                 // ���� ���� (Quiz ����)
}

public enum EventType
{
	Dialogue,
	Quiz,
	AR
}
