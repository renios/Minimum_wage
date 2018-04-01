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

	public void ShowMissionPanel(int stageIndex) {
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
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0, delay);
		missionPanelBg.raycastTarget = false;
	}

	List<StageButton> stageButtons = new List<StageButton>();

	void Awake () {
		stageButtons = FindObjectsOfType<StageButton>().ToList();
		stageButtons.ForEach(button => button.Initialize());
		stageButtons.OrderBy(button => button.stageIndex);
		// PlayerPrefs.SetInt("Progress", 15);
		int progress = PlayerPrefs.GetInt("Progress", 1);

		// Debug.Log("Current Progress : " + progress);

		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				button.Active();
			}
		});
		
		SoundManager.Play(MusicType.Main);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetProgress();
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			OpenWorld2();
		}

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

	public void ResetProgress() {
		Debug.Log("Progress reset to 1");
		PlayerPrefs.SetInt("Progress", 1);
		PlayerPrefs.SetInt("UnlockProgress", 1);
		
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

	public void OpenWorld2() {
		Debug.Log("Progress reset to 15");
		PlayerPrefs.SetInt("Progress", 15);
		PlayerPrefs.SetInt("UnlockProgress", 15);
		
		int progress = PlayerPrefs.GetInt("Progress", 1);

		stageButtons.ForEach(button => button.Inactive());
		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				button.Active();
			}
		});
	}
}
