using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MissionDataType {
	StageIndex,
	customerCount,
	remainTime, 
	touchCount,
	maxHeart,
	waitingTime,
	customerCooldown,
	maxCustomer,
	foodTypeCount
}

public static class MissionData {

	public static int stageIndex = -1;
	public static int customerCount = -1;
	public static int remainTime = -1;
	public static int touchCount = -1;
	public static int maxHeart = 3;
	public static int waitingTime = -1;
	public static int customerCooldown = -1;
	public static int maxCustomer = 4;
	public static int foodTypeCount = 4;

	public static void Initialize() {
		stageIndex = -1;
		customerCount = -1;
		remainTime = -1;
		touchCount = -1;
		maxHeart = 3;
		waitingTime = -1;
		customerCooldown = -1;
		maxCustomer = 4;
		foodTypeCount = 4;
	}

	public static void SetMissionData(int inputStageIndex, Dictionary<MissionDataType, int> missionDataDict) {
		stageIndex = inputStageIndex;
		
		if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
			customerCount = missionDataDict[MissionDataType.customerCount];
		}
		if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			remainTime = missionDataDict[MissionDataType.remainTime];
		}
		if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
			touchCount = missionDataDict[MissionDataType.touchCount];
		}
		if (missionDataDict.ContainsKey(MissionDataType.maxHeart)) {
			maxHeart = missionDataDict[MissionDataType.maxHeart];
		}
		if (missionDataDict.ContainsKey(MissionDataType.waitingTime)) {
			waitingTime = missionDataDict[MissionDataType.waitingTime];
		}
		if (missionDataDict.ContainsKey(MissionDataType.customerCooldown)) {
			customerCooldown = missionDataDict[MissionDataType.customerCooldown];
		}
		if (missionDataDict.ContainsKey(MissionDataType.maxCustomer)) {
			maxCustomer = missionDataDict[MissionDataType.maxCustomer];
		}
		if (missionDataDict.ContainsKey(MissionDataType.foodTypeCount)) {
			foodTypeCount = missionDataDict[MissionDataType.foodTypeCount];
		}
	}

	public static Dictionary<MissionDataType, int> GetMissionDataDict() {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();
		missionDataDict.Add(MissionDataType.StageIndex, stageIndex);
		if (customerCount != -1) {
			missionDataDict.Add(MissionDataType.customerCount, customerCount);
		}
		if (remainTime != -1) {
			missionDataDict.Add(MissionDataType.remainTime, remainTime);
		}
		if (touchCount != -1) {
			missionDataDict.Add(MissionDataType.touchCount, touchCount);
		}
		if (maxHeart != -1) {
			missionDataDict.Add(MissionDataType.maxHeart, maxHeart);
		}
		if (waitingTime != -1) {
			missionDataDict.Add(MissionDataType.waitingTime, waitingTime);
		}
		if (customerCooldown != -1) {
			missionDataDict.Add(MissionDataType.customerCooldown, customerCooldown);
		}
		if (maxCustomer != -1) {
			missionDataDict.Add(MissionDataType.maxCustomer, maxCustomer);
		}
		if (foodTypeCount != -1) {
			missionDataDict.Add(MissionDataType.foodTypeCount, foodTypeCount);
		}

		return missionDataDict;
	}

	public static Dictionary<MissionDataType, int> LoadMissionDataDict(int stageIndex) {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();

		string stageName = ((stageIndex / 10) + 1).ToString() + "-" + (stageIndex % 10).ToString();
		SoundManager.SetWorldIndex(stageIndex/10);

		if (stageName == "1-1") {
			// missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 10);
			missionDataDict.Add(MissionDataType.waitingTime, 30);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 2);
		}
		else if (stageName == "1-2") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.waitingTime, 30);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 2);
		}
		else if (stageName == "1-3") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 10);
			missionDataDict.Add(MissionDataType.waitingTime, 20);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "1-4") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 10);
			missionDataDict.Add(MissionDataType.waitingTime, 30);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);
			missionDataDict.Add(MissionDataType.maxHeart, 1);
			missionDataDict.Add(MissionDataType.maxCustomer, 2);
		}
		else if (stageName == "1-5") {
			missionDataDict.Add(MissionDataType.remainTime, 100);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.waitingTime, 20);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "1-6") {
			missionDataDict.Add(MissionDataType.remainTime, 150);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.waitingTime, 30);
			missionDataDict.Add(MissionDataType.customerCooldown, 5);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "1-7") {
			missionDataDict.Add(MissionDataType.remainTime, 60);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.waitingTime, 15);
			missionDataDict.Add(MissionDataType.customerCooldown, 7);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 2);
		}
		else if (stageName == "1-8") {
			missionDataDict.Add(MissionDataType.remainTime, 100);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.waitingTime, 60);
			missionDataDict.Add(MissionDataType.customerCooldown, 3);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "1-9") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.waitingTime, 15);
			missionDataDict.Add(MissionDataType.customerCooldown, 1);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);
			missionDataDict.Add(MissionDataType.maxHeart, 1);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "1-10") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.waitingTime, 20);
			missionDataDict.Add(MissionDataType.customerCooldown, 3);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);
			missionDataDict.Add(MissionDataType.maxHeart, 3);
			missionDataDict.Add(MissionDataType.maxCustomer, 4);
		}
		else if (stageName == "2-1") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-2") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-3") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-4") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-5") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-6") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-7") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-8") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-9") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}
		else if (stageName == "2-10") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
		}

		return missionDataDict;
	}
}
