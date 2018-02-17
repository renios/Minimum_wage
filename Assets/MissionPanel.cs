using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MissionPanel : MonoBehaviour {

	public Text todoText;
	public Text dayText;
	public Text customerText;
	public Text timeText;

	public void LoadMissonInfo(string stageName) {
		string[] parsedStageName = stageName.Split('-');
		int date = Convert.ToInt32(parsedStageName[0]) + Convert.ToInt32(parsedStageName[1]) -1;
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();

		dayText.text = "DAY " + date;

		string todoString = "1) 3명 이상의 손님을" + '\n' + "돌려보내지 않기";
		int missionCount = 2;
		if (missionDataDict.ContainsKey(MissionDataType.customerCount) &&
			missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			int customerCount = missionDataDict[MissionDataType.customerCount];
			int remainTime = missionDataDict[MissionDataType.remainTime];
			string newTodoString = "" + '\n' + '\n' + missionCount + ") " + remainTime + "초 이내에" + '\n' 
								+ customerCount + "명 이상 서빙하기"; 
			todoString += newTodoString;
			missionCount++;
		}
		// if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
		// 	touchCount = missionDataDict[MissionDataType.touchCount];
		// }
		todoText.text = todoString;

		if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
			customerText.text = ": " + missionDataDict[MissionDataType.customerCount] + "+";
		}
		else {
			customerText.text = ": --";
		}

		if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			int remainTime = missionDataDict[MissionDataType.remainTime];
			timeText.text = ": " + (remainTime / 60).ToString("D2") + ":" + (remainTime % 60).ToString("D2");
		}
		else {
			timeText.text = ": --";
		}
	}
}
