using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RabbitCatalogManager : MonoBehaviour {

	public GameObject catalogPanel;
	public RabbitInfoScrollView rabbitInfoScrollView;
	public RabbitTableScrollView rabbitTableScrollView;
	public RectTransform[] tableRabbitRectTransform;

	public void ShowCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay).SetEase(Ease.OutBack);
		rabbitInfoScrollView.Initialize();
		rabbitTableScrollView.Initialize();
	}

	public void HideCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width*3/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
	}

	public void ShowMatchingInfo(int index)
	{
		// currentPanel로 content 이동
		rabbitInfoScrollView.scrollContent.anchoredPosition = new Vector2();
		// currentPanel에 index 값으로 값 바꾸기
		rabbitInfoScrollView.currentPanelIndex = index;
		rabbitInfoScrollView.SetAllInfoPanels(rabbitInfoScrollView.currentPanelIndex);
		rabbitInfoScrollView.atCenter = true;
	}

	public void ShowMatchingRabbit(int index)
	{
		string wallName = tableRabbitRectTransform[index].parent.name;
		print("wallName: " + wallName);
		int wallNumber = 0;
		int padding = (index < 2 ) ? 0 : 540;
		if(wallName.Length == 12)
		{
			wallNumber = wallName[11] - 48;
		}
		else if(wallName.Length == 13)
		{
			wallNumber = (wallName[11] - 48)*10 + wallName[12] - 48;
		}
		rabbitTableScrollView.scrollContent.anchoredPosition
			= new Vector2(-(wallNumber - 1) * 2160 - tableRabbitRectTransform[index].anchoredPosition.x + padding, 0);
	}

	// Use this for initialization
	void Start () {
		rabbitInfoScrollView = FindObjectOfType<RabbitInfoScrollView>();
		rabbitTableScrollView = FindObjectOfType<RabbitTableScrollView>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
