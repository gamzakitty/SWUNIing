using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	// �����۵��� ���� ����Ʈ (�� �������� GameObject�� ����)
	public List<GameObject> allItems; // ��ü ������ ����Ʈ
	public GameObject contentPanel;   // ��ũ�� ���� ������ �г�

	// ���� ��ư�� (�ε��� ��ư��)
	public GameObject indexButtonAll;        // ��ü ���� ��ư
	public GameObject indexButtonTimetable;  // �ð�ǥ �ε��� ��ư
	public GameObject indexButton2;
	public GameObject indexButton3;
	public GameObject indexButton4;
	public GameObject indexButton5;
	public GameObject indexButton6;
	public GameObject indexButton7;

	private void Start()
	{
		// �ε��� ��ư�� Ŭ�� �̺�Ʈ �߰�
		indexButtonAll.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(0));
		indexButtonTimetable.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(1));
		indexButton2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(2));
		indexButton3.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(3));
		indexButton4.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(4));
		indexButton5.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(5));
		indexButton6.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(6));
		indexButton7.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemsByIndex(7));
		
		// ������ �� ��ü ������ �����ֱ�
		ShowItemsByIndex(0);
	}

	public void ShowItemsByIndex(int index)
	{
		// ��� �������� ��Ȱ��ȭ
		foreach (var item in allItems)
		{
			item.SetActive(false);
		}

		// �ε����� ���� Ȱ��ȭ�� ������ ����
		List<int> itemsToShow = new List<int>();

		switch (index)
		{
			case 0: // ��ü ����
				itemsToShow = new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // ��ü �������� Ȱ��ȭ
				break;
			case 1:// �ε��� 1�� ������ �� (������ 0 ���̱�)
				itemsToShow = new List<int> { 0 };
				break;
			case 2: // �ε��� 2�� ������ �� (�ӽ÷� ������ 2, 3, 4 ���̱�)
				itemsToShow = new List<int> { 1, 2, 3 };
				break;
			case 3: // �ε��� 3�� ������ �� (�ӽ÷� ������ 5, 6 ���̱�)
				itemsToShow = new List<int> { 4, 5 };
				break;
			case 4: // �ε��� 4�� ������ �� (�ӽ÷� ������ 7 ���̱�)
				itemsToShow = new List<int> { 6 };
				break;
		}

		// ���õ� �����۵鸸 Ȱ��ȭ
		foreach (int i in itemsToShow)
		{
			allItems[i].SetActive(true);
		}

		// UI ������Ʈ: ���̾ƿ� �׷��� �ٽ� �����Ͽ� ������ �������� �ڸ��� ����
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
	}
}
