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

	int visualScoreAmount = 0;
	public int realScoreAmount = 0;

	public int numberOfStars = 0;

	int starTrigger3 = 0;

	readonly int defaultCoin = 150;
	readonly int comboCoef = 50;
	readonly int customerCoef = 50;

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
		InactiveAllPoints();

		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.starTrigger3)) {
			starTrigger3 = MissionData.GetMissionDataDict()[MissionDataType.starTrigger3];
		}
	}

	int amount = 1;

	public void CheckStarPoint() {
		if (visualScoreAmount > starTrigger3/3f && numberOfStars < 1) {
			ActivePoint(checkPoint1);
		}

		if (visualScoreAmount > (starTrigger3*2)/3f && numberOfStars < 2) {
			ActivePoint(checkPoint2);
		}

		if (visualScoreAmount >= starTrigger3 && numberOfStars < 3) {
			ActivePoint(checkPoint3);
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 10; i++){
			if (visualScoreAmount < realScoreAmount) {
				visualScoreAmount += amount;
				bar.fillAmount = visualScoreAmount / (float)starTrigger3;
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
