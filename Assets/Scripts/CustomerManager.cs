﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour {

	Dictionary<int, Rabbit> openedRabbitDict;

	public GameObject customerPrefab;
	public List<Transform> customerSlot;
	public float waitingTime;
    public float toleranceRate;
    public float maxFuryRate;
	public float customerCooldown;
	float lastCustomerMakeTime;

	public GameObject coinPrefab;
	public GameObject successEffectPrefab;
	public GameObject resetWaitingTimeEffectPrefab;

	public Customer[] currentWaitingCustomers;

	public bool isPlayingCustomerAnim = false;

	// 코인 관련 수치
	int defaultCoin = 100;
	int coinCoef = 20;

	GameManager gameManager;
	GameStateManager gameStateManager;
	HeartManager heartManager;
	TrayManager trayManager;
	MissionManager missionManager;
	ScoreManager scoreManager;
	public TestManager testManager;
	RabbitGroupOrder groupOrder;
	TutorialManager tutorialManager;

	public void ResetFoundCorrespondentEachOrder() {
		var customers = currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		customers.ForEach(customer => {
			customer.orderedFoods.ForEach(food => food.foundCorrespondent = false);
		});
	}

	public void ResetWaitingTime()
	{
		if (MissionData.gotTimeItem == true)
		{
			foreach (var customer in currentWaitingCustomers)
			{
				if (customer != null) {
					Vector3 startPos = Vector3.up + customer.GetComponent<Customer>().customerImage.GetComponent<RectTransform>().position;
					Instantiate(resetWaitingTimeEffectPrefab, startPos, Quaternion.identity);
					customer.GetComponent<Customer>().remainWaitingTime = customer.GetComponent<Customer>().waitingTime;
					if(customer.startedFury)
					{
						customer.startedFury = false;
						customer.customerImage.transform.localPosition = customer.customerImageOriginPos;
						customer.timerImage.color = new Color(131f / 255f, 193f / 255f, 193f / 255f, 1f);
					}
				}
			}
		}
		MissionData.gotTimeItem = false;
	}

	public void RemoveCustomerByTimeout(int indexInArray) {
		if (tutorialManager != null && 
		    (tutorialManager.tutorialStep == 14 || tutorialManager.tutorialStep == 18)) {
			tutorialManager.tutorialStep += 1;
		}

		Customer customer = currentWaitingCustomers[indexInArray];
		if (customer.rabbitData.isVip) {
			heartManager.ReduceAllHearts();
		}
		else {
			heartManager.ReduceHeart(customer.rabbitData.reduceHeartsByFail);
		}
		Destroy(customer.gameObject);
		currentWaitingCustomers[indexInArray] = null;
	}

	void MakeCoinParticle(Vector3 pos, float delay) {
		Vector3 prefabPos = pos + Vector3.down/2f;
		Instantiate(successEffectPrefab, prefabPos, Quaternion.identity);
		Instantiate(coinPrefab, prefabPos, Quaternion.identity);
	}

	void AddCoinAmount(float waitingTime) {
		int comboCount = trayManager.comboCount;
		scoreManager.AddScore(comboCount, waitingTime);
		SoundManager.Play(SoundType.Cashier);
		missionManager.coinText.text = scoreManager.realScoreAmount.ToString();
		StartCoroutine(missionManager.TextAnimation(missionManager.coinText));
	}

	public void RemoveCustomerByMatching(int indexInArray, float delay) {
		var currentWaitingTime = currentWaitingCustomers[indexInArray].GetRateOfWatingTime();
		currentWaitingCustomers[indexInArray].isServeCompleted = true;

		MakeCoinParticle(currentWaitingCustomers[indexInArray].transform.position, delay);

		Destroy(currentWaitingCustomers[indexInArray].gameObject, delay);
		currentWaitingCustomers[indexInArray] = null;
		missionManager.successCustomerCount += 1;
		StartCoroutine(missionManager.TextAnimation(missionManager.customerText));
		AddCoinAmount(currentWaitingTime);

		if (tutorialManager != null && tutorialManager.tutorialStep == 18) {
			tutorialManager.tutorialStep += 1;
		}
	}

	bool IsThereSameIndexCustomer(int rabbitIndex) {
		bool isThere = false;
		currentWaitingCustomers.ToList().ForEach(customer => {
			if (customer != null) {
				if (customer.rabbitData.index == rabbitIndex)
					isThere = true;
			}
		});
		return isThere;
	}

	public Customer MakeNewCustomer(int indexInArray) {
		Transform parentTransform = customerSlot[indexInArray];
		GameObject customerObj = Instantiate(customerPrefab, parentTransform.position, Quaternion.identity);
		customerObj.transform.parent = parentTransform;
		customerObj.transform.localScale = Vector3.one;
		Customer customer = customerObj.GetComponent<Customer>();

		customer.toleranceRate = toleranceRate;
		customer.maxFuryRate = maxFuryRate;

		int newRabbitIndex;
		if(testManager != null && !testManager.randomizeCustomer 
			&& testManager.nextCustomers.Count > 0)
		{
			// 테스트 중이고 입력된 손님이 있다면 입력된대로
			newRabbitIndex = testManager.nextCustomers.First();
			testManager.nextCustomers.Remove(newRabbitIndex);
		}
		else if (true){
			List<int> indexList = RabbitData.GetRabbitGroup(groupOrder.GetNext());

			var availableIndex = indexList.Where(index => !IsThereSameIndexCustomer(index)).ToList();
			if(availableIndex.Count > 0){
				newRabbitIndex = availableIndex[Random.Range(0, availableIndex.Count)];
			}
			else{
				newRabbitIndex = indexList[Random.Range(0, indexList.Count)];
			}
		}
		else {
			// 해금된 토끼중 랜덤으로 나옴 / 이미지 중복 체크
			List<int> indexList = openedRabbitDict.Keys.ToList();
			newRabbitIndex = indexList[Random.Range(0, indexList.Count)];
			while (IsThereSameIndexCustomer(newRabbitIndex)) {
				newRabbitIndex = indexList[Random.Range(0, indexList.Count)];
			}
		}

		if (tutorialManager != null) {
			// initialize & setorder 한꺼번에
			tutorialManager.MakeCustomer(customer);
		}
		else {
			Rabbit newRabbitData = RabbitData.GetRabbitData(newRabbitIndex);
			customer.Initialize(indexInArray, newRabbitData);
			if (IsCustomerSlotEmpty()) {
				customer.SetOrder(trayManager.MakeOrderTray(customer.rabbitData.variablesOfOrderFood, 0));
			}
			else {
				int comboCount = trayManager.comboCount;
				int autoServedProb = 100 - (comboCount * 20);
				customer.SetOrder(trayManager.MakeOrderTray(customer.rabbitData.variablesOfOrderFood, autoServedProb));
			}
		}

		AddCustomerInEmptySlot(customer);
		lastCustomerMakeTime = 0;
		Canvas[] customerCanvases = customer.GetComponentsInChildren<Canvas>();
		foreach(var canvas in customerCanvases)
		{
			canvas.overrideSorting = true;
		}

		return customer;
	}

	void AddCustomerInEmptySlot(Customer newCustomer) {
		int index = newCustomer.indexInArray;
		if (currentWaitingCustomers[index] != null) {
			Debug.LogError("Cannot add new customer in slot");
			return;
		}
		else {
			currentWaitingCustomers[index] = newCustomer;
		}
	}
	bool IsCustomerSlotEmpty(){
		for (int i = 0; i < currentWaitingCustomers.Length; i++) {
			if (currentWaitingCustomers[i] != null) {
				return false;
			}
		}
		//Debug.Log("CustomerManager.IsCustomerSlotEmpty == true : "+Time.time);
		return true;
	}

	bool IsEmptyPosInCustomerSlot() {
		for (int i = 0; i < currentWaitingCustomers.Length; i++) {
			if (currentWaitingCustomers[i] == null) {
				return true;
			}
		}
		return false;
	}

	int GetFirstEmptyPosInCustomerSlot() {
		for (int i = 0; i < currentWaitingCustomers.Length; i++) {
			if (currentWaitingCustomers[i] == null) {
				return i;
			}
		}
		Debug.LogError("Cannot find empty slot");
		return -1;
	}

	void SetOpenedRabbitsIndexList() {
		int currentStageIndex = MissionData.stageIndex;
		openedRabbitDict = new Dictionary<int, Rabbit>();
		for (int index = 1; index <= RabbitData.numberOfRabbitData; index++) {
			Rabbit newRabbitData = RabbitData.GetRabbitData(index);
			if (newRabbitData.releaseStageIndex <= currentStageIndex)
				openedRabbitDict.Add(index, newRabbitData);
		}
	}

	void Awake () {
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.waitingTime)) {
			waitingTime = missionDataDict[MissionDataType.waitingTime];
		}
		if (missionDataDict.ContainsKey(MissionDataType.customerCooldown)) {
			customerCooldown = missionDataDict[MissionDataType.customerCooldown];
		}

		groupOrder = RabbitGroupOrder.GetOrderData(MissionData.stageIndex);
		SetOpenedRabbitsIndexList();
	}

	// Use this for initialization
	void Start () {
		heartManager = FindObjectOfType<HeartManager>();
		trayManager = FindObjectOfType<TrayManager>();
		gameManager = FindObjectOfType<GameManager>();
		missionManager = FindObjectOfType<MissionManager>();
		gameStateManager = FindObjectOfType<GameStateManager>();
		scoreManager = FindObjectOfType<ScoreManager>();
		tutorialManager = FindObjectOfType<TutorialManager>();

		lastCustomerMakeTime = customerCooldown - 0.5f;
		isPlayingCustomerAnim = false;
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		// 튜토리얼 씬의 손님 추가는 별도의 로직으로 이루어진다
		if (tutorialManager != null) return;

		if (IsEmptyPosInCustomerSlot()) {
			// 손님 리필 쿨타임은 자리가 비어있을 때만 돌아간다
			// Test 씬의 경우에는 입력 손님 대기열에 손님이 있거나 랜덤 토글이 눌려 있을 때에만 돌아간다
			if(testManager != null)
			{
				if(!testManager.randomizeCustomer && testManager.nextCustomers.Count == 0) return;
			}

			lastCustomerMakeTime += Time.deltaTime; 

			if (lastCustomerMakeTime < customerCooldown) return;

			// 손님 추가는 항상 된다
			int emptySlotIndex = GetFirstEmptyPosInCustomerSlot();
			MakeNewCustomer(emptySlotIndex);
			FindObjectOfType<GameStateManager>().NewCustomerTrigger();
		}
	}
}
