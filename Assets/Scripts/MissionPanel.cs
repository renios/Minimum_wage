using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MissionPanel : MonoBehaviour {

    public GameObject[] worlds;
    public Text[] worldCustomerTexts;
    public Text[] worldTimeTexts;
    public Text[] worldTouchTexts;

	public Text todoText;
	public Text dayText;
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

        // 우선은 비활화
        foreach(var world in worlds)
        {
            world.SetActive(false);
        }

        // 에디터에서 오브젝트, 컴포넌트를 끼울 때 존재하는 월드 수만큼 다 만들어 주어야 하고, 뭔가가 빠지는 스테이지는 그냥 비운 채로 둔다.
        int worldKey = ((MissionData.stageIndex - 1) / 10);

        // 해당 월드 활성화
        worlds[worldKey].SetActive(true);

        // 텍스트 수정
        if (missionDataDict.ContainsKey(MissionDataType.customerCount))
            worldCustomerTexts[worldKey].text = "" + missionDataDict[MissionDataType.customerCount];
        else
            worldCustomerTexts[worldKey].text = "--";

        if (missionDataDict.ContainsKey(MissionDataType.remainTime))
        {
            int remainTime = missionDataDict[MissionDataType.remainTime];
            worldTimeTexts[worldKey].text = "" + (remainTime / 60).ToString("D1") + ":" + (remainTime % 60).ToString("D2");
        }
        else
            worldTimeTexts[worldKey].text = "--";

        if (worldKey > 1) {
            if (missionDataDict.ContainsKey(MissionDataType.touchCount))
            {
                int touchCount = missionDataDict[MissionDataType.touchCount];
                worldTouchTexts[worldKey].text = "" + missionDataDict[MissionDataType.touchCount];
            }
            else
                worldTouchTexts[worldKey].text = "--";
        }
    }
}
