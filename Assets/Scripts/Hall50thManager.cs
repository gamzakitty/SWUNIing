using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using UnityEngine.SceneManagement;

public class Hall50thManager : MonoBehaviour
{
	public List<Hall50thData> events;                // 현재 스테이지의 대화 및 이벤트 리스트
	
	public TextMeshProUGUI characterNameText;        // 캐릭터 이름을 표시할 UI 텍스트 (TextMeshPro)
	public TextMeshProUGUI contentText;              // 대화나 문제 내용을 표시할 UI 텍스트 (TextMeshPro)
	public TextMeshProUGUI titleText;                // 팝업의 제목 텍스트
	public Button[] choiceButtons;                   // 선택지 버튼 배열
	//public string nextSceneName;					 // 다음으로 이동할 씬

	public TextMeshProUGUI quizPopupContentText;     // 퀴즈 팝업의 내용 텍스트
	public TMP_InputField answerInputField;          // 퀴즈 정답 입력 필드 (TextMeshPro)
	public GameObject quizPanel;                     // 퀴즈 팝업 패널
	public Image quizImage;                          // 퀴즈 화면에서의 이미지
	public Image popupImage;                         // 팝업에서의 이미지
	public Button checkButton;                       // 퀴즈 확인 버튼
	public Button okButton;                          // 팝업에서의 확인 버튼
	public Button cancelButton;                      // 팝업에서의 취소 버튼

	public GameObject ARCameraPanel;                 // AR 카메라 패널
	public GameObject ARCamera;                      // AR 카메라 (Vuforia AR 카메라)
	public Button ARCancelButton;                    // AR 패널의 취소 버튼
	public Button ARCaptureButton;                   // AR 패널의 카메라 촬영 버튼
	public Button AROkButton;                        // AR 패널의 확인 버튼
	public Button ARRetryButton;                     // AR 패널의 다시하기 버튼
	public TextMeshProUGUI ARPopupContentText;       // AR 팝업의 내용 텍스트
	public GameObject marker;                        // 이미지 인식 시 표시할 표식
	private Texture2D capturedImage;                 // 캡처된 화면 저장

	//public GameObject messageWindow;                // 메시지 창 오브젝트
	//public GameObject messageCardPrefab;            // 메시지 카드 프리팹
	//public Transform messageContentParent;          // 메시지 카드가 추가될 부모 객체
	//public TMP_InputField responseInputField;       // 플레이어의 입력 필드
	//public Button responseButton;                   // 입력 확인 버튼
	//private string characterNameHeader;

	private int currentEventIndex = 0;               // 현재 대화 또는 이벤트 인덱스
	public int nextSceneIndex = -1;					 // 다음으로 넘어갈 씬의 인덱스 (Build Setting에서 확인 가능)

