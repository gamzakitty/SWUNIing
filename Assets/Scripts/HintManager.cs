using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
	public GameObject hintPanel;				// ��Ʈ Panel
	public Button useButton;					// ��� ��ư
	public Button rejectButton;					// ��� ��ư
	public Button confirmButton;				// Ȯ�� ��ư
	public TextMeshProUGUI checkTMP;			// ��Ʈ�� ������� ���� TextMeshPro �ؽ�Ʈ
	public TextMeshProUGUI hintTMP;             // ��Ʈ�� ǥ���� TextMeshPro �ؽ�Ʈ
	public TextMeshProUGUI point;               // ����Ʈ�� ǥ���� TextMeshPro �ؽ�Ʈ
	public TextMeshProUGUI hintCount;           // ��Ʈ ��� ������ ǥ���� TextMeshPro �ؽ�Ʈ

	private Hall50thManager hall50thManager;
	private bool isHintShowed = false;          // ��Ʈ ǥ�� ����
	private int previousEventId = -1;           // ���� �̺�Ʈ ID�� �����ϴ� ����
	private int currentPoints = 2000;           // �ʱ� ����Ʈ ����
	private int currentHintCount = 0;			// �ʱ� ��Ʈ ��� ���� ����

	private void Start()
	{
		// Hall50thManager ã��
		hall50thManager = FindObjectOfType<Hall50thManager>();
		hintPanel.SetActive(false);             // �ʱ� ���¿��� hintPanel ��Ȱ��ȭ
		point.text = currentPoints.ToString();  // �ʱ� ����Ʈ �ؽ�Ʈ ����

		// ��ư Ŭ�� �̺�Ʈ ����
		useButton.onClick.AddListener(OnUseButtonClicked);
		rejectButton.onClick.AddListener(OnRejectButtonClicked);
		confirmButton.onClick.AddListener(OnConfirmButtonClicked);
	}

	// ��Ʈ ������ Ŭ�� �� ȣ���� �޼���
	public void ShowHint()
	{
		// ���� �̺�Ʈ ������ ��������
		Hall50thData currentEventData = hall50thManager.GetCurrentEventData();

		// �̺�Ʈ ID�� ������ �ٸ��� isHintShowed�� false�� ����
		if (currentEventData != null && currentEventData.nextEventIds.Count > 0)
		{
			if (currentEventData.nextEventIds[0] != previousEventId)
			{
				isHintShowed = false;
				previousEventId = currentEventData.nextEventIds[0];
			}

			// ��Ʈ�� �����Ǿ� �ִ� ��� hintPanel�� ������
			if (!string.IsNullOrEmpty(currentEventData.hintText))
			{
				// ����Ʈ�� �������� Ȯ��
				if (currentPoints < 500)
				{
					hintPanel.SetActive(true);					// ��Ʈ �г� Ȱ��ȭ
					isHintShowed = true;						// ��Ʈ�� ǥ�� ���·� ����
					hintTMP.text = "����Ʈ�� �����մϴ�";		// ����Ʈ ���� �޽��� ǥ��
					confirmButton.gameObject.SetActive(true);	// Ȯ�� ��ư Ȱ��ȭ
					useButton.gameObject.SetActive(false);
					rejectButton.gameObject.SetActive(false);
					checkTMP.gameObject.SetActive(false);
				}
				else
				{
					// ����Ʈ�� ����ϸ� ���� ������� ��Ʈ�� ������� ���� UI ǥ��
					hintPanel.SetActive(true);
					UpdateHintUI();
				}
			}
			else
			{
				Debug.Log("�ش� �̺�Ʈ���� ��Ʈ�� �����ϴ�.");
			}
		}
	}

	// UI ���� ������Ʈ �޼���
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

	// ��� ��ư Ŭ�� ��
	private void OnUseButtonClicked()
	{
		if (currentPoints >= 500)
		{
			currentPoints -= 500;							// ����Ʈ ����
			point.text = currentPoints.ToString();			// ����Ʈ �ؽ�Ʈ ������Ʈ

			currentHintCount += 1;							// ��Ʈ ��� ���� 1 ����
			hintCount.text = currentHintCount.ToString();	// ��Ʈ ��� ���� �ؽ�Ʈ ������Ʈ

			isHintShowed = true;
			UpdateHintUI(); // UI ���� ������Ʈ
		}
		else
		{
			Debug.Log("����Ʈ�� �����Ͽ� ��Ʈ�� ����� �� �����ϴ�.");
		}
	}

	// ��� ��ư Ŭ�� ��
	private void OnRejectButtonClicked()
	{
		hintPanel.SetActive(false); // ��Ʈ �г� ��Ȱ��ȭ
	}

	// Ȯ�� ��ư Ŭ�� ��
	private void OnConfirmButtonClicked()
	{
		hintPanel.SetActive(false); // ��Ʈ �г� ��Ȱ��ȭ
	}
}
