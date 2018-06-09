using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RabbitCatalogManager : MonoBehaviour {

	public GameObject catalogPanel;
	public GameObject catalogButton;
	public RabbitInfoScrollView rabbitInfoScrollView;
	public RectTransform[] tableRabbitRectTransform;
	public List<GameObject> catalogCells;

	public void ShowCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		rabbitInfoScrollView.Initialize();
		Initialize();
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
	}

	void Initialize()
	{
		int unlockProgress = PlayerPrefs.GetInt("UnlockProgress", 1);
		
		if (unlockProgress < 2)
			catalogButton.SetActive(false);
		else
			catalogButton.SetActive(true);

		catalogCells.ForEach(cell => cell.SetActive(false));
		
		if (unlockProgress >= 1)
			catalogCells[0].SetActive(true);
		if (unlockProgress >= 2)
			catalogCells[1].SetActive(true);
		if (unlockProgress >= 3)
			catalogCells[2].SetActive(true);
		if (unlockProgress >= 4)
			catalogCells[3].SetActive(true);
		if (unlockProgress >= 5)
			catalogCells[4].SetActive(true);
		if (unlockProgress >= 7)
			catalogCells[5].SetActive(true);
		if (unlockProgress >= 9)
			catalogCells[6].SetActive(true);
		if (unlockProgress >= 12)
			catalogCells[7].SetActive(true);
		if (unlockProgress >= 13)
			catalogCells[8].SetActive(true);
		if (unlockProgress >= 14)
			catalogCells[9].SetActive(true);
		if (unlockProgress >= 16)
			catalogCells[10].SetActive(true);
	}

	// Use this for initialization
	void Start () {
		rabbitInfoScrollView = FindObjectOfType<RabbitInfoScrollView>();
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
