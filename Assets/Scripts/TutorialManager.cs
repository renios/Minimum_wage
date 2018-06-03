using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Enums;

public class TutorialManager : MonoBehaviour {

	int index = 0;
	bool customerTrigger = true;
	Customer currentCustomer;

	// 강조용 화살표
	public GameObject arrowObj;
	
	public List<GameObject> tutorialList;
	public GameObject currentTutorialPanel;
	public int tutorialStep = 0;
	public int beforeTutorialStep = -1;

	public List<FoodType> refillList = 
		new List<FoodType> {FoodType.A, FoodType.B, FoodType.A, FoodType.D,
							FoodType.C, FoodType.B, FoodType.B, FoodType.C};

	public void MakeCustomer(Customer customer) {
		customerTrigger = false;

		if (index == 0) {
			Make1stCustomer(customer);
		}
		else if (index == 1) {
			Make2ndCustomer(customer);
		}
		else if (index == 2) {
			Make3rdCustomer(customer);
		}
		else if (index == 3) {
			Make4thCustomer(customer);
		}
		index += 1;
	}

	public void Make1stCustomer(Customer customer) {
		// 1121
		Rabbit newRabbitData = new Rabbit();
		newRabbitData.gender = Gender.Male;
		newRabbitData.imageName = "orchid";
		newRabbitData.waitingTime = 100000;
		customer.Initialize(index % 2, newRabbitData);
		List<FoodType> newFoodList = new List<FoodType> {FoodType.A, FoodType.A, FoodType.B, FoodType.A};
		customer.SetOrder(newFoodList);

		UpdateTutorialPanel();
		tutorialStep += 1;
	}

	public void Make2ndCustomer(Customer customer) {
		// 3123
		Rabbit newRabbitData = new Rabbit();
		newRabbitData.gender = Gender.Female;
		newRabbitData.imageName = "haram";
		newRabbitData.waitingTime = 100000;
		customer.Initialize(index % 2, newRabbitData);
		List<FoodType> newFoodList = new List<FoodType> {FoodType.C, FoodType.A, FoodType.B, FoodType.C};
		customer.SetOrder(newFoodList);

		UpdateTutorialPanel();
		tutorialStep += 1;
	}

	public void Make3rdCustomer(Customer customer) {
		// 4333
		Rabbit newRabbitData = new Rabbit();
		newRabbitData.gender = Gender.Male;
		newRabbitData.imageName = "eq";
		newRabbitData.waitingTime = 10;
		customer.Initialize(index % 2, newRabbitData);
		List<FoodType> newFoodList = new List<FoodType> {FoodType.D, FoodType.C, FoodType.C, FoodType.C};
		customer.SetOrder(newFoodList);

		UpdateTutorialPanel();
		tutorialStep += 1;
	}

	public void Make4thCustomer(Customer customer) {
		// 1312
		Rabbit newRabbitData = new Rabbit();
		newRabbitData.gender = Gender.Male;
		newRabbitData.imageName = "youngsang";
		newRabbitData.waitingTime = 100000;
		customer.Initialize(index % 2, newRabbitData);
		List<FoodType> newFoodList = new List<FoodType> {FoodType.A, FoodType.C, FoodType.A, FoodType.B};
		customer.SetOrder(newFoodList);

		UpdateTutorialPanel();
		tutorialStep += 1;
	}

	public void MakeTutorialTray() {
		trayManager.MakeBlockObject(44);

		trayManager.foods[1, 1].Initialize(FoodType.C);
		trayManager.foods[1, 2].Initialize(FoodType.B);
		trayManager.foods[1, 3].Initialize(FoodType.B);
		trayManager.foods[1, 4].Initialize(FoodType.D);

		trayManager.foods[2, 1].Initialize(FoodType.B);
		trayManager.foods[2, 2].Initialize(FoodType.D);
		trayManager.foods[2, 3].Initialize(FoodType.C);
		trayManager.foods[2, 4].Initialize(FoodType.D);

		trayManager.foods[3, 1].Initialize(FoodType.C);
		trayManager.foods[3, 2].Initialize(FoodType.D);
		trayManager.foods[3, 3].Initialize(FoodType.C);
		trayManager.foods[3, 4].Initialize(FoodType.A);

		trayManager.foods[4, 1].Initialize(FoodType.A);
		trayManager.foods[4, 2].Initialize(FoodType.B);
		trayManager.foods[4, 3].Initialize(FoodType.A);
		trayManager.foods[4, 4].Initialize(FoodType.A);
	}

