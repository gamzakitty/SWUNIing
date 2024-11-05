using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
	public TextMeshProUGUI characterNameHeaderTMP; // �޽��� â ����� TMP
	public TextMeshProUGUI cardTMP;				   // �޽��� ĭ�� TMP
	public GameObject messageBackground; // 1��1 �޽��� â ��� (MessageBackground)

	private void Start()
	{
		// �ʱ⿡�� MessageBackground�� ��Ȱ��ȭ
		messageBackground.SetActive(false);
	}

	// ĳ���� �̸� ���� �� MessageBackground Ȱ��ȭ
	public void OpenMessageWindow(string characterName, string content)
	{
		// ĳ���� �̸� ����
		characterNameHeaderTMP.text = characterName;
		cardTMP.text = content;

		// MessageBackground Ȱ��ȭ
		messageBackground.SetActive(true);
	}
}
