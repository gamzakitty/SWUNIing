using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	//public GameObject canvas;
	public Image image;
	public TMP_Text text1;
	public TMP_Text text2;
	public Button nextButton;
	public Button exitButton;
	public TMP_InputField input;

    private int currentStep = 1;

    public Sprite schoolImage_1;
    public Sprite characterImage_1;
    public Sprite characterImage_2;
    public Sprite neonSignImage_quiz;
	public Sprite neonSignImage_answer;

	// Start is called before the first frame update
	void Start()
    {
		text2.gameObject.SetActive(false);
		exitButton.gameObject.SetActive(false);
        input.gameObject.SetActive(false);
		UpdateStory();
	}

    // Update is called once per frame
    private void UpdateStory()
    {
        switch (currentStep)
        {
            case 1:
				SetImage(schoolImage_1);
				text1.text = "���� �����ߴ�! ���Ⱑ 50�ֳ� �����ΰ�?";
				
				nextButton.GetComponentInChildren<TMP_Text>().text = "����";
                break;

            case 2:
				SetImage(characterImage_1);
				text1.text = "��, �� ��� ���� ������ �ִ� �� ����. ���� �����ٱ�?";
				
				nextButton.GetComponentInChildren<TMP_Text>().text = "���͵帱���?";
                break;

            case 3:
				SetImage(characterImage_2);
				text1.text = "<color=red>�� �����մϴ�! ���� ���� ī��ing�� �����ϴµ� ���� �Ҿ����.</color>";
				AdjustTextHeight(text1);
				nextButton.GetComponentInChildren<TMP_Text>().text = "ī��ing? �� �� ������...";
                break;

            case 4:
				text2.gameObject.SetActive(true);
				image.rectTransform.SetSiblingIndex(1); // �� ��°�� ����
				text1.text = "�´�, ī��ing�� �̷� �׿»��� ������ �־���!";
				SetImage(neonSignImage_quiz);
				text2.text = "�ٵ�... ������ �ܾ ��������?";
                input.gameObject.SetActive(true);
				nextButton.GetComponentInChildren<TMP_Text>().text = "Ȯ��";
                break;

			default:
				image.rectTransform.SetSiblingIndex(0); // ����ġ
				text2.gameObject.SetActive(false);
				input.gameObject.SetActive(false);
				nextButton.gameObject.SetActive(false);
				exitButton.gameObject.SetActive(true);
				SetImage(neonSignImage_answer);
				text1.text = "Clear!";
				exitButton.GetComponentInChildren<TMP_Text>().text = "����";
                break;

        }
    }

	// ���� ��ư Ŭ�� �Լ�
	public void OnNextButtonClick()
	{
		currentStep++;
		UpdateStory();
	}

	// ���� ��ư Ŭ�� �Լ�
	public void OnExitButtonClick()
    {
		Application.Quit();
	}

	// �������� �̹����� �����ϴ� �Լ�
	public void SetImage(Sprite newSprite)
	{
		// �̹��� ������Ʈ�� ���ο� Sprite ����
		image.sprite = newSprite;

		// ���� ������ �����ϸ鼭 �̹����� ǥ��
		image.preserveAspect = true;
	}

	// �������� �ؽ�Ʈ ĭ ���� �����ϴ� �Լ�
	public void AdjustTextHeight(TMP_Text text)
	{
		// �ؽ�Ʈ�� �������� �´� ���� ���
		float preferredHeight = text.preferredHeight;

		// RectTransform�� ���̸� ������ ���̿� �°� ����
		RectTransform rectTransform = text.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);
	}
}
