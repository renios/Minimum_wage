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
	public Text coinText;

	float remainTime;
	int customerCount;
	public int successCustomerCount = 0;
	public int touchCount;
	public int currentTouchCount = 0;
	public int currentCoin;

	bool isUsedTime = false;
	public bool isUsedCustomerCount = false;
	public bool isUsedTouchCount = false;

	int currentStage;

	// 텍스트 애니메이션 관련 변수
	public int defaultFontSize;
	public int maxFontSize;
	public float animRate;

	GameManager gameManager;
	GameStateManager gameStateManager;
	ScoreManager scoreManager;

	void UpdateProgress() {
		int progress = PlayerPrefs.GetInt("Progress", -1);
		if (progress == currentStage) {
            foreach(var stage in MissionData.rewardingStage)
            {
                if(progress == stage)
                {
                    // HidePanel 코루틴에서 아이템 보상 패널을 작동시킬 수 있도록 명령 전달
                    gameManager.needsReward = true;
                    break;
                }
            }
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
		scoreManager = FindObjectOfType<ScoreManager>();

		LoadMissionData();

		int currentCoin = scoreManager.realScoreAmount;
		coinText.text = currentCoin.ToString();

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
		if (gameStateManager.gameState != GameState.Idle) yield break;

		// 손님 조건 체크
		if (isUsedCustomerCount && successCustomerCount >= customerCount/* && !gameManager.gameoverCanvas.activeInHierarchy*/) {
			gameStateManager.gameState = GameState.Result;
			yield return StartCoroutine(gameManager.ShowClearCanvas());
			UpdateProgress();
			gameStateManager.gameState = GameState.End;
		}
		
		// 시간 조건 체크
		else if (isUsedTime && remainTime <= 0/* && !gameManager.gameoverCanvas.activeInHierarchy && !gameManager.isPlayingAnim*/) {
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

		// 터치 조건 체크
		else if (isUsedTouchCount && currentTouchCount > touchCount/* && !gameManager.gameoverCanvas.activeInHierarchy*/) {
			gameStateManager.gameState = GameState.Result;
			yield return StartCoroutine(gameManager.ShowGameoverCanvas());
			gameStateManager.gameState = GameState.End;
		}
	}

    public IEnumerator TextAnimation(Text _text)
    {
        // 현재로서는 문제 되는 상황이 없는 듯 하고 + 변수를 너무 늘리게 될 것 같아
        // 이미 같은 코루틴이 돌고 있을 때 끊고 다시 시작하게 하는 코딩은 하지 않음
        while (_text.fontSize < maxFontSize)
        {
            _text.fontSize = (int)Mathf.Lerp(_text.fontSize, defaultFontSize * 2, animRate);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a * (1 - animRate));
            yield return null;
        }
        _text.fontSize = defaultFontSize;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
    }

    // Update is called once per frame
    void Update () {
		if (!gameManager.isPlaying) return;

		if (isUsedTime) {
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
