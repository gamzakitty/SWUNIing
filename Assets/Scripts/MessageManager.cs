using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
	public TextMeshProUGUI characterNameHeaderTMP; // 메시지 창 상단의 TMP
	public GameObject messageBackground; // 1대1 메시지 창 배경 (MessageBackground)

	private void Start()
	{
		// 초기에는 MessageBackground를 비활성화
		messageBackground.SetActive(false);
	}

	// 캐릭터 이름 설정 및 MessageBackground 활성화
	public void OpenMessageWindow(string characterName)
	{
		// 캐릭터 이름 설정
		characterNameHeaderTMP.text = characterName;

		// MessageBackground 활성화
		messageBackground.SetActive(true);
	}
}
