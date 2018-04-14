using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RabbitInfoScrollView : MonoBehaviour {
public RabbitInfoPanel prevPanel;
public RabbitInfoPanel currentPanel;
public RabbitInfoPanel nextPanel;
public RectTransform scrollContent;
int currentRabbitIndex = 1;
int unlockedMaxIndex;
bool atCenter = false;
public float slideSpeedByButton;

	public void InertiaOn()
	{
		GetComponent<ScrollRect>().inertia = true;
	}

	public void SetAllInfoPanels(int index)
	{
		currentPanel.SetRabbitInfo(currentRabbitIndex);
		prevPanel.SetRabbitInfo(currentRabbitIndex - 1);
		nextPanel.SetRabbitInfo(currentRabbitIndex + 1);
	}

	public void SlideToPrevPanel()
	{
		StartCoroutine("SlidingToPrevPanel");
	}

	public void SlideToNextPanel()
	{
		StartCoroutine("SlidingToNextPanel");
	}

	public IEnumerator SlidingToPrevPanel()
	{
		while(scrollContent.anchoredPosition.x < 1080)
		{
			scrollContent.anchoredPosition 
			= new Vector2(scrollContent.anchoredPosition.x + slideSpeedByButton, scrollContent.anchoredPosition.y);
			yield return null;
		}
	}

	public IEnumerator SlidingToNextPanel()
	{
		while(scrollContent.anchoredPosition.x > -1080)
		{
			scrollContent.anchoredPosition 
			= new Vector2(scrollContent.anchoredPosition.x - slideSpeedByButton, scrollContent.anchoredPosition.y);
			yield return null;
		}
	}

	// Use this for initialization
	void Start () {
		scrollContent = GetComponent<ScrollRect>().content;
		currentRabbitIndex = 1;
		SetAllInfoPanels(currentRabbitIndex);
		scrollContent.anchoredPosition = new Vector2(1080, 0);
		atCenter = false;
	}
	
	// Update is called once per frame
	void Update () {
		if( !atCenter &&
			((currentRabbitIndex == 1 && scrollContent.anchoredPosition.x < 0) 
			|| (currentRabbitIndex == 29 && scrollContent.anchoredPosition.x > 0)) )
		{
			StopCoroutine("SlidingToPrevPanel");
			StopCoroutine("SlidingToNextPanel");
			scrollContent.anchoredPosition = new Vector2();
			GetComponent<ScrollRect>().inertia = false;
			atCenter = true;
		}
		if(scrollContent.anchoredPosition.x >= 1080)
		{
			atCenter = false;
			if(currentRabbitIndex > 1)
			{
				// prevPanel이 보이도록 슬라이딩된 경우
				// currentPanel의 정보가 prevPanel과 같도록 패널들의 정보를 바꾸고
				currentRabbitIndex--;
				SetAllInfoPanels(currentRabbitIndex);
				// currentPanel이 보이도록 위치를 조정한다
				StopCoroutine("SlidingToPrevPanel");
				scrollContent.anchoredPosition = new Vector2();
				// 평생 슬라이딩되지 않도록 관성 적용 해제(다시 클릭할 때 활성화)
				GetComponent<ScrollRect>().inertia = false;
			}
		}
		if(scrollContent.anchoredPosition.x <= - 1080)
		{
			atCenter = false;
			if(currentRabbitIndex < 29)
			{
				//nextPanel이 보이도록 슬라이딩된 경우
				// currentPanel의 정보가 nextPanel과 같도록 바꾸고
				currentRabbitIndex++;
				SetAllInfoPanels(currentRabbitIndex);
				// currentPanel이 보이도록 위치를 조정한다
				StopCoroutine("SlidingToNextPanel");
				scrollContent.anchoredPosition = new Vector2();
				// 평생 슬라이딩되지 않도록 관성 적용 해제(다시 클릭할 때 활성화)
				GetComponent<ScrollRect>().inertia = false;
			}
		}
	}
}
