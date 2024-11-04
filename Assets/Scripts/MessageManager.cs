using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
	public TextMeshProUGUI characterNameHeaderTMP; // �޽��� â ����� TMP
	public GameObject messageBackground; // 1��1 �޽��� â ��� (MessageBackground)

	private void Start()
	{
		// �ʱ⿡�� MessageBackground�� ��Ȱ��ȭ
		messageBackground.SetActive(false);
	}

	// ĳ���� �̸� ���� �� MessageBackground Ȱ��ȭ
	public void OpenMessageWindow(string characterName)
	{
		// ĳ���� �̸� ����
		characterNameHeaderTMP.text = characterName;

		// MessageBackground Ȱ��ȭ
		messageBackground.SetActive(true);
	}
}
