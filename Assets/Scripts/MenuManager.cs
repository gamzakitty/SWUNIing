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
		// �г� �ʱ� ���´� ��� ��Ȱ��ȭ
		inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
		messagePanel.SetActive(false);
	}

	public void OpenInventoryPanel()
	{
		// ��� �г� �ݱ�
		CloseAllPanels();

		// ������ �г� ����
		inventoryPanel.SetActive(true);
	}

	public void OpenMapPanel()
	{
		// ��� �г� �ݱ�
		CloseAllPanels();

		// ������ �г� ����
		mapPanel.SetActive(true);
	}

	public void OpenMessagePanel()
	{
		// ��� �г� �ݱ�
		CloseAllPanels();

		// ������ �г� ����
		messagePanel.SetActive(true);
	}

	public void CloseAllPanels()
	{
		// ��� �г� ��Ȱ��ȭ
		inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
		messagePanel.SetActive(false);
		messageBackground.SetActive(false);
	}
}
