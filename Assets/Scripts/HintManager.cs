using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
	public GameObject hintPanel;				// 힌트 Panel
	public Button useButton;					// 사용 버튼
	public Button rejectButton;					// 취소 버튼
	public Button confirmButton;				// 확인 버튼
	public TextMeshProUGUI checkTMP;			// 힌트를 사용할지 묻는 TextMeshPro 텍스트
	public TextMeshProUGUI hintTMP;             // 힌트를 표시할 TextMeshPro 텍스트
	public TextMeshProUGUI point;               // 포인트를 표시할 TextMeshPro 텍스트
	public TextMeshProUGUI hintCount;           // 힌트 사용 개수를 표시할 TextMeshPro 텍스트

	private Hall50thManager hall50thManager;
	private bool isHintShowed = false;          // 힌트 표시 여부
	private int previousEventId = -1;           // 이전 이벤트 ID를 추적하는 변수
	private int currentPoints = 2000;           // 초기 포인트 설정
	private int currentHintCount = 0;			// 초기 힌트 사용 개수 설정

	private void Start()
	{
		// Hall50thManager 찾기
		hall50thManager = FindObjectOfType<Hall50thManager>();
		hintPanel.SetActive(false);             // 초기 상태에서 hintPanel 비활성화
		point.text = currentPoints.ToString();  // 초기 포인트 텍스트 설정

		// 버튼 클릭 이벤트 연결
		useButton.onClick.AddListener(OnUseButtonClicked);
		rejectButton.onClick.AddListener(OnRejectButtonClicked);
		confirmButton.onClick.AddListener(OnConfirmButtonClicked);
	}

	// 힌트 아이콘 클릭 시 호출할 메서드
	public void ShowHint()
	{
		// 현재 이벤트 데이터 가져오기
		Hall50thData currentEventData = hall50thManager.GetCurrentEventData();

		// 이벤트 ID가 이전과 다르면 isHintShowed를 false로 리셋
		if (currentEventData != null && currentEventData.nextEventIds.Count > 0)
		{
			if (currentEventData.nextEventIds[0] != previousEventId)
			{
				isHintShowed = false;
				previousEventId = currentEventData.nextEventIds[0];
			}

			// 힌트가 설정되어 있는 경우 hintPanel을 보여줌
			if (!string.IsNullOrEmpty(currentEventData.hintText))
			{
				// 포인트가 부족한지 확인
				if (currentPoints < 500)
				{
					hintPanel.SetActive(true);					// 힌트 패널 활성화
					isHintShowed = true;						// 힌트를 표시 상태로 설정
					hintTMP.text = "포인트가 부족합니다";		// 포인트 부족 메시지 표시
					confirmButton.gameObject.SetActive(true);	// 확인 버튼 활성화
					useButton.gameObject.SetActive(false);
					rejectButton.gameObject.SetActive(false);
					checkTMP.gameObject.SetActive(false);
				}
				else
				{
					// 포인트가 충분하면 기존 방식으로 힌트를 사용할지 묻는 UI 표시
					hintPanel.SetActive(true);
					UpdateHintUI();
				}
			}
			else
			{
				Debug.Log("해당 이벤트에는 힌트가 없습니다.");
			}
		}
	}

	// UI 상태 업데이트 메서드
	private void UpdateHintUI()
	{
		if (isHintShowed)
		{
			hintTMP.text = hall50thManager.GetCurrentEventData().hintText;
			hintTMP.gameObject.SetActive(true);
			confirmButton.gameObject.SetActive(true);
			useButton.gameObject.SetActive(false);
			rejectButton.gameObject.SetActive(false);
			checkTMP.gameObject.SetActive(false);
		}
		else
		{
			hintTMP.gameObject.SetActive(false);
			confirmButton.gameObject.SetActive(false);
			useButton.gameObject.SetActive(true);
			rejectButton.gameObject.SetActive(true);
			checkTMP.gameObject.SetActive(true);
		}
	}

	// 사용 버튼 클릭 시
	private void OnUseButtonClicked()
	{
		if (currentPoints >= 500)
		{
			currentPoints -= 500;							// 포인트 감소
			point.text = currentPoints.ToString();			// 포인트 텍스트 업데이트

			currentHintCount += 1;							// 힌트 사용 개수 1 증가
			hintCount.text = currentHintCount.ToString();	// 힌트 사용 개수 텍스트 업데이트

			isHintShowed = true;
			UpdateHintUI(); // UI 상태 업데이트
		}
		else
		{
			Debug.Log("포인트가 부족하여 힌트를 사용할 수 없습니다.");
		}
	}

	// 취소 버튼 클릭 시
	private void OnRejectButtonClicked()
	{
		hintPanel.SetActive(false); // 힌트 패널 비활성화
	}

	// 확인 버튼 클릭 시
	private void OnConfirmButtonClicked()
	{
		hintPanel.SetActive(false); // 힌트 패널 비활성화
	}
}
