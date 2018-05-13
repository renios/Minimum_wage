using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Enums;

public class RabbitInfoScrollView : MonoBehaviour {
public RabbitInfoPanel prevPanel;
public RabbitInfoPanel currentPanel;
public RabbitInfoPanel nextPanel;
public RectTransform scrollContent;
public int currentPanelIndex = 0;
public bool atCenter = false;
public float slideSpeedByButton;
public List<int> unlockedIndexList;
public RabbitCatalogManager rabbitCatalogManager;

	public void InertiaOn()
	{
		GetComponent<ScrollRect>().inertia = true;
	}

	public void SetAllInfoPanels(int index)
	{
		if(index == 0)
		{
			currentPanel.SetRabbitInfo(unlockedIndexList[index]);
			nextPanel.SetRabbitInfo(unlockedIndexList[index + 1]);
			prevPanel.idImage.enabled = false;
		}
		else if(index == unlockedIndexList.Count - 1)
		{
			currentPanel.SetRabbitInfo(unlockedIndexList[index]);
			prevPanel.SetRabbitInfo(unlockedIndexList[index - 1]);
			nextPanel.idImage.enabled = false;
		}
		else
		{
			currentPanel.SetRabbitInfo(unlockedIndexList[index]);
			prevPanel.SetRabbitInfo(unlockedIndexList[index - 1]);
			nextPanel.SetRabbitInfo(unlockedIndexList[index + 1]);
		}
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

	public void Initialize()
	{
		int unlockProgress = PlayerPrefs.GetInt("UnlockProgress", 1);
		unlockedIndexList = new List<int>();
		for(int j = 0; j < unlockProgress; j++)
		{
			for(int i = 0; i < RabbitData.numberOfRabbitData; i++)
			{
				Rabbit rabbitData = RabbitData.GetRabbitData(i + 1);
				if(rabbitData.releaseStageIndex == j + 1)
				{
					unlockedIndexList.Add(rabbitData.index);
				}
			}
		}

		// 슬라이드 기능 막아놓음
//		currentPanelIndex = 0;
//		SetAllInfoPanels(currentPanelIndex);
		currentPanel.Initialize();
		scrollContent.anchoredPosition = new Vector2(1080, 0);
		atCenter = false;
	}

	// Use this for initialization
	void Start () {
		scrollContent = GetComponent<ScrollRect>().content;
		rabbitCatalogManager = FindObjectOfType<RabbitCatalogManager>();
	}
	
	// Update is called once per frame
	void Update () {
		// 하단부 슬라이드 기능 임시 제거.
//		if( !atCenter &&
//			((currentPanelIndex == 0 && scrollContent.anchoredPosition.x < 0) 
//			|| (currentPanelIndex == unlockedIndexList.Count - 1 && scrollContent.anchoredPosition.x > 0)) )
//		{
//			StopCoroutine("SlidingToPrevPanel");
//			StopCoroutine("SlidingToNextPanel");
//			scrollContent.anchoredPosition = new Vector2();
//			GetComponent<ScrollRect>().inertia = false;
//			atCenter = true;
//		}
//		if(scrollContent.anchoredPosition.x >= 1080 - 1)
//		{
//			atCenter = false;
//			if(currentPanelIndex > 0)
//			{
//				// prevPanel이 보이도록 슬라이딩된 경우
//				// currentPanel의 정보가 prevPanel과 같도록 패널들의 정보를 바꾸고
//				currentPanelIndex--;
//				SetAllInfoPanels(currentPanelIndex);
//				rabbitCatalogManager.ShowMatchingRabbit(currentPanelIndex);
//				// currentPanel이 보이도록 위치를 조정한다
//				StopCoroutine("SlidingToPrevPanel");
//				scrollContent.anchoredPosition = new Vector2();
//				// 평생 슬라이딩되지 않도록 관성 적용 해제(다시 클릭할 때 활성화)
//				GetComponent<ScrollRect>().inertia = false;
//			}
//		}
//		if(scrollContent.anchoredPosition.x <= - 1080 + 1)
//		{
//			atCenter = false;
//			if(currentPanelIndex < unlockedIndexList.Count - 1)
//			{
//				//nextPanel이 보이도록 슬라이딩된 경우
//				// currentPanel의 정보가 nextPanel과 같도록 바꾸고
//				currentPanelIndex++;
//				SetAllInfoPanels(currentPanelIndex);
//				rabbitCatalogManager.ShowMatchingRabbit(currentPanelIndex);
//				// currentPanel이 보이도록 위치를 조정한다
//				StopCoroutine("SlidingToNextPanel");
//				scrollContent.anchoredPosition = new Vector2();
//				// 평생 슬라이딩되지 않도록 관성 적용 해제(다시 클릭할 때 활성화)
//				GetComponent<ScrollRect>().inertia = false;
//			}
//		}
	}
}
