using UnityEngine;
using Vuforia;

public class VuforiaFOVFix : MonoBehaviour
{
	public Camera arCamera;  // AR 카메라
	public float fixedFOV = 60f; // 원하는 고정 FOV 값

	void Start()
	{
		if (arCamera != null)
		{
			arCamera.fieldOfView = fixedFOV; // FOV를 고정
		}
	}
}