	GameStateManager gameStateManager;
	CustomerManager customerManager;
	TrayManager trayManager;

	// Use this for initialization
	void Awake () {
		gameStateManager = FindObjectOfType<GameStateManager>();
		customerManager = FindObjectOfType<CustomerManager>();
		trayManager = FindObjectOfType<TrayManager>();
	}
	
	float customerCooldown = 1;
	float remainCooldown = 1;

	float stepDelay = 4;
	float remainStepDelay = 2.4f*2; // 하람토끼 처음 들어왔을때 적용

	// Update is called once per frame
	void Update () {
		if (tutorialStep > 14) return;

		if (tutorialStep == 14) {
			FindObjectOfType<GameStateManager>().gameState = GameState.End;
			StartCoroutine(FindObjectOfType<GameManager>().ShowClearCanvas());
			tutorialStep = 15;
		}

		UpdateTrayHighlight();
		UpdateTutorialPanel();
		
		if (gameStateManager.gameState != GameState.Idle) return;

		if (currentCustomer == null && customerTrigger == false) {
			if (remainCooldown < 0) {
				customerTrigger = true;
				remainCooldown = customerCooldown;
			}
			else {
				remainCooldown -= Time.deltaTime;
			}
		}

		if (customerTrigger) {
			currentCustomer = customerManager.MakeNewCustomer(index % 2);
		}

		if (tutorialStep == 4 || tutorialStep == 12) {
			if (remainStepDelay > 0) {
				remainStepDelay -= Time.deltaTime;
			}
			else {
				tutorialStep += 1;
				remainStepDelay = stepDelay;
			}
		}
	}

	void UpdateTrayHighlight()
	{
		if (beforeTutorialStep == tutorialStep) return;
		
		arrowObj.SetActive(false);
		for (int col = 1; col < 5; col++)
		{
			for (int row = 1; row < 5; row++)
			{
				trayManager.foods[row, col].GetComponent<SpriteRenderer>().color = Color.white;
			}
		}

		if (tutorialStep == 1)
		{
			HighlightTargetFood(3, 1);
		}
		else if (tutorialStep == 2)
		{
			HighlightTargetFood(3, 3);
		}
		else if (tutorialStep == 5)
		{
			HighlightTargetFood(3, 4);
		}
		else if (tutorialStep == 6)
		{
			HighlightTargetFood(4, 2);
		}
		else if (tutorialStep == 7)
		{
			HighlightTargetFood(1, 2);
		}
		else if (tutorialStep == 8)
		{
			HighlightTargetFood(4, 1);
		}

		if (beforeTutorialStep > tutorialStep) 
			beforeTutorialStep -= 1;
	}

	void HighlightTargetFood(int colIndex, int rowIndex)
	{
		var targetFood = trayManager.foods[rowIndex, colIndex];
		arrowObj.transform.position = trayManager.foodPoses[rowIndex, colIndex].position;
		arrowObj.SetActive(true);
		for (int col = 1; col < 5; col++)
		{
			for (int row = 1; row < 5; row++)
			{
				if (trayManager.foods[row, col] == targetFood) continue;
				trayManager.foods[row, col].GetComponent<SpriteRenderer>().color = Color.gray;
			}
		}
	}

	void UpdateTutorialPanel() {
		if (beforeTutorialStep < tutorialStep) {
			if (tutorialStep == 0) {
				currentTutorialPanel = tutorialList[0];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 3) {
				currentTutorialPanel.SetActive(false);
			}
			else if (tutorialStep == 4) {
				currentTutorialPanel = tutorialList[1];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 5) {
				currentTutorialPanel.SetActive(false);
				currentTutorialPanel = tutorialList[2];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 7) {
				currentTutorialPanel.SetActive(false);
				currentTutorialPanel = tutorialList[3];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 9) {
				currentTutorialPanel.SetActive(false);
			}
			else if (tutorialStep == 10) {
				currentTutorialPanel = tutorialList[4];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 11) {
				currentTutorialPanel.SetActive(false);
				currentTutorialPanel = tutorialList[5];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 12) {
				currentTutorialPanel.SetActive(false);
				currentTutorialPanel = tutorialList[6];
				currentTutorialPanel.SetActive(true);
			}
			else if (tutorialStep == 13) {
				currentTutorialPanel.SetActive(false);
				currentCustomer.waitingTime = 40;
				currentCustomer.remainWaitingTime = 40;
			}

			beforeTutorialStep += 1;
		}
	}
}
