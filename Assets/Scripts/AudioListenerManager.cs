using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
	public Camera mainCamera;   // Main Camera
	public Camera arCamera;     // AR Camera

	void Start()
	{
		// 기본적으로 Main Camera의 Audio Listener만 활성화
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
			mainCamera.GetComponent<AudioListener>().enabled = false; // 메인 카메라 Audio Listener 비활성화
		}

		if (arCamera != null)
		{
			arCamera.GetComponent<AudioListener>().enabled = true;  // AR 카메라 Audio Listener 활성화
		}
	}

	public void DeactivateARCamera()
	{
		if (arCamera != null)
		{
			arCamera.GetComponent<AudioListener>().enabled = false; // AR 카메라 Audio Listener 비활성화
		}

		if (mainCamera != null)
		{
			mainCamera.GetComponent<AudioListener>().enabled = true; // 메인 카메라 Audio Listener 활성화
		}
	}
}
