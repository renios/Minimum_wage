using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class StageSelectManager : MonoBehaviour {

	public GameObject missionPanel;
	public Image missionPanelBg;
	public Button openWorld2Button;
	public Image impossibleOpenWorld2PanelBg;
	public GameObject impossibleOpenWorld2Panel;
	public Text starCountTextAtImpossiblePanel;
	public Image possibleOpenWorld2PanelBg;
	public GameObject possibleOpenWorld2Panel;
	public Text starCountTextAtPossiblePanel;
	public Image openWorld2PanelBg;
	public GameObject openWorld2Panel;

	public void CheckAndPopupPanel() {
		int totalStars = FindObjectOfType<StarManager>().GetTotalStars();
		if (totalStars < 20) {
			ShowImpossibleOpenWorld2Panel();
		}
		else {
			ShowPossibleOpenWorld2Panel();
		}
	}

	public void ShowMissionPanel(int stageIndex) {
		SoundManager.Play(SoundType.Button);
		missionPanelBg.raycastTarget = true;
		MissionData.Initialize();
		Dictionary<MissionDataType, int> missionDataDict = MissionData.LoadMissionDataDict(stageIndex);
		MissionData.SetMissionData(stageIndex, missionDataDict);

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0.4f, delay);

		missionPanel.GetComponent<MissionPanel>().LoadMissonInfo();
	}

	public void HideMissonPanel() {
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0, delay);
		missionPanelBg.raycastTarget = false;
	}

	public void ShowImpossibleOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		impossibleOpenWorld2PanelBg.raycastTarget = true;
		
		// 현재 별 갯수 동기화
		starCountTextAtImpossiblePanel.text = FindObjectOfType<StarManager>().GetTotalStars().ToString() + " / 30"; 

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		impossibleOpenWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		impossibleOpenWorld2PanelBg.DOFade(0.4f, delay);
	}

	public void HideimpossibleOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		impossibleOpenWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		impossibleOpenWorld2PanelBg.DOFade(0, delay);
		impossibleOpenWorld2PanelBg.raycastTarget = false;
	}

	public void ShowPossibleOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		possibleOpenWorld2PanelBg.raycastTarget = true;
		
		// 현재 별 갯수 동기화
		starCountTextAtPossiblePanel.text = FindObjectOfType<StarManager>().GetTotalStars().ToString() + " / 30";

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		possibleOpenWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		possibleOpenWorld2PanelBg.DOFade(0.4f, delay);
	}

	public void HidePossibleOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		possibleOpenWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		possibleOpenWorld2PanelBg.DOFade(0, delay);
		possibleOpenWorld2PanelBg.raycastTarget = false;
	}

	public void OpenWorld2() {
		
	}

	public void ShowOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		openWorld2PanelBg.raycastTarget = true;

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		openWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		openWorld2PanelBg.DOFade(0.4f, delay);
	}

	public void HideOpenWorld2Panel() {
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		openWorld2Panel.GetComponent<RectTransform>().DOMove(endPos, delay);
		openWorld2PanelBg.DOFade(0, delay);
		openWorld2PanelBg.raycastTarget = false;
	}

	List<StageButton> stageButtons = new List<StageButton>();

	void Awake () {
		stageButtons = FindObjectsOfType<StageButton>().ToList();
		stageButtons.ForEach(button => button.Initialize());
		stageButtons.OrderBy(button => button.stageIndex);
		// PlayerPrefs.SetInt("Progress", 15);
		int progress = PlayerPrefs.GetInt("Progress", 1);
		int worldOpenProgress = PlayerPrefs.GetInt("WorldOpenProgress", 1);

		// Debug.Log("Current Progress : " + progress);

		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				if (button.stageIndex > 10 && worldOpenProgress >= 2) {
					button.Active();
				}
				else {
					button.Active();
				}
			}
		});
		
		SoundManager.Play(MusicType.Main);
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void ResetProgress() {
		Debug.Log("Progress reset to 1");
		PlayerPrefs.SetInt("Progress", 1);
		PlayerPrefs.SetInt("UnlockProgress", 1);
		PlayerPrefs.SetInt("WorldOpenProgress", 1);
		
		int progress = PlayerPrefs.GetInt("Progress", 1);

		FindObjectOfType<StarManager>().ResetTotalStars();

		stageButtons.ForEach(button => button.Inactive());
		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				button.Active();
			}
		});

		PlayerPrefs.SetInt("TimerReset", 0);
		PlayerPrefs.SetInt("Superfood", 0);
		PlayerPrefs.SetInt("TrayReset", 0);
	}

	public void BeforeOpenWorld2() {
		Debug.Log("Progress reset to 10");
		PlayerPrefs.SetInt("Progress", 10);
		PlayerPrefs.SetInt("UnlockProgress", 10);
		PlayerPrefs.SetInt("WorldOpenProgress", 1);
		
		int progress = PlayerPrefs.GetInt("Progress", 1);
		int worldOpenProgress = PlayerPrefs.GetInt("WorldOpenProgress", 1);

		for (int i = 1; i <= 10; i++) {
			PlayerPrefs.SetInt("StarsOfStage" + i.ToString(), 2);
		}

		FindObjectOfType<StarManager>().UpdateTotalStars();

		stageButtons.ForEach(button => button.Inactive());
		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				if (button.stageIndex > 10 && worldOpenProgress >= 2) {
					button.Active();
				}
				else {
					button.Active();
				}
			}
		});

		PlayerPrefs.SetInt("TimerReset", 2);
		PlayerPrefs.SetInt("Superfood", 2);
		PlayerPrefs.SetInt("TrayReset", 2);
	}

	// public void OpenWorld2() {
	// 	Debug.Log("Progress reset to 15");
	// 	PlayerPrefs.SetInt("Progress", 15);
	// 	PlayerPrefs.SetInt("UnlockProgress", 15);
	// 	PlayerPrefs.SetInt("WorldOpenProgress", 2);
		
	// 	int progress = PlayerPrefs.GetInt("Progress", 1);
	// 	int worldOpenProgress = PlayerPrefs.GetInt("WorldOpenProgress", 1);

	// 	for (int i = 1; i <= 10; i++) {
	// 		PlayerPrefs.SetInt("StarsOfStage" + i.ToString(), 2);
	// 	}

	// 	FindObjectOfType<StarManager>().UpdateTotalStars();

	// 	stageButtons.ForEach(button => button.Inactive());
	// 	stageButtons.ForEach(button => {
	// 		if (button.stageIndex <= progress) {
	// 			if (button.stageIndex > 10 && worldOpenProgress >= 2) {
	// 				button.Active();
	// 			}
	// 			else {
	// 				button.Active();
	// 			}
	// 		}
	// 	});
	// }
}
