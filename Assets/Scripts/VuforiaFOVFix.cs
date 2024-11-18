using UnityEngine;
using Vuforia;

public class VuforiaFOVFix : MonoBehaviour
{
	public Camera arCamera;  // AR ī�޶�
	public float fixedFOV = 60f; // ���ϴ� ���� FOV ��

	void Start()
	{
		if (arCamera != null)
		{
			arCamera.fieldOfView = fixedFOV; // FOV�� ����
		}
	}
}
