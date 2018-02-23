﻿using System.Collections;
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
		Dictionary<MissionDataType, int> missionDataDict = LoadMissionDataDict(stageIndex);
		MissionData.SetMissionData(stageIndex, missionDataDict);

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0.4f, delay);
		missionPanelBg.raycastTarget = true;

		missionPanel.GetComponent<MissionPanel>().LoadMissonInfo();
	}

	public void HideMissonPanel() {
		MissionData.Initialize();

		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0, delay);
		missionPanelBg.raycastTarget = false;
	}

	Dictionary<MissionDataType, int> LoadMissionDataDict(int stageIndex) {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();

		string stageName = ((stageIndex / 10) + 1).ToString() + "-" + (stageIndex % 10).ToString();

		if (stageName == "1-1") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 10);
		}
		else if (stageName == "1-2") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}

		return missionDataDict;
	}

	List<StageButton> stageButtons = new List<StageButton>();

	void Awake () {
		stageButtons = FindObjectsOfType<StageButton>().ToList();
		stageButtons.ForEach(button => button.Initialize());
		stageButtons.OrderBy(button => button.stageIndex);
// PlayerPrefs.SetInt("Progress", 1);
		int progress = PlayerPrefs.GetInt("Progress", -1);

		if (progress == -1) {
			PlayerPrefs.SetInt("Progress", 1);
		}

		// Debug.Log("Current Progress : " + progress);

		stageButtons.ForEach(button => {
			if (button.stageIndex <= progress) {
				button.Active();
			}
		});
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Debug.Log("Progress reset to 1");
			PlayerPrefs.SetInt("Progress", 1);
			
			int progress = PlayerPrefs.GetInt("Progress", -1);

			stageButtons.ForEach(button => button.Inactive());
			stageButtons.ForEach(button => {
				if (button.stageIndex <= progress) {
					button.Active();
				}
			});
		}
	}
}