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
				text1.text = "드디어 도착했다! 여기가 50주년 기념관인가?";
				
				nextButton.GetComponentInChildren<TMP_Text>().text = "다음";
                break;

            case 2:
				SetImage(characterImage_1);
				text1.text = "어, 저 사람 뭔가 문제가 있는 것 같아. 내가 도와줄까?";
				
				nextButton.GetComponentInChildren<TMP_Text>().text = "도와드릴까요?";
                break;

            case 3:
				SetImage(characterImage_2);
				text1.text = "<color=red>아 감사합니다! 제가 지금 카페ing에 가야하는데 길을 잃었어요.</color>";
				AdjustTextHeight(text1);
				nextButton.GetComponentInChildren<TMP_Text>().text = "카페ing? 들어본 것 같은데...";
                break;

            case 4:
				text2.gameObject.SetActive(true);
				image.rectTransform.SetSiblingIndex(1); // 두 번째로 설정
				text1.text = "맞다, 카페ing에 이런 네온사인 문구가 있었지!";
				SetImage(neonSignImage_quiz);
				text2.text = "근데... 마지막 단어가 뭐였더라?";
                input.gameObject.SetActive(true);
				nextButton.GetComponentInChildren<TMP_Text>().text = "확인";
                break;

			default:
				image.rectTransform.SetSiblingIndex(0); // 원위치
				text2.gameObject.SetActive(false);
				input.gameObject.SetActive(false);
				nextButton.gameObject.SetActive(false);
				exitButton.gameObject.SetActive(true);
				SetImage(neonSignImage_answer);
				text1.text = "Clear!";
				exitButton.GetComponentInChildren<TMP_Text>().text = "종료";
                break;

        }
    }

	// 다음 버튼 클릭 함수
	public void OnNextButtonClick()
	{
		currentStep++;
		UpdateStory();
	}

	// 종료 버튼 클릭 함수
	public void OnExitButtonClick()
    {
		Application.Quit();
	}

	// 동적으로 이미지를 설정하는 함수
	public void SetImage(Sprite newSprite)
	{
		// 이미지 컴포넌트에 새로운 Sprite 설정
		image.sprite = newSprite;

		// 원본 비율을 유지하면서 이미지를 표시
		image.preserveAspect = true;
	}

	// 동적으로 텍스트 칸 높이 설정하는 함수
	public void AdjustTextHeight(TMP_Text text)
	{
		// 텍스트의 콘텐츠에 맞는 높이 계산
		float preferredHeight = text.preferredHeight;

		// RectTransform의 높이를 콘텐츠 높이에 맞게 조정
		RectTransform rectTransform = text.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);
	}
}
