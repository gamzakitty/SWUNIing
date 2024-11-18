using UnityEngine;

public class ARCameraScaler : MonoBehaviour
{
	public Camera arCamera;         // AR ī�޶�
	public RectTransform popupImage; // PopupImage�� RectTransform

	void Update()
	{
		if (arCamera != null && popupImage != null)
		{
			// PopupImage�� ũ�⸦ ������ ī�޶� �ݿ�
			Vector2 size = popupImage.rect.size;
			arCamera.aspect = size.x / size.y; // AR ī�޶��� ������ PopupImage�� ����ȭ
		}
	}
}
