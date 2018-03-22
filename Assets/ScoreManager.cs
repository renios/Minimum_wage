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

	float scoreAmount = 0;
	float goalAmount = 0;

	int feverLevel = 0;

	int waitingTime = 30;

	int maxAmount = 50;

	readonly int comboCoef = 5;
	readonly int customerCoef = 2;

	void ActivePoint(Image point) {
		point.GetComponentInChildren<ParticleSystem>().Play();
		point.sprite = activeBunny;
		point.color = Color.yellow;
		feverLevel += 1;
	}

	void InactiveAllPoints() {
		checkPoint1.sprite = inactiveBunny;
		checkPoint1.color = Color.gray;
		checkPoint1.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint2.sprite = inactiveBunny;
		checkPoint2.color = Color.gray;
		checkPoint2.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint3.sprite = inactiveBunny;
		checkPoint3.color = Color.gray;
		checkPoint3.GetComponentInChildren<ParticleSystem>().Stop();

		feverLevel = 0;
	}

	public void AddFeverAmountByCustomer(Customer customer) {
		float amount = 0;
		amount = customer.remainWaitingTime / (float)waitingTime;
		amount *= customerCoef;
		AddScoreAmount(amount);
	}

	public void AddFeverAmountByCombo(int comboCount) {
		float amount = comboCount * comboCoef;
		AddScoreAmount(amount);
	}

	public void AddScoreAmount(float amount) {
		goalAmount += amount;
		Debug.Log("Score : " + scoreAmount + " -> " + goalAmount);
	}

	// Use this for initialization
	void Start () {
		scoreAmount = 0;
		feverLevel = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();

		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.waitingTime)) {
			waitingTime = MissionData.GetMissionDataDict()[MissionDataType.waitingTime];
		}
	}

	float amount = 0.5f;

	public void CheckFeverPoint() {
		if (scoreAmount > maxAmount/3f && feverLevel < 1) {
			ActivePoint(checkPoint1);
		}

		if (scoreAmount > (maxAmount*2)/3f && feverLevel < 2) {
			ActivePoint(checkPoint2);
		}

		if (scoreAmount >= maxAmount && feverLevel < 3) {
			ActivePoint(checkPoint3);
		}
	}

	// Update is called once per frame
	void Update () {
		if (scoreAmount < goalAmount) {
			scoreAmount += amount;
			bar.fillAmount = scoreAmount / maxAmount;
		}

		if (Input.GetKeyDown(KeyCode.G)) {
			AddScoreAmount(15);
		}

		CheckFeverPoint();
	}

	void Reset () {
		goalAmount = 0;
		scoreAmount = 0;
		feverLevel = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();
	}
}
