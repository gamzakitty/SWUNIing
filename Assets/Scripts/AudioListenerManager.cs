using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
	public Camera mainCamera;   // Main Camera
	public Camera arCamera;     // AR Camera

	void Start()
	{
		// �⺻������ Main Camera�� Audio Listener�� Ȱ��ȭ
		if (mainCamera != null)
		{
			mainCamera.GetComponent<AudioListener>().enabled = true;
		}

		if (arCamera != null)
		{
			arCamera.GetComponent<AudioListener>().enabled = false;
		}
	}

	public void ActivateARCamera()
	{
		if (mainCamera != null)
		{
			mainCamera.GetComponent<AudioListener>().enabled = false; // ���� ī�޶� Audio Listener ��Ȱ��ȭ
		}

		if (arCamera != null)
		{
			arCamera.GetComponent<AudioListener>().enabled = true;  // AR ī�޶� Audio Listener Ȱ��ȭ
		}
	}

	public void DeactivateARCamera()
	{
		if (arCamera != null)
		{
			arCamera.GetComponent<AudioListener>().enabled = false; // AR ī�޶� Audio Listener ��Ȱ��ȭ
		}

		if (mainCamera != null)
		{
			mainCamera.GetComponent<AudioListener>().enabled = true; // ���� ī�޶� Audio Listener Ȱ��ȭ
		}
	}
}
