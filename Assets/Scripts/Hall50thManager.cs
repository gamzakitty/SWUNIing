using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hall50thManager : MonoBehaviour
{
	public List<Hall50thData> events;                // ���� ���������� ��ȭ �� �̺�Ʈ ����Ʈ
	
	public TextMeshProUGUI characterNameText;        // ĳ���� �̸��� ǥ���� UI �ؽ�Ʈ (TextMeshPro)
	public TextMeshProUGUI contentText;              // ��ȭ�� ���� ������ ǥ���� UI �ؽ�Ʈ (TextMeshPro)
	public TextMeshProUGUI titleText;                // �˾��� ���� �ؽ�Ʈ
	public TextMeshProUGUI popupContentText;         // �˾��� ���� �ؽ�Ʈ
	public Button[] choiceButtons;                   // ������ ��ư �迭
	public TMP_InputField answerInputField;          // ���� ���� �Է� �ʵ� (TextMeshPro)
	public GameObject quizPanel;                     // ���� �˾� �г�
	public Image quizImage;                          // ���� ȭ�鿡���� �̹���
	public Image popupImage;                         // �˾������� �̹���
	public Button checkButton;                       // ���� Ȯ�� ��ư
	public Button okButton;                          // �˾������� Ȯ�� ��ư
	public Button cancelButton;                      // �˾������� ��� ��ư

	//public GameObject messageWindow;                // �޽��� â ������Ʈ
	//public GameObject messageCardPrefab;            // �޽��� ī�� ������
	//public Transform messageContentParent;          // �޽��� ī�尡 �߰��� �θ� ��ü
	//public TMP_InputField responseInputField;       // �÷��̾��� �Է� �ʵ�
	//public Button responseButton;                   // �Է� Ȯ�� ��ư
	//private string characterNameHeader;

	private int currentEventIndex = 0;               // ���� ��ȭ �Ǵ� �̺�Ʈ �ε���

	private void Start()
	{
		checkButton.gameObject.SetActive(false);     // Ȯ�� ��ư ��Ȱ��ȭ
		okButton.gameObject.SetActive(false);        // OK ��ư ��Ȱ��ȭ
		popupImage.enabled = false;                  // �˾� �̹��� ��Ȱ��ȭ
		quizPanel.SetActive(false);                  // ���� �˾� ��Ȱ��ȭ
		quizImage.enabled = false;                   // ���� �̹��� ��Ȱ��ȭ
		StartDialogue();                             // ��ȭ ����

		// �ؽ�Ʈ ���� ����
		contentText.alignment = TextAlignmentOptions.Left;
		contentText.enableWordWrapping = true;       // �� �ٲ� ����
		contentText.overflowMode = TextOverflowModes.Overflow; // �ؽ�Ʈ ��ħ ����
	}

	// ��ȭ ����
	public void StartDialogue()
	{
		currentEventIndex = 0;
		ShowEvent(currentEventIndex);               // ù ��° ��ȭ ǥ��
	}

	// �̺�Ʈ ǥ��
	public void ShowEvent(int eventIndex)
	{
		Hall50thData eventData = events[eventIndex]; // Hall50thData ���

		// �̺�Ʈ �̹��� ����
		quizImage.sprite = eventData.eventImage;
		quizImage.enabled = eventData.eventImage != null;

		switch (eventData.eventType)
		{
			case EventType.Dialogue:
				ShowDialogue(eventData);
				break;

			case EventType.Quiz:
				ShowQuiz(eventData);
				break;
			
				// �ʿ��� ��� �ٸ� �̺�Ʈ ���� �߰� ����
		}
	}

	// ��ȭ �̺�Ʈ ó��
	private void ShowDialogue(Hall50thData eventData)
	{
		characterNameText.text = eventData.characterName;
		contentText.text = eventData.content;         // �ؽ�Ʈ�� �� ���� ����

		// �ؽ�Ʈ ���뿡 ���� RectTransform ũ�� ����
		AdjustTextSize();

		// ������ ��ư ����
		for (int i = 0; i < choiceButtons.Length; i++)
		{
			if (i < eventData.choices.Count && i < eventData.nextEventIds.Count)
			{
				choiceButtons[i].gameObject.SetActive(true);  // ��ư�� �ٽ� Ȱ��ȭ
				choiceButtons[i].image.enabled = true;
				choiceButtons[i].interactable = true;
				choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = eventData.choices[i];

				int nextEventId = eventData.nextEventIds[i];  // nextEventIds�� Hall50thData���� ������
				choiceButtons[i].onClick.RemoveAllListeners();
				choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextEventId));
			}
			else
			{
				choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
				choiceButtons[i].image.enabled = false;
				choiceButtons[i].interactable = false;
				choiceButtons[i].gameObject.SetActive(true);
			}
		}

		checkButton.gameObject.SetActive(false);     // ���� Ȯ�� ��ư ����
	}

	// ���� �̺�Ʈ ó��
	private void ShowQuiz(Hall50thData eventData)
	{
		// ���� ���� �� �̹��� ����
		contentText.text = eventData.content;
		answerInputField.text = "";                  // �Է� �ʵ� �ʱ�ȭ
		quizImage.sprite = eventData.eventImage;     // ���� �̹��� ����
		quizImage.enabled = eventData.eventImage != null;

		// ���� �˾� ���� UI ����
		quizPanel.SetActive(false);                  // �˾� ��Ȱ��ȭ
		checkButton.gameObject.SetActive(true);      // Ȯ�� ��ư Ȱ��ȭ

		// ������ ��ư ��Ȱ��ȭ
		foreach (Button button in choiceButtons)
		{
			button.gameObject.SetActive(false);
		}

		// Ȯ�� ��ư Ŭ�� �� �˾� ǥ��
		checkButton.onClick.RemoveAllListeners();
		checkButton.onClick.AddListener(() => ShowQuizPopup(eventData));
	}

	// ���� �˾� ǥ��
	private void ShowQuizPopup(Hall50thData eventData)
	{
		quizPanel.SetActive(true);                    // �˾� Ȱ��ȭ

		// �˾��� ǥ���� �ؽ�Ʈ ����
		popupContentText.text = eventData.popupContent; // Quiz �̺�Ʈ�� �˾� ���� ���

		popupImage.sprite = eventData.eventImage;      // �˾� �̹��� ����
		popupImage.enabled = eventData.eventImage != null;
		answerInputField.text = "";                    // �Է� �ʱ�ȭ
		okButton.gameObject.SetActive(true);           // OK ��ư Ȱ��ȭ
		cancelButton.gameObject.SetActive(true);       // Cancel ��ư Ȱ��ȭ

		// OK ��ư Ŭ�� �� ���� Ȯ��
		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(() =>
		{
			string answer = answerInputField.text;

			// ���� Ȯ��
			if (answer.Equals(eventData.correctAnswer, System.StringComparison.OrdinalIgnoreCase))
			{
				quizPanel.SetActive(false);           // �˾� ��Ȱ��ȭ
				MoveToNextEvent();                    // ���� �̺�Ʈ�� �̵�
			}
			else
			{
				answerInputField.text = "";           // ������ ��� �Է� �ʵ� �ʱ�ȭ
			}
		});

		// Cancel ��ư Ŭ�� �� �˾� ��Ȱ��ȭ
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(() =>
		{
			quizPanel.SetActive(false);               // �˾� ��Ȱ��ȭ
		});
	}


	// �ؽ�Ʈ ũ�⿡ ���� RectTransform ����
	private void AdjustTextSize()
	{
		RectTransform rectTransform = contentText.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(contentText.preferredWidth, contentText.preferredHeight);
	}

	// ���� �̺�Ʈ�� �̵�
	private void MoveToNextEvent()
	{
		quizPanel.SetActive(false);
		checkButton.gameObject.SetActive(false);
		popupImage.enabled = false;                // ���� �̺�Ʈ�� �Ѿ �� �˾� �̹��� ��Ȱ��ȭ
		quizImage.enabled = false;                 // ���� �̺�Ʈ�� �Ѿ �� ���� �̹��� ��Ȱ��ȭ

		currentEventIndex++;
		ShowEvent(currentEventIndex);

		// ���� �̺�Ʈ�� ���̾�α��� ��� choiceButton�� �ٽ� Ȱ��ȭ
		if (events[currentEventIndex].eventType == EventType.Dialogue)
		{
			foreach (var button in choiceButtons)
			{
				button.gameObject.SetActive(true);  // ���̾�α׷� �Ѿ �� ��ư Ȱ��ȭ
			}
		}
	}

	// ������ ���� �� ȣ��
	private void OnChoiceSelected(int nextEventId)
	{
		currentEventIndex = nextEventId;
		ShowEvent(currentEventIndex);
	}

	private string GetLastSentence(string content)
	{
		var sentences = content.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
		return sentences.Length > 0 ? sentences[^1].Trim() : content;
	}

	// ���� �̺�Ʈ id ��ȯ
	public Hall50thData GetCurrentEventData()
	{
		return events[currentEventIndex];
	}

}
