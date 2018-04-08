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
	foodTypeCount,
	starTrigger1,
	starTrigger2,
	starTrigger3
}

public static class MissionData {

	public static int stageIndex = 1;
	public static int customerCount = -1;
	public static int remainTime = -1;
	public static int touchCount = -1;
	public static int foodTypeCount = 4;
	public static bool gotTimeItem = false;
	public static bool gotSuperfood = false;
	public static bool gotTrayItem = false;
	public static int starTrigger1 = 200;
	public static int starTrigger2 = 300;
	public static int starTrigger3 = 500;
    // 아이템 보상을 주는 스테이지
    public static int[] rewardingStage = { 1, 3, 5, 7, 9,
                                        11, 13, 15, 17, 19};
    public static bool fromStage = false;

	// 상수값
	public static int maxHeart = 3;
	public static int waitingTime = 30;
	public static int customerCooldown = 1;
	public static int maxCustomer = 4;

	public static void Initialize() {
		stageIndex = 1;
		customerCount = -1;
		remainTime = -1;
		touchCount = -1;
		foodTypeCount = 4;

		maxHeart = 3;
		waitingTime = 30;
		customerCooldown = 1;
		maxCustomer = 4;

		starTrigger1 = 200;
		starTrigger2 = 300;
		starTrigger3 = 500;
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
		if (missionDataDict.ContainsKey(MissionDataType.foodTypeCount)) {
			foodTypeCount = missionDataDict[MissionDataType.foodTypeCount];
		}

		if (missionDataDict.ContainsKey(MissionDataType.maxHeart)) {
			maxHeart = missionDataDict[MissionDataType.maxHeart];
		}

		starTrigger1 = missionDataDict[MissionDataType.starTrigger1];
		starTrigger2 = missionDataDict[MissionDataType.starTrigger2];
		starTrigger3 = missionDataDict[MissionDataType.starTrigger3];

		waitingTime = missionDataDict[MissionDataType.waitingTime];
		customerCooldown = missionDataDict[MissionDataType.customerCooldown];
		maxCustomer = missionDataDict[MissionDataType.maxCustomer];
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
		if (foodTypeCount != -1) {
			missionDataDict.Add(MissionDataType.foodTypeCount, foodTypeCount);
		}

		missionDataDict.Add(MissionDataType.starTrigger1, starTrigger1);
		missionDataDict.Add(MissionDataType.starTrigger2, starTrigger2);
		missionDataDict.Add(MissionDataType.starTrigger3, starTrigger3);

		missionDataDict.Add(MissionDataType.maxHeart, maxHeart);
		missionDataDict.Add(MissionDataType.waitingTime, waitingTime);
		missionDataDict.Add(MissionDataType.customerCooldown, customerCooldown);
		missionDataDict.Add(MissionDataType.maxCustomer, maxCustomer);

		return missionDataDict;
	}

	public static Dictionary<MissionDataType, int> SetDefaultValues(Dictionary<MissionDataType, int> missionDataDict) {
		missionDataDict.Add(MissionDataType.starTrigger1, starTrigger1);
		missionDataDict.Add(MissionDataType.starTrigger2, starTrigger2);
		missionDataDict.Add(MissionDataType.starTrigger3, starTrigger3);

		missionDataDict.Add(MissionDataType.maxHeart, maxHeart);
		missionDataDict.Add(MissionDataType.waitingTime, waitingTime);
		missionDataDict.Add(MissionDataType.customerCooldown, customerCooldown);
		missionDataDict.Add(MissionDataType.maxCustomer, maxCustomer);

		return missionDataDict;
	}

	public static Dictionary<MissionDataType, int> LoadMissionDataDict(int stageIndex) {
		Dictionary<MissionDataType, int> missionDataDict = new Dictionary<MissionDataType, int>();

		missionDataDict = SetDefaultValues(missionDataDict);

		int world = ((stageIndex-1) / 10) + 1;
		int stage = (stageIndex % 10);
		if (stage == 0) stage = 10;
		string stageName = world + "-" + stage;
		SoundManager.SetWorldIndex(world-1);
		Debug.Log(stageName);

		if (stageName == "1-1") {
			// missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);

			missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13000;
		}
		else if (stageName == "1-2") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);

