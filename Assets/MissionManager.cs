using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {

	public Image timeImage;
	public Image customerImage;

	public Text timeText;
	public Text customerText;

	float remainTime;
	int customerCount;
	int successCustomerCount = 0;

	bool isUsedTime = false;
	bool isUsedCustomerCount = false;

	GameManager gameManager;

	void SetDefaultValue() {
		remainTime = 90;
		isUsedTime = true;

		customerCount = 20;
		isUsedCustomerCount = true;
	}

	void LoadMissionData() {
		if (MissionData.stageName == "") {
			SetDefaultValue();
		}
		else {
			Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();

			if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
				customerCount = missionDataDict[MissionDataType.customerCount];
				isUsedCustomerCount = true;
			}
			if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
				remainTime = missionDataDict[MissionDataType.remainTime];
				isUsedTime = true;
			}
			// if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
			// 	touchCount = missionDataDict[MissionDataType.touchCount];
			// }
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();

		LoadMissionData();

		if (isUsedTime) {
			timeText.text = (int)(remainTime / 60) + ":" + ((int)(remainTime % 60)).ToString("D2");
		}
		else {
			timeText.text = "--:--";
		}

		if (isUsedCustomerCount) {
			customerText.text = successCustomerCount + "/" + customerCount;
		}
		else {
			customerText.text = "--/--";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		if (isUsedTime) {
			remainTime -= Time.deltaTime;	
			timeText.text = (int)(remainTime / 60) + ":" + ((int)(remainTime % 60)).ToString("D2");

			if (remainTime <= 0 && !gameManager.gameoverCanvas.activeInHierarchy) {
				StartCoroutine(gameManager.ShowGameoverCanvas());
			}
		}

		if (isUsedCustomerCount) {
			customerText.text = successCustomerCount + "/" + customerCount;

			if (successCustomerCount == customerCount && !gameManager.gameoverCanvas.activeInHierarchy) {
				StartCoroutine(gameManager.ShowClearCanvas());
			}
		}
		
	}
}
