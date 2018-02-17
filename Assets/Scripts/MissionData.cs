using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionDataType {
	customerCount,
	remainTime, 
	touchCount
}

public static class MissionData {

	public static string stageName = "";
	public static int customerCount = -1;
	public static int remainTime = -1;
	public static int touchCount = -1;

	public static void Initialize() {
		stageName = "";
		customerCount = -1;
		remainTime = -1;
		touchCount = -1;
	}

	public static void SetMissionData(string name, Dictionary<MissionDataType, int> missionDataDict) {
		stageName = name;
		if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
			customerCount = missionDataDict[MissionDataType.customerCount];
		}
		if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			remainTime = missionDataDict[MissionDataType.remainTime];
		}
		if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
			touchCount = missionDataDict[MissionDataType.touchCount];
		}
	}

	public static Dictionary<MissionDataType, int> GetMissionDataDict() {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();
		if (customerCount != -1) {
			missionDataDict.Add(MissionDataType.customerCount, customerCount);
		}
		if (remainTime != -1) {
			missionDataDict.Add(MissionDataType.remainTime, remainTime);
		}
		if (touchCount != -1) {
			missionDataDict.Add(MissionDataType.touchCount, touchCount);
		}

		return missionDataDict;
	}
}
