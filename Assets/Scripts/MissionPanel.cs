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

	public void LoadMissonInfo() {
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
