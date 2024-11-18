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
	public string content;                       // ���� ����

	public Sprite eventImage;                    // �̺�Ʈ �̹���

	public List<int> nextEventIds = new();       // ���� �̺�Ʈ ID ���

	[Header("Dialogue �̺�Ʈ ����")]
	[Tooltip("Dialogue �̺�Ʈ�� ��� �Է��� ������ ���")]
	public List<string> choices = new();         // ������ ��� (Dialogue ����)

	[Header("Quiz �̺�Ʈ ����")]
	[Tooltip("Quiz �̺�Ʈ�� ��� ǥ���� �˾� ����")]
	public string quizPopupContent;              // �˾� ������ (Quiz ����)

	[Tooltip("Quiz �̺�Ʈ�� ��� �Է��� ����")]
	public string correctAnswer;                 // ���� ���� (Quiz ����)

	[Tooltip("��� ���� ��Ʈ (���� ���)")]
	public string hintText;                      // �̺�Ʈ�� ���� ��Ʈ

	[Header("AR �̺�Ʈ ����")]
	public string ARPopupContent;                // �˾� ������ (AR ����)

	//[Header("Message �̺�Ʈ ����")]
	//public List<string> messageSequence = new(); // �޽��� ����
	//public List<string> highlightedWords = new(); // ������ �ܾ� ���
	//public Dictionary<string, int> responseNextEventMap = new(); // ���信 ���� ���� �̺�Ʈ ����
}

public enum EventType
{
	Dialogue,
	Quiz,
	AR,
	Message
}
