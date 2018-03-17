using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MissionManager : MonoBehaviour {

	public Image timeImage;
	public Image customerImage;
	public Image touchImage;

	public Text timeText;
	public Text customerText;
	public Text touchText;

	float remainTime;
	int customerCount;
	public int successCustomerCount = 0;
	int touchCount;
	public int currentTouchCount = 0;

	bool isUsedTime = false;
	bool isUsedCustomerCount = false;
	bool isUsedTouchCount = false;

	int currentStage;

	GameManager gameManager;
	GameStateManager gameStateManager;

	void UpdateProgress() {
		int progress = PlayerPrefs.GetInt("Progress", -1);
		if (progress == currentStage) {
			int newProgress = progress + 1;
			PlayerPrefs.SetInt("Progress", newProgress);
			// Debug.Log("Progress change : " + progress + "->" + newProgress);
		}
	}

	void SetDefaultValue() {
		currentStage = 1;

		isUsedTime = false;

		isUsedCustomerCount = false;

		isUsedTouchCount = false;
	}

	void LoadMissionData() {
		SetDefaultValue();
		
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();

		currentStage = missionDataDict[MissionDataType.StageIndex];

		if (missionDataDict.ContainsKey(MissionDataType.customerCount)) {
			customerCount = missionDataDict[MissionDataType.customerCount];
			isUsedCustomerCount = true;
		}
		if (missionDataDict.ContainsKey(MissionDataType.remainTime)) {
			remainTime = missionDataDict[MissionDataType.remainTime];
			isUsedTime = true;
		}
		if (missionDataDict.ContainsKey(MissionDataType.touchCount)) {
			touchCount = missionDataDict[MissionDataType.touchCount];
			isUsedTouchCount = true;
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		gameStateManager = FindObjectOfType<GameStateManager>();

		LoadMissionData();

		if (isUsedTime) {
			timeText.text = ((int)(remainTime / 60)).ToString("D2") + ":" + ((int)(remainTime % 60)).ToString("D2");
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

		if (isUsedTouchCount) {
			touchText.text = currentTouchCount + "/" + touchCount;
		}
		else {
			touchImage.enabled = false;
			touchText.enabled = false;
		}
	}
	
	public IEnumerator CheckGameEnd() {
		// 시간 조건 체크
		if (isUsedTime && remainTime <= 0/* && !gameManager.gameoverCanvas.activeInHierarchy && !gameManager.isPlayingAnim*/) {
			gameStateManager.gameState = GameState.Result;
			// 버티기 미션일 경우 시간이 다 떨어졌을 때 게임 오버가 되는 대신 게임 클리어가 됨
			if (!isUsedCustomerCount) {
				yield return StartCoroutine(gameManager.ShowClearCanvas());
				UpdateProgress();
			}
			else {
				yield return StartCoroutine(gameManager.ShowGameoverCanvas());
			}
			gameStateManager.gameState = GameState.End;
		}

		// 손님 조건 체크
		if (isUsedCustomerCount && successCustomerCount >= customerCount/* && !gameManager.gameoverCanvas.activeInHierarchy*/) {
			gameStateManager.gameState = GameState.Result;
			yield return StartCoroutine(gameManager.ShowClearCanvas());
			UpdateProgress();
			gameStateManager.gameState = GameState.End;
		}

		// 터치 조건 체크
		if (isUsedTouchCount && currentTouchCount > touchCount/* && !gameManager.gameoverCanvas.activeInHierarchy*/) {
			gameStateManager.gameState = GameState.Result;
			yield return StartCoroutine(gameManager.ShowGameoverCanvas());
			gameStateManager.gameState = GameState.End;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		if (isUsedTime) {
			// if (gameStateManager.gameState == GameState.Idle || gameStateManager.gameState == GameState.Picked)
			remainTime -= Time.deltaTime;	
			timeText.text = ((int)(remainTime / 60)).ToString("D2") + ":" + ((int)(remainTime % 60)).ToString("D2");
		}

		if (isUsedCustomerCount) {
			customerText.text = successCustomerCount + "/" + customerCount;
		}

		if (isUsedTouchCount) {
			touchText.text = currentTouchCount + "/" + touchCount;
		}
		
		// 테스트용 클리어 치트
		if (Input.GetKeyDown(KeyCode.C)) {
			if (gameStateManager.gameState == GameState.Idle) {
				gameStateManager.gameState = GameState.Result;
				StartCoroutine(gameManager.ShowClearCanvas());
				UpdateProgress();
				gameStateManager.gameState = GameState.End;
			}
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			if (gameStateManager.gameState == GameState.Idle) {
				gameStateManager.gameState = GameState.Result;
				StartCoroutine(gameManager.ShowGameoverCanvas());
				gameStateManager.gameState = GameState.End;
			}
		}
	}
}
