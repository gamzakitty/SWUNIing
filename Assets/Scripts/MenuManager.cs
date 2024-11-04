using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject inventoryPanel;
	public GameObject mapPanel;
	public GameObject messagePanel;
	public GameObject messageBackground;

	void Start()
	{
		// 패널 초기 상태는 모두 비활성화
		inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
		messagePanel.SetActive(false);
	}

	public void OpenInventoryPanel()
	{
		// 모든 패널 닫기
		CloseAllPanels();

		// 선택한 패널 열기
		inventoryPanel.SetActive(true);
	}

	public void OpenMapPanel()
	{
		// 모든 패널 닫기
		CloseAllPanels();

		// 선택한 패널 열기
		mapPanel.SetActive(true);
	}

	public void OpenMessagePanel()
	{
		// 모든 패널 닫기
		CloseAllPanels();

		// 선택한 패널 열기
		messagePanel.SetActive(true);
	}

	public void CloseAllPanels()
	{
		// 모든 패널 비활성화
		inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
		messagePanel.SetActive(false);
		messageBackground.SetActive(false);
	}
}