            // 임의로 15 넣음
            missionDataDict[MissionDataType.starTrigger1] = 1200; 			
			missionDataDict[MissionDataType.starTrigger2] = 1500; 			
			missionDataDict[MissionDataType.starTrigger3] = 11200;
        }
		else if (stageName == "1-3") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13300;
        }
		else if (stageName == "1-4") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 2000; 			
			missionDataDict[MissionDataType.starTrigger2] = 2300; 			
			missionDataDict[MissionDataType.starTrigger3] = 12300;
        }
		else if (stageName == "1-5") {
			//missionDataDict.Add(MissionDataType.remainTime, 100);
			missionDataDict.Add(MissionDataType.customerCount, 25);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 5000; 			
			missionDataDict[MissionDataType.starTrigger2] = 5300; 			
			missionDataDict[MissionDataType.starTrigger3] = 15300;
        }
		else if (stageName == "1-6") {
			//missionDataDict.Add(MissionDataType.remainTime, 150);
			missionDataDict.Add(MissionDataType.customerCount, 30);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);

            missionDataDict[MissionDataType.starTrigger1] = 6000; 			
			missionDataDict[MissionDataType.starTrigger2] = 6300; 			
			missionDataDict[MissionDataType.starTrigger3] = 16300;
        }
		else if (stageName == "1-7") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            // 임의로 15 넣음
            missionDataDict[MissionDataType.starTrigger1] = 4000; 			
			missionDataDict[MissionDataType.starTrigger2] = 4300; 			
			missionDataDict[MissionDataType.starTrigger3] = 14300;
        }
		else if (stageName == "1-8") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 4000; 			
			missionDataDict[MissionDataType.starTrigger2] = 4300; 			
			missionDataDict[MissionDataType.starTrigger3] = 14300;
        }
		else if (stageName == "1-9") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.maxHeart, 1);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13300;
        }
		else if (stageName == "1-10") {
			missionDataDict.Add(MissionDataType.remainTime, 120);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            // 임의로 15 넣음
            missionDataDict[MissionDataType.starTrigger1] = 1800; 			
			missionDataDict[MissionDataType.starTrigger2] = 2100; 			
			missionDataDict[MissionDataType.starTrigger3] = 12100;
        }
		else if (stageName == "2-1") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 10);
			missionDataDict.Add(MissionDataType.touchCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);

            missionDataDict[MissionDataType.starTrigger1] = 2000; 			
			missionDataDict[MissionDataType.starTrigger2] = 2300; 			
			missionDataDict[MissionDataType.starTrigger3] = 12300;
        }
		else if (stageName == "2-2") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.touchCount, 20);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13300;
        }
		else if (stageName == "2-3") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			//missionDataDict.Add(MissionDataType.customerCount, 15);
			//missionDataDict.Add(MissionDataType.touchCount, 20);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 1200; 			
			missionDataDict[MissionDataType.starTrigger2] = 1500; 			
			missionDataDict[MissionDataType.starTrigger3] = 11500;
        }
		else if (stageName == "2-4") {
			//missionDataDict.Add(MissionDataType.remainTime, 150);
			missionDataDict.Add(MissionDataType.customerCount, 20);
			missionDataDict.Add(MissionDataType.touchCount, 30);
			missionDataDict.Add(MissionDataType.foodTypeCount, 5);

            missionDataDict[MissionDataType.starTrigger1] = 4000; 			
			missionDataDict[MissionDataType.starTrigger2] = 4300; 			
			missionDataDict[MissionDataType.starTrigger3] = 14300;
        }
		else if (stageName == "2-5") {
			//missionDataDict.Add(MissionDataType.remainTime, 60);
			missionDataDict.Add(MissionDataType.customerCount, 25);
			missionDataDict.Add(MissionDataType.touchCount, 35);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 5000; 			
			missionDataDict[MissionDataType.starTrigger2] = 5300; 			
			missionDataDict[MissionDataType.starTrigger3] = 15300;
        }
		else if (stageName == "2-6") {
			missionDataDict.Add(MissionDataType.remainTime, 75);
			missionDataDict.Add(MissionDataType.customerCount, 25);
			missionDataDict.Add(MissionDataType.touchCount, 25);
			missionDataDict.Add(MissionDataType.foodTypeCount, 4);

            missionDataDict[MissionDataType.starTrigger1] = 5000; 			
			missionDataDict[MissionDataType.starTrigger2] = 5300; 			
			missionDataDict[MissionDataType.starTrigger3] = 15300;
        }
		else if (stageName == "2-7") {
			missionDataDict.Add(MissionDataType.remainTime, 60);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.touchCount, 25);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13300;
        }
		else if (stageName == "2-8") {
			missionDataDict.Add(MissionDataType.remainTime, 90);
			// missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.touchCount, 15);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            // 임의로 15 넣음
            missionDataDict[MissionDataType.starTrigger1] = 1400; 			
			missionDataDict[MissionDataType.starTrigger2] = 1700; 			
			missionDataDict[MissionDataType.starTrigger3] = 11400;
        }
		else if (stageName == "2-9") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 15);
			missionDataDict.Add(MissionDataType.touchCount, 15);
			missionDataDict.Add(MissionDataType.maxHeart, 1);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 3000; 			
			missionDataDict[MissionDataType.starTrigger2] = 3300; 			
			missionDataDict[MissionDataType.starTrigger3] = 13300;
        }
		else if (stageName == "2-10") {
			// missionDataDict.Add(MissionDataType.remainTime, 120);
			missionDataDict.Add(MissionDataType.customerCount, 30);
			missionDataDict.Add(MissionDataType.touchCount, 35);
			missionDataDict.Add(MissionDataType.foodTypeCount, 6);

            missionDataDict[MissionDataType.starTrigger1] = 6000; 			
			missionDataDict[MissionDataType.starTrigger2] = 6300; 			
			missionDataDict[MissionDataType.starTrigger3] = 16300;
        }

		return missionDataDict;
	}
}
