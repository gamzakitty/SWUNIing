using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	// 아이템들을 담은 리스트 (각 아이템은 GameObject로 가정)
	public List<GameObject> allItems; // 전체 아이템 리스트
	public GameObject contentPanel;   // 스크롤 뷰의 컨텐츠 패널

	// 필터 버튼들 (인덱스 버튼들)
	public GameObject indexButtonAll;        // 전체 보기 버튼
	public GameObject indexButtonTimetable;  // 시간표 인덱스 버튼
	public GameObject indexButton2;
	public GameObject indexButton3;
	public GameObject indexButton4;
	public GameObject indexButton5;
	public GameObject indexButton6;
	public GameObject indexButton7;

	private void Start()
	{
		// 인덱스 버튼에 클릭 이벤트 추가
		indexButtonAll.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(0));
		indexButtonTimetable.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(1));
		indexButton2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(2));
		indexButton3.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(3));
		indexButton4.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(4));
		indexButton5.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(5));
		indexButton6.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(6));
		indexButton7.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(7));
		
		// 시작할 때 전체 아이템 보여주기
		ShowItemsByIndex(0);
	}

	public void ShowItemsByIndex(int index)
	{
		// 모든 아이템을 비활성화
		foreach (var item in allItems)
		{
			item.SetActive(false);
		}

		// 인덱스에 따라 활성화할 아이템 설정
		List<int> itemsToShow = new List<int>();

		switch (index)
		{
			case 0: // 전체 보기
				itemsToShow = new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // 전체 아이템을 활성화
				break;
			case 1:// 인덱스 1를 눌렀을 때 (아이템 0 보이기)
				itemsToShow = new List<int> { 0 };
				break;
			case 2: // 인덱스 2를 눌렀을 때 (임시로 아이템 2, 3, 4 보이기)
				itemsToShow = new List<int> { 1, 2, 3 };
				break;
			case 3: // 인덱스 3을 눌렀을 때 (임시로 아이템 5, 6 보이기)
				itemsToShow = new List<int> { 4, 5 };
				break;
			case 4: // 인덱스 4를 눌렀을 때 (임시로 아이템 7 보이기)
				itemsToShow = new List<int> { 6 };
				break;
		}

		// 선택된 아이템들만 활성화
		foreach (int i in itemsToShow)
		{
			allItems[i].SetActive(true);
		}

		// UI 업데이트: 레이아웃 그룹을 다시 빌드하여 숨겨진 아이템의 자리를 없앰
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
	}
}
