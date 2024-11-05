using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hall50thManager : MonoBehaviour
{
	public List<Hall50thData> events;                // 현재 스테이지의 대화 및 이벤트 리스트
	
	public TextMeshProUGUI characterNameText;        // 캐릭터 이름을 표시할 UI 텍스트 (TextMeshPro)
	public TextMeshProUGUI contentText;              // 대화나 문제 내용을 표시할 UI 텍스트 (TextMeshPro)
	public TextMeshProUGUI titleText;                // 팝업의 제목 텍스트
	public TextMeshProUGUI popupContentText;         // 팝업의 내용 텍스트
	public Button[] choiceButtons;                   // 선택지 버튼 배열
	public TMP_InputField answerInputField;          // 퀴즈 정답 입력 필드 (TextMeshPro)
	public GameObject quizPanel;                     // 퀴즈 팝업 패널
	public Image quizImage;                          // 퀴즈 화면에서의 이미지
	public Image popupImage;                         // 팝업에서의 이미지
	public Button checkButton;                       // 퀴즈 확인 버튼
	public Button okButton;                          // 팝업에서의 확인 버튼
	public Button cancelButton;                      // 팝업에서의 취소 버튼

	//public GameObject messageWindow;                // 메시지 창 오브젝트
	//public GameObject messageCardPrefab;            // 메시지 카드 프리팹
	//public Transform messageContentParent;          // 메시지 카드가 추가될 부모 객체
	//public TMP_InputField responseInputField;       // 플레이어의 입력 필드
	//public Button responseButton;                   // 입력 확인 버튼
	//private string characterNameHeader;

	private int currentEventIndex = 0;               // 현재 대화 또는 이벤트 인덱스

	private void Start()
	{
		checkButton.gameObject.SetActive(false);     // 확인 버튼 비활성화
		okButton.gameObject.SetActive(false);        // OK 버튼 비활성화
		popupImage.enabled = false;                  // 팝업 이미지 비활성화
		quizPanel.SetActive(false);                  // 퀴즈 팝업 비활성화
		quizImage.enabled = false;                   // 퀴즈 이미지 비활성화
		StartDialogue();                             // 대화 시작

		// 텍스트 왼쪽 정렬
		contentText.alignment = TextAlignmentOptions.Left;
		contentText.enableWordWrapping = true;       // 줄 바꿈 설정
		contentText.overflowMode = TextOverflowModes.Overflow; // 텍스트 넘침 방지
	}

	// 대화 시작
	public void StartDialogue()
	{
		currentEventIndex = 0;
		ShowEvent(currentEventIndex);               // 첫 번째 대화 표시
	}

	// 이벤트 표시
	public void ShowEvent(int eventIndex)
	{
		Hall50thData eventData = events[eventIndex]; // Hall50thData 사용

		// 이벤트 이미지 설정
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
			
				// 필요한 경우 다른 이벤트 유형 추가 가능
		}
	}

	// 대화 이벤트 처리
	private void ShowDialogue(Hall50thData eventData)
	{
		characterNameText.text = eventData.characterName;
		contentText.text = eventData.content;         // 텍스트를 한 번에 설정

		// 텍스트 내용에 맞춰 RectTransform 크기 조정
		AdjustTextSize();

		// 선택지 버튼 설정
		for (int i = 0; i < choiceButtons.Length; i++)
		{
			if (i < eventData.choices.Count && i < eventData.nextEventIds.Count)
			{
				choiceButtons[i].gameObject.SetActive(true);  // 버튼을 다시 활성화
				choiceButtons[i].image.enabled = true;
				choiceButtons[i].interactable = true;
				choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = eventData.choices[i];

				int nextEventId = eventData.nextEventIds[i];  // nextEventIds를 Hall50thData에서 가져옴
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

		checkButton.gameObject.SetActive(false);     // 퀴즈 확인 버튼 숨김
	}

	// 퀴즈 이벤트 처리
	private void ShowQuiz(Hall50thData eventData)
	{
		// 퀴즈 내용 및 이미지 설정
		contentText.text = eventData.content;
		answerInputField.text = "";                  // 입력 필드 초기화
		quizImage.sprite = eventData.eventImage;     // 퀴즈 이미지 설정
		quizImage.enabled = eventData.eventImage != null;

		// 퀴즈 팝업 관련 UI 설정
		quizPanel.SetActive(false);                  // 팝업 비활성화
		checkButton.gameObject.SetActive(true);      // 확인 버튼 활성화

		// 선택지 버튼 비활성화
		foreach (Button button in choiceButtons)
		{
			button.gameObject.SetActive(false);
		}

		// 확인 버튼 클릭 시 팝업 표시
		checkButton.onClick.RemoveAllListeners();
		checkButton.onClick.AddListener(() => ShowQuizPopup(eventData));
	}

	// 퀴즈 팝업 표시
	private void ShowQuizPopup(Hall50thData eventData)
	{
		quizPanel.SetActive(true);                    // 팝업 활성화

		// 팝업에 표시할 텍스트 설정
		popupContentText.text = eventData.popupContent; // Quiz 이벤트의 팝업 내용 사용

		popupImage.sprite = eventData.eventImage;      // 팝업 이미지 설정
		popupImage.enabled = eventData.eventImage != null;
		answerInputField.text = "";                    // 입력 초기화
		okButton.gameObject.SetActive(true);           // OK 버튼 활성화
		cancelButton.gameObject.SetActive(true);       // Cancel 버튼 활성화

		// OK 버튼 클릭 시 정답 확인
		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(() =>
		{
			string answer = answerInputField.text;

			// 정답 확인
			if (answer.Equals(eventData.correctAnswer, System.StringComparison.OrdinalIgnoreCase))
			{
				quizPanel.SetActive(false);           // 팝업 비활성화
				MoveToNextEvent();                    // 다음 이벤트로 이동
			}
			else
			{
				answerInputField.text = "";           // 오답인 경우 입력 필드 초기화
			}
		});

		// Cancel 버튼 클릭 시 팝업 비활성화
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(() =>
		{
			quizPanel.SetActive(false);               // 팝업 비활성화
		});
	}


	// 텍스트 크기에 맞춰 RectTransform 조정
	private void AdjustTextSize()
	{
		RectTransform rectTransform = contentText.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(contentText.preferredWidth, contentText.preferredHeight);
	}

	// 다음 이벤트로 이동
	private void MoveToNextEvent()
	{
		quizPanel.SetActive(false);
		checkButton.gameObject.SetActive(false);
		popupImage.enabled = false;                // 다음 이벤트로 넘어갈 때 팝업 이미지 비활성화
		quizImage.enabled = false;                 // 다음 이벤트로 넘어갈 때 퀴즈 이미지 비활성화

		currentEventIndex++;
		ShowEvent(currentEventIndex);

		// 다음 이벤트가 다이얼로그일 경우 choiceButton을 다시 활성화
		if (events[currentEventIndex].eventType == EventType.Dialogue)
		{
			foreach (var button in choiceButtons)
			{
				button.gameObject.SetActive(true);  // 다이얼로그로 넘어갈 때 버튼 활성화
			}
		}
	}

	// 선택지 선택 시 호출
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

	// 현재 이벤트 id 반환
	public Hall50thData GetCurrentEventData()
	{
		return events[currentEventIndex];
	}

}
