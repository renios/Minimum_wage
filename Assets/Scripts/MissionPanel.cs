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
    public Text[] itemAmountTexts;
    // WorldItemPanel에 이미 있는 내용이라 중복이긴 함
    int maxItemAmount = 5;

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
        // resetTime 아이템 관련 판정
        if(resetTimeItem.isOn == true)
        {
            int itemAmount = PlayerPrefs.GetInt("TimerReset", 0);
            if(itemAmount < 1)
            {
                resetTimeItem.isOn = false;
                resetTimeImage.enabled = false;
            }
            else
                resetTimeImage.enabled = true;
        }
        else
            resetTimeImage.enabled = false;

        // superfood 아이템 관련 판정
        if (superfoodItem.isOn == true)
        {
            int itemAmount = PlayerPrefs.GetInt("Superfood", 0);
            if (itemAmount < 1)
            {
                superfoodItem.isOn = false;
                superfoodImage.enabled = false;
            }
            else
                superfoodImage.enabled = true;
        }
        else
            superfoodImage.enabled = false;

        // renewTray 아이템 관련 판정
        if (renewTrayItem.isOn == true)
        {
            int itemAmount = PlayerPrefs.GetInt("TrayReset", 0);
            if (itemAmount < 1)
            {
                renewTrayItem.isOn = false;
                renewTrayImage.enabled = false;
            }
            else
                renewTrayImage.enabled = true;
        }
        else
            renewTrayImage.enabled = false;
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

        // 아이템 갯수 텍스트
        for (int i = 0; i < itemAmountTexts.Length; i++)
        {
            int itemAmount = 0;

            switch (i)
            {
                case 0:
                    {
                        itemAmount = PlayerPrefs.GetInt("TimerReset", 0);
                    }
                    break;
                case 1:
                    {
                        itemAmount = PlayerPrefs.GetInt("Superfood", 0);
                    }
                    break;
                case 2:
                    {
                        itemAmount = PlayerPrefs.GetInt("TrayReset", 0);
                    }
                    break;
            }

            if (itemAmount == 0)
            {
                itemAmountTexts[i].text = "-";
            }
            else if (itemAmount == maxItemAmount)
            {
                itemAmountTexts[i].text = "MAX";
            }
            else
            {
                itemAmountTexts[i].text = itemAmount.ToString("N0");
            }
        }
    }
}
