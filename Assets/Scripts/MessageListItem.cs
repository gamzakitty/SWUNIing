using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MessageListItem : MonoBehaviour, IPointerClickHandler
{
	public TextMeshProUGUI characterNameTMP; // 캐릭터 이름이 적힌 TMP
	private MessageManager messageManager;

	private void Start()
	{
		// MessageManager를 찾아 참조
		messageManager = FindObjectOfType<MessageManager>();
	}

	// 이미지 클릭 이벤트
	public void OnPointerClick(PointerEventData eventData)
	{
		if (messageManager != null)
		{
			// 캐릭터 이름을 MessageManager에 전달하여 1대1 메시지 창을 엽니다.
			messageManager.OpenMessageWindow(characterNameTMP.text);
		}
	}
}
