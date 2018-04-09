using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Image bar;
	public Image checkPoint1;
	public Image checkPoint2;
	public Image checkPoint3;

	public Sprite inactiveBunny;
	public Sprite activeBunny;

	public Transform leftPivot;
	public Transform rightPivot;

	int visualScoreAmount = 0;
	public int realScoreAmount = 0;

	public int numberOfStars = 0;

	Dictionary<int, int> starTriggers;

	readonly int defaultCoin = 150;
	readonly int comboCoef = 50;
	readonly int customerCoef = 50;

	void SetCheckPointPos() {
		float deltaDist = rightPivot.position.x - leftPivot.position.x;
		float checkPoint1PosX = leftPivot.position.x + 
								(deltaDist * (starTriggers[1]/(float)starTriggers[3]));
		checkPoint1.transform.position = new Vector3(checkPoint1PosX, checkPoint1.transform.position.y, checkPoint1.transform.position.z);
		float checkPoint2PosX = leftPivot.position.x + 
								(deltaDist * (starTriggers[2]/(float)starTriggers[3]));
		checkPoint2.transform.position = new Vector3(checkPoint2PosX, checkPoint2.transform.position.y, checkPoint2.transform.position.z);
	}

	void ActivePoint(Image point) {
		point.GetComponentInChildren<ParticleSystem>().Play();
		point.sprite = activeBunny;
		numberOfStars += 1;
	}

	void InactiveAllPoints() {
		checkPoint1.sprite = inactiveBunny;
		checkPoint1.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint2.sprite = inactiveBunny;
		checkPoint2.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint3.sprite = inactiveBunny;
		checkPoint3.GetComponentInChildren<ParticleSystem>().Stop();

		numberOfStars = 0;
	}

	public void AddScoreAmount(int amount){
		realScoreAmount += amount;
		Debug.Log("Score : " + visualScoreAmount + " -> " + realScoreAmount);
	}
	public void AddScore(int comboCount = -1, float waitingTime = -1) {
		realScoreAmount += defaultCoin;
		if(comboCount >= 0){
			realScoreAmount += comboCount * comboCoef;
		}
		if(waitingTime >= 0){
			realScoreAmount += (int)(waitingTime * customerCoef + 0.999f);
		}
		Debug.Log("Score : " + visualScoreAmount + " -> " + realScoreAmount);
	}

	// Use this for initialization
	void Start () {
		visualScoreAmount = 0;
		numberOfStars = 0;
		bar.fillAmount = 0;
		starTriggers = new Dictionary<int, int>();
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.starTrigger3)) {
			starTriggers[3] = MissionData.GetMissionDataDict()[MissionDataType.starTrigger3];
		}
		if (missionDataDict.ContainsKey(MissionDataType.starTrigger2)) {
			starTriggers[2] = MissionData.GetMissionDataDict()[MissionDataType.starTrigger2];
		}
		if (missionDataDict.ContainsKey(MissionDataType.starTrigger1)) {
			starTriggers[1] = MissionData.GetMissionDataDict()[MissionDataType.starTrigger1];
		}

		// 임시변수 예외처리
		if (starTriggers[2] > starTriggers[3]) starTriggers[2] = starTriggers[3];
		if (starTriggers[1] > starTriggers[2]) starTriggers[1] = starTriggers[2];

		SetCheckPointPos();
		InactiveAllPoints();

	}

	int amount = 1;

	public void CheckStarPoint() {
		if (visualScoreAmount > starTriggers[1] && numberOfStars < 1) {
			ActivePoint(checkPoint1);
		}

		if (visualScoreAmount > starTriggers[2] && numberOfStars < 2) {
			ActivePoint(checkPoint2);
		}

		if (visualScoreAmount >= starTriggers[3] && numberOfStars < 3) {
			ActivePoint(checkPoint3);
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 10; i++){
			if (visualScoreAmount < realScoreAmount) {
				visualScoreAmount += amount;
				bar.fillAmount = visualScoreAmount / (float)starTriggers[3];
			}
		}

		if (Input.GetKeyDown(KeyCode.G)) {
			AddScoreAmount(15);
		}

		CheckStarPoint();
	}

	void Reset () {
		realScoreAmount = 0;
		visualScoreAmount = 0;
		numberOfStars = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();
	}
}
