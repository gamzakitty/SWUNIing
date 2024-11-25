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
		public Image locationIcon;   // ��� ������ (�̹���)
		public Sprite locationImage; // ��ҿ� ������ �̹���
	}

	public GameObject locationPanel;  // �г� ������Ʈ
	public Image displayImage;        // �гο� ǥ�õ� �̹��� (UI Image)
	public List<LocationData> locations; // ��� �����ܰ� �̹����� ����Ʈ

	private void Start()
	{
		// �г� ��Ȱ��ȭ�� ����
		if (locationPanel != null)
		{
			locationPanel.SetActive(false);
		}

		// ��� �����ܿ� Ŭ�� �̺�Ʈ ���
		foreach (var location in locations)
		{
			if (location.locationIcon != null)
			{
				AddClickListener(location.locationIcon, location.locationImage);
			}
		}
	}

	// Ŭ�� �̺�Ʈ�� ������ �̹����� �߰�
	private void AddClickListener(Image icon, Sprite associatedImage)
	{
		EventTrigger trigger = icon.gameObject.AddComponent<EventTrigger>();

		// Ŭ�� �̺�Ʈ ����
		EventTrigger.Entry entry = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerClick
		};
		entry.callback.AddListener((data) => { OnIconClicked(associatedImage); });
		trigger.triggers.Add(entry);
	}

	// ������ Ŭ�� �� ȣ��Ǵ� �Լ�
	private void OnIconClicked(Sprite locationImage)
	{
		if (locationPanel != null && displayImage != null)
		{
			// �г� Ȱ��ȭ
			locationPanel.SetActive(true);

			// Ŭ���� �����ܿ� �ش��ϴ� �̹����� �гο� ǥ��
			displayImage.sprite = locationImage;

			// �̹��� ũ�⸦ ������ �°� ����
			AdjustImageSize(displayImage, locationImage);
		}
	}

	private void Update()
	{
		// Input System���� ���콺 Ŭ�� �̺�Ʈ Ȯ��
		if (locationPanel.activeSelf && Mouse.current.leftButton.wasPressedThisFrame)
		{
			locationPanel.SetActive(false);
			if (displayImage != null)
			{
				displayImage.sprite = null; // �̹��� ����
			}
		}
	}

	// �̹����� �ʺ� 600���� �����ϰ� ���̸� ������ �°� ����
	private void AdjustImageSize(Image imageComponent, Sprite sprite)
	{
		if (sprite == null || imageComponent == null) return;

		float spriteWidth = sprite.rect.width;
		float spriteHeight = sprite.rect.height;

		// ���� ���
		float aspectRatio = spriteHeight / spriteWidth;

		// �ʺ�� 600���� ����, ���̴� ������ ���� ���
		RectTransform imageRect = imageComponent.GetComponent<RectTransform>();
		imageRect.sizeDelta = new Vector2(600, 600 * aspectRatio);
	}
}

