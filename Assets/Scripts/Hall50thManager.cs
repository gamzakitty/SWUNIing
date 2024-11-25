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
	public List<Hall50thData> events;                // ���� ���������� ��ȭ �� �̺�Ʈ ����Ʈ
	
	public TextMeshProUGUI characterNameText;        // ĳ���� �̸��� ǥ���� UI �ؽ�Ʈ (TextMeshPro)
	public TextMeshProUGUI contentText;              // ��ȭ�� ���� ������ ǥ���� UI �ؽ�Ʈ (TextMeshPro)
	public TextMeshProUGUI titleText;                // �˾��� ���� �ؽ�Ʈ
	public Button[] choiceButtons;                   // ������ ��ư �迭
	//public string nextSceneName;					 // �������� �̵��� ��

	public TextMeshProUGUI quizPopupContentText;     // ���� �˾��� ���� �ؽ�Ʈ
	public TMP_InputField answerInputField;          // ���� ���� �Է� �ʵ� (TextMeshPro)
	public GameObject quizPanel;                     // ���� �˾� �г�
	public Image quizImage;                          // ���� ȭ�鿡���� �̹���
	public Image popupImage;                         // �˾������� �̹���
	public Button checkButton;                       // ���� Ȯ�� ��ư
	public Button okButton;                          // �˾������� Ȯ�� ��ư
	public Button cancelButton;                      // �˾������� ��� ��ư

	public GameObject ARCameraPanel;                 // AR ī�޶� �г�
	public GameObject ARCamera;                      // AR ī�޶� (Vuforia AR ī�޶�)
	public Button ARCancelButton;                    // AR �г��� ��� ��ư
	public Button ARCaptureButton;                   // AR �г��� ī�޶� �Կ� ��ư
	public Button AROkButton;                        // AR �г��� Ȯ�� ��ư
	public Button ARRetryButton;                     // AR �г��� �ٽ��ϱ� ��ư
	public TextMeshProUGUI ARPopupContentText;       // AR �˾��� ���� �ؽ�Ʈ
	public GameObject marker;                        // �̹��� �ν� �� ǥ���� ǥ��
	private Texture2D capturedImage;                 // ĸó�� ȭ�� ����

	//public GameObject messageWindow;                // �޽��� â ������Ʈ
	//public GameObject messageCardPrefab;            // �޽��� ī�� ������
	//public Transform messageContentParent;          // �޽��� ī�尡 �߰��� �θ� ��ü
	//public TMP_InputField responseInputField;       // �÷��̾��� �Է� �ʵ�
	//public Button responseButton;                   // �Է� Ȯ�� ��ư
	//private string characterNameHeader;

	private int currentEventIndex = 0;               // ���� ��ȭ �Ǵ� �̺�Ʈ �ε���
	public int nextSceneIndex = -1;					 // �������� �Ѿ ���� �ε��� (Build Setting���� Ȯ�� ����)

	private void Start()
	{
		checkButton.gameObject.SetActive(false);     // Ȯ�� ��ư ��Ȱ��ȭ
		okButton.gameObject.SetActive(false);        // OK ��ư ��Ȱ��ȭ
		popupImage.enabled = false;                  // �˾� �̹��� ��Ȱ��ȭ
		quizPanel.SetActive(false);                  // ���� �˾� ��Ȱ��ȭ
		quizImage.enabled = false;                   // ���� �̹��� ��Ȱ��ȭ

		ARCameraPanel.SetActive(false);              // AR �г� ��Ȱ��ȭ
		ARCamera.SetActive(false);                   // AR ī�޶� ��Ȱ��ȭ
		marker.SetActive(false);                     // ǥ�� ��Ȱ��ȭ
		ARCaptureButton.onClick.AddListener(CaptureScreen);
		AROkButton.onClick.AddListener(OnConfirmAR);
		ARRetryButton.onClick.AddListener(OnRetryAR);
		ResetButtons();

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

			case EventType.AR:
				ShowAR(eventData);
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
		AdjustTextSize();
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
		quizPopupContentText.text = eventData.quizPopupContent; // Quiz �̺�Ʈ�� �˾� ���� ���

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

	private void ResetButtons()
	{
		// ��ư �ʱ�ȭ
		ARCaptureButton.gameObject.SetActive(false);
		ARCancelButton.gameObject.SetActive(false);
		AROkButton.gameObject.SetActive(false);
		ARRetryButton.gameObject.SetActive(false);
	}

	private void ShowAR(Hall50thData eventData)
	{
		// ���� �ؽ�Ʈ ����
		contentText.text = eventData.content;
		AdjustTextSize();

		// AR �гΰ� ��ư �ʱ� ���� ��Ȱ��ȭ
		ARCameraPanel.SetActive(false);
		ARCamera.SetActive(false);
		ResetButtons(); // ��ư �ʱ�ȭ

		// AR ��ư(Ȯ�� ��ư) Ȱ��ȭ
		checkButton.gameObject.SetActive(true);
		checkButton.GetComponentInChildren<TextMeshProUGUI>().text = "AR";

		// AR ��ư Ŭ�� ������ ����
		checkButton.onClick.RemoveAllListeners();
		checkButton.onClick.AddListener(() =>
		{
			// AR �г� Ȱ��ȭ �� �ʱ� ��ư ���� ����
			ARCamera.SetActive(true);
			ARCameraPanel.SetActive(true);
			ARCaptureButton.gameObject.SetActive(true); // �Կ� ��ư Ȱ��ȭ
			ARCancelButton.gameObject.SetActive(true);  // ��� ��ư Ȱ��ȭ
			AROkButton.gameObject.SetActive(false);     // Ȯ�� ��ư ��Ȱ��ȭ
			ARRetryButton.gameObject.SetActive(false);  // �ٽ��ϱ� ��ư ��Ȱ��ȭ
		});

		// AR �̺�Ʈ�� �˾� ���� ����
		ARPopupContentText.text = eventData.ARPopupContent; // AR �̺�Ʈ �ؽ�Ʈ ����

		// ������ ��ư ��Ȱ��ȭ
		foreach (Button button in choiceButtons)
		{
			button.gameObject.SetActive(false);
		}

		// AR ��� ��ư ������ ����
		ARCancelButton.onClick.RemoveAllListeners();
		ARCancelButton.onClick.AddListener(() =>
		{
			ARCamera.SetActive(false);
			ARCameraPanel.SetActive(false);
			ResetButtons(); // ��ư �ʱ�ȭ
		});

		// �Կ� ��ư ������ ����
		ARCaptureButton.onClick.RemoveAllListeners();
		ARCaptureButton.onClick.AddListener(CaptureScreen);

		// Ȯ�� ��ư ������ ����
		AROkButton.onClick.RemoveAllListeners();
		AROkButton.onClick.AddListener(() =>
		{
			Debug.Log("Confirmed: Moving to the next event.");
			ARCamera.SetActive(false);
			ARCameraPanel.SetActive(false);
			ResetButtons();
			MoveToNextEvent();
		});

		// �ٽ��ϱ� ��ư ������ ����
		ARRetryButton.onClick.RemoveAllListeners();
		ARRetryButton.onClick.AddListener(() =>
		{
			Debug.Log("Retrying: Restarting AR Camera.");
			ARCamera.SetActive(true); // AR ī�޶� �ٽ� Ȱ��ȭ
			ResetButtons();
			ARCaptureButton.gameObject.SetActive(true); // �Կ� ��ư Ȱ��ȭ
			ARCancelButton.gameObject.SetActive(true);  // ��� ��ư Ȱ��ȭ
			marker.SetActive(false);                   // ǥ�� �ʱ�ȭ
		});
	}

	private void CaptureScreen()
	{
		StartCoroutine(CaptureAndDisplay());
	}

	private IEnumerator CaptureAndDisplay()
	{
		// �ǽð� �Կ� ����
		ARCamera.SetActive(false);  // AR ī�޶� ��Ȱ��ȭ

		yield return new WaitForEndOfFrame();

		// ȭ�� ĸó
		int width = Screen.width;
		int height = Screen.height;
		capturedImage = new Texture2D(width, height, TextureFormat.RGB24, false);
		capturedImage.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		capturedImage.Apply();

		// ĸó�� �̹����� ȭ�鿡 ǥ��
		quizImage.sprite = Sprite.Create(capturedImage, new Rect(0, 0, capturedImage.width, capturedImage.height), new Vector2(0.5f, 0.5f));
		quizImage.enabled = true;

		// ǥ�� Ȯ��
		if (marker.activeSelf)
		{
			Debug.Log("Marker detected: Confirm button activated.");
			ARCaptureButton.gameObject.SetActive(false); // �Կ� ��ư ��Ȱ��ȭ
			AROkButton.gameObject.SetActive(true);       // Ȯ�� ��ư Ȱ��ȭ
			ARRetryButton.gameObject.SetActive(false);   // �ٽ��ϱ� ��ư ��Ȱ��ȭ
			ARCancelButton.gameObject.SetActive(false);  // ��� ��ư ��Ȱ��ȭ
		}
		else
		{
			Debug.Log("Marker not detected: Retry button activated.");
			ARCaptureButton.gameObject.SetActive(false); // �Կ� ��ư ��Ȱ��ȭ
			AROkButton.gameObject.SetActive(false);      // Ȯ�� ��ư ��Ȱ��ȭ
			ARRetryButton.gameObject.SetActive(true);    // �ٽ��ϱ� ��ư Ȱ��ȭ
			ARCancelButton.gameObject.SetActive(false);  // ��� ��ư ��Ȱ��ȭ
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
		ARCaptureButton.gameObject.SetActive(true);   // �Կ� ��ư �ٽ� Ȱ��ȭ
		marker.SetActive(false);                      // ǥ�� �ʱ�ȭ
	}

	// �ؽ�Ʈ ũ�⿡ ���� RectTransform ����
	private void AdjustTextSize()
	{
		RectTransform rectTransform = contentText.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(contentText.preferredWidth, contentText.preferredHeight);
	}

	public void OnTargetFound()
	{
		marker?.SetActive(true); // �̹��� �ν� �� ǥ�� Ȱ��ȭ
	}

	public void OnTargetLost()
	{
		marker?.SetActive(false); // �̹��� �ν� ���� �� ǥ�� ��Ȱ��ȭ
	}

	private void OnDestroy()
	{
		// ARCamera�� null���� ���� Ȯ��
		if (ARCamera != null)
		{
			var observerEventHandler = ARCamera.GetComponentInChildren<DefaultObserverEventHandler>();
			if (observerEventHandler != null) // observerEventHandler�� null���� Ȯ��
			{
				observerEventHandler.OnTargetFound.RemoveListener(OnTargetFound);
				observerEventHandler.OnTargetLost.RemoveListener(OnTargetLost);
			}
		}

		// marker�� �����Ǿ����� Ȯ��
		if (marker != null)
		{
			Destroy(marker); // �ʿ� �� �ı�
		}
	}

	// ���� �̺�Ʈ�� �̵�
	private void MoveToNextEvent()
	{
		quizPanel.SetActive(false);
		checkButton.gameObject.SetActive(false);
		popupImage.enabled = false;                // ���� �̺�Ʈ�� �Ѿ �� �˾� �̹��� ��Ȱ��ȭ
		quizImage.enabled = false;                 // ���� �̺�Ʈ�� �Ѿ �� ���� �̹��� ��Ȱ��ȭ

		// ���� �̺�Ʈ ������ ��������
		Hall50thData currentEventData = events[currentEventIndex];

		// ���� �̺�Ʈ ID Ȯ��
		if (currentEventData.nextEventIds.Count > 0)
		{
			int nextEventIndex = currentEventData.nextEventIds[0];

			// ���� �̺�Ʈ ID�� -1�� ���, ���� ������ ��ȯ
			if (nextEventIndex == -1)
			{
				MoveToNextScene();
				return;
			}

			// ���� �̺�Ʈ�� �̵�
			if (nextEventIndex >= 0 && nextEventIndex < events.Count)
			{
				currentEventIndex = nextEventIndex; // ���� �̺�Ʈ�� �ε��� ����
				ShowEvent(currentEventIndex);      // ���� �̺�Ʈ ǥ��
			}
		}
		else
		{
			MoveToNextScene(); // ���� �̺�Ʈ�� ���� ��� �� ��ȯ
		}

		// ���� �̺�Ʈ�� ���̾�α��� ��� choiceButton�� �ٽ� Ȱ��ȭ
		if (events[currentEventIndex].eventType == EventType.Dialogue)
		{
			foreach (var button in choiceButtons)
			{
				button.gameObject.SetActive(true);  // ���̾�α׷� �Ѿ �� ��ư Ȱ��ȭ
			}
		}
	}

	// ������ �̺�Ʈ���� Ȯ��
	private bool IsLastEvent()
	{
		if (currentEventIndex >= 0 && currentEventIndex < events.Count)
		{
			Hall50thData currentEvent = events[currentEventIndex];
			return currentEvent.nextEventIds.Count == 0; // ���� �̺�Ʈ�� ������ Ȯ��
		}
		return false;
	}

	// ������ �̺�Ʈ�� ���� �� ���� ������ �̵�
	private void MoveToNextScene()
	{
		if (nextSceneIndex >= 0)
		{
			SceneManager.LoadScene(nextSceneIndex);
		}
	}

	private void OnChoiceSelected(int nextEventId)
	{
		// nextEventId�� -1�̸� ���� ������ ��ȯ
		if (nextEventId == -1)
		{
			MoveToNextScene();
			return;
		}

		// ��ȿ�� nextEventId��� �ش� �̺�Ʈ�� �̵�
		if (nextEventId >= 0 && nextEventId < events.Count)
		{
			currentEventIndex = nextEventId; // ���õ� �̺�Ʈ�� ����
			ShowEvent(currentEventIndex);
		}
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
