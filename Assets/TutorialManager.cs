﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class TutorialManager : MonoBehaviour {

	int index = 0;
	bool customerTrigger = true;
	Customer currentCustomer;

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
	}

	public void Make4thCustomer(Customer customer) {
		// 1312
		Rabbit newRabbitData = new Rabbit();
		newRabbitData.gender = Gender.Male;
		newRabbitData.imageName = "youngsang";
		newRabbitData.waitingTime = 40;
		customer.Initialize(index % 2, newRabbitData);
		List<FoodType> newFoodList = new List<FoodType> {FoodType.A, FoodType.C, FoodType.A, FoodType.B};
		customer.SetOrder(newFoodList);
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
	void Start () {
		gameStateManager = FindObjectOfType<GameStateManager>();
		customerManager = FindObjectOfType<CustomerManager>();
		trayManager = FindObjectOfType<TrayManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (index > 3) return;

		if (gameStateManager.gameState != GameState.Idle) return;

		if (currentCustomer == null && customerTrigger == false) {
			customerTrigger = true;
		}

		if (customerTrigger) {
			currentCustomer = customerManager.MakeNewCustomer(index % 2);
		}
	}
}
