using UnityEngine;

public class ARCameraScaler : MonoBehaviour
{
	public Camera arCamera;         // AR 카메라
	public RectTransform popupImage; // PopupImage의 RectTransform

	void Update()
	{
		if (arCamera != null && popupImage != null)
		{
			// PopupImage의 크기를 가져와 카메라에 반영
			Vector2 size = popupImage.rect.size;
			arCamera.aspect = size.x / size.y; // AR 카메라의 비율을 PopupImage와 동기화
		}
	}
}
