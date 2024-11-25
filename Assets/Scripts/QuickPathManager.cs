using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LocationPanelController : MonoBehaviour
{
	[System.Serializable]
	public class LocationData
	{
		public Image locationIcon;   // 장소 아이콘 (이미지)
		public Sprite locationImage; // 장소와 연관된 이미지
	}

	public GameObject locationPanel;  // 패널 오브젝트
	public Image displayImage;        // 패널에 표시될 이미지 (UI Image)
	public List<LocationData> locations; // 장소 아이콘과 이미지의 리스트

	private void Start()
	{
		// 패널 비활성화로 시작
		if (locationPanel != null)
		{
			locationPanel.SetActive(false);
		}

		// 모든 아이콘에 클릭 이벤트 등록
		foreach (var location in locations)
		{
			if (location.locationIcon != null)
			{
				AddClickListener(location.locationIcon, location.locationImage);
			}
		}
	}

	// 클릭 이벤트를 아이콘 이미지에 추가
	private void AddClickListener(Image icon, Sprite associatedImage)
	{
		EventTrigger trigger = icon.gameObject.AddComponent<EventTrigger>();

		// 클릭 이벤트 생성
		EventTrigger.Entry entry = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry.callback.AddListener((data) => { OnIconClicked(associatedImage); });
		trigger.triggers.Add(entry);
	}

	// 아이콘 클릭 시 호출되는 함수
	private void OnIconClicked(Sprite locationImage)
	{
		if (locationPanel != null && displayImage != null)
		{
			// 패널 활성화
			locationPanel.SetActive(true);

			// 클릭한 아이콘에 해당하는 이미지를 패널에 표시
			displayImage.sprite = locationImage;

			// 이미지 크기를 비율에 맞게 조정
			AdjustImageSize(displayImage, locationImage);
		}
	}

	private void Update()
	{
		// Input System에서 마우스 클릭 이벤트 확인
		if (locationPanel.activeSelf && Mouse.current.leftButton.wasPressedThisFrame)
		{
			locationPanel.SetActive(false);
			if (displayImage != null)
			{
				displayImage.sprite = null; // 이미지 제거
			}
		}
	}

	// 이미지의 너비를 600으로 고정하고 높이를 비율에 맞게 조정
	private void AdjustImageSize(Image imageComponent, Sprite sprite)
	{
		if (sprite == null || imageComponent == null) return;

		float spriteWidth = sprite.rect.width;
		float spriteHeight = sprite.rect.height;

		// 비율 계산
		float aspectRatio = spriteHeight / spriteWidth;

		// 너비는 600으로 고정, 높이는 비율에 따라 계산
		RectTransform imageRect = imageComponent.GetComponent<RectTransform>();
		imageRect.sizeDelta = new Vector2(600, 600 * aspectRatio);
	}
}