	private void Start()
	{
		checkButton.gameObject.SetActive(false);     // 확인 버튼 비활성화
		okButton.gameObject.SetActive(false);        // OK 버튼 비활성화
		popupImage.enabled = false;                  // 팝업 이미지 비활성화
		quizPanel.SetActive(false);                  // 퀴즈 팝업 비활성화
		quizImage.enabled = false;                   // 퀴즈 이미지 비활성화

		ARCameraPanel.SetActive(false);              // AR 패널 비활성화
		ARCamera.SetActive(false);                   // AR 카메라 비활성화
		marker.SetActive(false);                     // 표식 비활성화
		ARCaptureButton.onClick.AddListener(CaptureScreen);
		AROkButton.onClick.AddListener(OnConfirmAR);
		ARRetryButton.onClick.AddListener(OnRetryAR);
		ResetButtons();

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

			case EventType.AR:
				ShowAR(eventData);
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
		AdjustTextSize();
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
		quizPopupContentText.text = eventData.quizPopupContent; // Quiz 이벤트의 팝업 내용 사용

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

	private void ResetButtons()
	{
		// 버튼 초기화
		ARCaptureButton.gameObject.SetActive(false);
		ARCancelButton.gameObject.SetActive(false);
		AROkButton.gameObject.SetActive(false);
		ARRetryButton.gameObject.SetActive(false);
	}

	private void ShowAR(Hall50thData eventData)
	{
		// 본문 텍스트 설정
		contentText.text = eventData.content;
		AdjustTextSize();

		// AR 패널과 버튼 초기 상태 비활성화
		ARCameraPanel.SetActive(false);
		ARCamera.SetActive(false);
		ResetButtons(); // 버튼 초기화

		// AR 버튼(확인 버튼) 활성화
		checkButton.gameObject.SetActive(true);
		checkButton.GetComponentInChildren<TextMeshProUGUI>().text = "AR";

		// AR 버튼 클릭 리스너 설정
		checkButton.onClick.RemoveAllListeners();
		checkButton.onClick.AddListener(() =>
		{
			// AR 패널 활성화 및 초기 버튼 상태 설정
			ARCamera.SetActive(true);
			ARCameraPanel.SetActive(true);
			ARCaptureButton.gameObject.SetActive(true); // 촬영 버튼 활성화
			ARCancelButton.gameObject.SetActive(true);  // 취소 버튼 활성화
			AROkButton.gameObject.SetActive(false);     // 확인 버튼 비활성화
			ARRetryButton.gameObject.SetActive(false);  // 다시하기 버튼 비활성화
		});

		// AR 이벤트의 팝업 내용 설정
		ARPopupContentText.text = eventData.ARPopupContent; // AR 이벤트 텍스트 설정

		// 선택지 버튼 비활성화
		foreach (Button button in choiceButtons)
		{
			button.gameObject.SetActive(false);
		}

		// AR 취소 버튼 리스너 설정
		ARCancelButton.onClick.RemoveAllListeners();
		ARCancelButton.onClick.AddListener(() =>
		{
			ARCamera.SetActive(false);
			ARCameraPanel.SetActive(false);
			ResetButtons(); // 버튼 초기화
		});

		// 촬영 버튼 리스너 설정
		ARCaptureButton.onClick.RemoveAllListeners();
		ARCaptureButton.onClick.AddListener(CaptureScreen);

		// 확인 버튼 리스너 설정
		AROkButton.onClick.RemoveAllListeners();
		AROkButton.onClick.AddListener(() =>
		{
			Debug.Log("Confirmed: Moving to the next event.");
			ARCamera.SetActive(false);
			ARCameraPanel.SetActive(false);
			ResetButtons();
			MoveToNextEvent();
		});

		// 다시하기 버튼 리스너 설정
		ARRetryButton.onClick.RemoveAllListeners();
		ARRetryButton.onClick.AddListener(() =>
		{
			Debug.Log("Retrying: Restarting AR Camera.");
			ARCamera.SetActive(true); // AR 카메라 다시 활성화
			ResetButtons();
			ARCaptureButton.gameObject.SetActive(true); // 촬영 버튼 활성화
			ARCancelButton.gameObject.SetActive(true);  // 취소 버튼 활성화
			marker.SetActive(false);                   // 표식 초기화
		});
	}

	private void CaptureScreen()
	{
		StartCoroutine(CaptureAndDisplay());
	}

	private IEnumerator CaptureAndDisplay()
	{
		// 실시간 촬영 중지
		ARCamera.SetActive(false);  // AR 카메라 비활성화

		yield return new WaitForEndOfFrame();

		// 화면 캡처
		int width = Screen.width;
		int height = Screen.height;
		capturedImage = new Texture2D(width, height, TextureFormat.RGB24, false);
		capturedImage.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		capturedImage.Apply();

		// 캡처된 이미지를 화면에 표시
		quizImage.sprite = Sprite.Create(capturedImage, new Rect(0, 0, capturedImage.width, capturedImage.height), new Vector2(0.5f, 0.5f));
		quizImage.enabled = true;

		// 표식 확인
		if (marker.activeSelf)
		{
			Debug.Log("Marker detected: Confirm button activated.");
			ARCaptureButton.gameObject.SetActive(false); // 촬영 버튼 비활성화
			AROkButton.gameObject.SetActive(true);       // 확인 버튼 활성화
			ARRetryButton.gameObject.SetActive(false);   // 다시하기 버튼 비활성화
			ARCancelButton.gameObject.SetActive(false);  // 취소 버튼 비활성화
		}
		else
		{
			Debug.Log("Marker not detected: Retry button activated.");
			ARCaptureButton.gameObject.SetActive(false); // 촬영 버튼 비활성화
			AROkButton.gameObject.SetActive(false);      // 확인 버튼 비활성화
			ARRetryButton.gameObject.SetActive(true);    // 다시하기 버튼 활성화
			ARCancelButton.gameObject.SetActive(false);  // 취소 버튼 비활성화
		}
	}

	private void OnConfirmAR()
	{
		Debug.Log("Confirmed: Moving to the next event.");
		ResetButtons();
		MoveToNextEvent();
	}

	private void OnRetryAR()
	{
		Debug.Log("Retry: Restarting AR Camera.");
		ResetButtons();
		ARCaptureButton.gameObject.SetActive(true);   // 촬영 버튼 다시 활성화
		marker.SetActive(false);                      // 표식 초기화
	}

	// 텍스트 크기에 맞춰 RectTransform 조정
	private void AdjustTextSize()
	{
		RectTransform rectTransform = contentText.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(contentText.preferredWidth, contentText.preferredHeight);
	}

	public void OnTargetFound()
	{
		marker?.SetActive(true); // 이미지 인식 시 표식 활성화
	}

	public void OnTargetLost()
	{
		marker?.SetActive(false); // 이미지 인식 해제 시 표식 비활성화
	}

	private void OnDestroy()
	{
		// ARCamera가 null인지 먼저 확인
		if (ARCamera != null)
		{
			var observerEventHandler = ARCamera.GetComponentInChildren<DefaultObserverEventHandler>();
			if (observerEventHandler != null) // observerEventHandler가 null인지 확인
			{
				observerEventHandler.OnTargetFound.RemoveListener(OnTargetFound);
				observerEventHandler.OnTargetLost.RemoveListener(OnTargetLost);
			}
		}

		// marker가 삭제되었는지 확인
		if (marker != null)
		{
			Destroy(marker); // 필요 시 파괴
		}
	}

	// 다음 이벤트로 이동
	private void MoveToNextEvent()
	{
		quizPanel.SetActive(false);
		checkButton.gameObject.SetActive(false);
		popupImage.enabled = false;                // 다음 이벤트로 넘어갈 때 팝업 이미지 비활성화
		quizImage.enabled = false;                 // 다음 이벤트로 넘어갈 때 퀴즈 이미지 비활성화

		// 현재 이벤트 데이터 가져오기
		Hall50thData currentEventData = events[currentEventIndex];

		// 다음 이벤트 ID 확인
		if (currentEventData.nextEventIds.Count > 0)
		{
			int nextEventIndex = currentEventData.nextEventIds[0];

			// 다음 이벤트 ID가 -1인 경우, 다음 씬으로 전환
			if (nextEventIndex == -1)
			{
				MoveToNextScene();
				return;
			}

			// 다음 이벤트로 이동
			if (nextEventIndex >= 0 && nextEventIndex < events.Count)
			{
				currentEventIndex = nextEventIndex; // 다음 이벤트로 인덱스 설정
				ShowEvent(currentEventIndex);      // 다음 이벤트 표시
			}
		}
		else
		{
			MoveToNextScene(); // 다음 이벤트가 없는 경우 씬 전환
		}

		// 다음 이벤트가 다이얼로그일 경우 choiceButton을 다시 활성화
		if (events[currentEventIndex].eventType == EventType.Dialogue)
		{
			foreach (var button in choiceButtons)
			{
				button.gameObject.SetActive(true);  // 다이얼로그로 넘어갈 때 버튼 활성화
			}
		}
	}

	// 마지막 이벤트인지 확인
	private bool IsLastEvent()
	{
		if (currentEventIndex >= 0 && currentEventIndex < events.Count)
		{
			Hall50thData currentEvent = events[currentEventIndex];
			return currentEvent.nextEventIds.Count == 0; // 다음 이벤트가 없는지 확인
		}
		return false;
	}

	// 마지막 이벤트가 끝난 후 다음 씬으로 이동
	private void MoveToNextScene()
	{
		if (nextSceneIndex >= 0)
		{
			SceneManager.LoadScene(nextSceneIndex);
		}
	}

	private void OnChoiceSelected(int nextEventId)
	{
		// nextEventId가 -1이면 다음 씬으로 전환
		if (nextEventId == -1)
		{
			MoveToNextScene();
			return;
		}

		// 유효한 nextEventId라면 해당 이벤트로 이동
		if (nextEventId >= 0 && nextEventId < events.Count)
		{
			currentEventIndex = nextEventId; // 선택된 이벤트로 갱신
			ShowEvent(currentEventIndex);
		}
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
