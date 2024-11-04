using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MessageListItem : MonoBehaviour, IPointerClickHandler
{
	public TextMeshProUGUI characterNameTMP; // ĳ���� �̸��� ���� TMP
	private MessageManager messageManager;

	private void Start()
	{
		// MessageManager�� ã�� ����
		messageManager = FindObjectOfType<MessageManager>();
	}

	// �̹��� Ŭ�� �̺�Ʈ
	public void OnPointerClick(PointerEventData eventData)
	{
		if (messageManager != null)
		{
			// ĳ���� �̸��� MessageManager�� �����Ͽ� 1��1 �޽��� â�� ���ϴ�.
			messageManager.OpenMessageWindow(characterNameTMP.text);
		}
	}
}
