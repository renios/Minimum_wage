using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RabbitCatalogManager : MonoBehaviour {

	public GameObject catalogPanel;
	public RabbitInfoScrollView rabbitInfoScrollView;
	public RabbitTableScrollView rabbitTableScrollView;
	public List<RectTransform> tableRabbitRectTransform;

	public void ShowCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
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
		rabbitTableScrollView.scrollContent.anchoredPosition
			= new Vector2(tableRabbitRectTransform[index].parent.gameObject.GetComponent<RectTransform>().anchoredPosition.x, 0);
	}

	// Use this for initialization
	void Start () {
		rabbitInfoScrollView = FindObjectOfType<RabbitInfoScrollView>();
		rabbitTableScrollView = FindObjectOfType<RabbitTableScrollView>();
		tableRabbitRectTransform = new List<RectTransform>();
		GameObject[] catalogRabbitList = GameObject.FindGameObjectsWithTag("CatalogRabbit");
		foreach(var rabbit in catalogRabbitList)
		{
			tableRabbitRectTransform.Add(rabbit.GetComponent<RectTransform>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
