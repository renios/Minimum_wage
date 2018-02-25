using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MissionPanel : MonoBehaviour {

	public GameObject world1;
	public GameObject world2;

	public Text todoText;
	public Text dayText;
	public Text world1customerText;
	public Text world1timeText;
	public Text world2customerText;
	public Text world2timeText;
	public Text world2touchText;
    public Toggle resetTimeItem;
    public Toggle superfoodItem;
    public Toggle renewTrayItem;
    public Image resetTimeImage;
    public Image superfoodImage;
    public Image renewTrayImage;

    private void Update()
    {
        resetTimeImage.enabled = (resetTimeItem.isOn == true) ? true : false;
        superfoodImage.enabled = (superfoodItem.isOn == true) ? true : false;
        renewTrayImage.enabled = (renewTrayItem.isOn == true) ? true : false;
    }

    public void LoadMissonInfo() {
		resetTimeItem.isOn = false;
        superfoodItem.isOn = false;
        renewTrayItem.isOn = false;

		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();

		int date = missionDataDict[MissionDataType.StageIndex];
		dayText.text = "DAY " + date;

		int maxHeart = missionDataDict[MissionDataType.maxHeart];
		string todoString = "1) "+ maxHeart + "명 이상의 손님을" + '\n' + "돌려보내지 않기";
		if (maxHeart == 1) {
			todoString = "1) 손님을 한명도 돌려보내지 않기";
		}
		int missionCount = 2;

		if (missionDataDict.ContainsKey(MissionDataType.customerCount) &&
			missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			int customerCount = missionDataDict[MissionDataType.customerCount];
			int remainTime = missionDataDict[MissionDataType.remainTime];
			string newTodoString = "" + '\n' + '\n' + missionCount + ") " + (remainTime / 60) + "분 " + (remainTime % 60) + "초 이내에 " + customerCount + "명 이상 서빙하기"; 
			todoString += newTodoString;
			missionCount++;
		}
		else if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
			int customerCount = missionDataDict[MissionDataType.customerCount];
			string newTodoString = "" + '\n' + '\n' + missionCount + ") " + customerCount + "명 이상 서빙하기"; 
			todoString += newTodoString;
			missionCount++;
		}
		else if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			int remainTime = missionDataDict[MissionDataType.remainTime];
			string newTodoString = "" + '\n' + '\n' + missionCount + ") " + (remainTime / 60) + "분 " + (remainTime % 60) + "초 동안 서빙 계속하기"; 
			todoString += newTodoString;
			missionCount++;
		}
		
		if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
			int touchCount = missionDataDict[MissionDataType.touchCount];
			string newTodoString = "" + '\n' + '\n' + missionCount + ") " + touchCount + "번 이내로 움직여 모든 손님을 서빙하기"; 
			todoString += newTodoString;
			missionCount++;
		}
		todoText.text = todoString;

		world1.SetActive(false);
		world2.SetActive(false);

		if (MissionData.stageIndex < 11) {
			world1.SetActive(true);

			if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
				world1customerText.text = "" + missionDataDict[MissionDataType.customerCount];
			}
			else {
				world1customerText.text = "--";
			}

			if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
				int remainTime = missionDataDict[MissionDataType.remainTime];
				world1timeText.text = "" + (remainTime / 60).ToString("D1") + ":" + (remainTime % 60).ToString("D2");
			}
			else {
				world1timeText.text = "--";
			}
		}
		else if (MissionData.stageIndex < 21) {
			world2.SetActive(true);

			if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
				world2customerText.text = "" + missionDataDict[MissionDataType.customerCount];
			}
			else {
				world2customerText.text = "--";
			}

			if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
				int remainTime = missionDataDict[MissionDataType.remainTime];
				world2timeText.text = "" + (remainTime / 60).ToString("D1") + ":" + (remainTime % 60).ToString("D2");
			}
			else {
				world2timeText.text = "--";
			}

			if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
				int touchCount = missionDataDict[MissionDataType.touchCount];
				world2touchText.text = "" + missionDataDict[MissionDataType.touchCount];
			}
			else {
				world2timeText.text = "--";
			}
		}
	}
}
