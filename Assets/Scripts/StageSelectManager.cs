using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectManager : MonoBehaviour {

	public GameObject missionPanel;
	public Image missionPanelBg;

	public string selectedStageName = "";

	public void ShowMissionPanel(string stageName) {
		Dictionary<MissionDataType, int> missionDataDict = LoadMissionDataDict(stageName);
		MissionData.SetMissionData(stageName, missionDataDict);

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0.4f, delay);
		missionPanelBg.raycastTarget = true;

		missionPanel.GetComponent<MissionPanel>().LoadMissonInfo(stageName);
	}

	public void HideMissonPanel() {
		MissionData.Initialize();

		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		missionPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		missionPanelBg.DOFade(0, delay);
		missionPanelBg.raycastTarget = false;
	}

	Dictionary<MissionDataType, int> LoadMissionDataDict(string stageName) {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();

		if (stageName == "1-1") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "1-2") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 30);
		}

		return missionDataDict;
	} 
}
