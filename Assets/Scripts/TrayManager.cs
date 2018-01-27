﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;
using UnityEngine.UI;
using DG.Tweening;

public class TrayManager : MonoBehaviour {

	readonly int ROW = 5;
	readonly int COL = 6;

	Transform[,] foodPoses;
	FoodOnTray[,] foods;
	public GameObject foodObj;

	public GameObject pickedFood1;
	public GameObject pickedFood2;

	public float resetTime;
	float lastResetTime = 0;
	public Image resetTimerImage;

	bool isPlayMovingAnim = false;

	CustomerManager customerManager;

	bool NoMatchingFoods() {
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		List<FoodOnTray> foods = new List<FoodOnTray>();

		if (customers.Count == 0) return false;

		bool result = true;

		// 모든 손님이 전부 주문 불가일 경우에만 true
		foreach (var customer in customers) {
			bool partialResult = false;
			foreach (var orderedFood in customer.orderedFoods) {
				var matchedFood = foods.Find(food => food.foodType == orderedFood.foodType);
				if (matchedFood != null) {
					foods.Remove(matchedFood);
				}
				else {
					partialResult = true;
				}
			}

			if (!partialResult) result = partialResult;
		}

		return result;
	}

	void TryMatchEachPart(Vector2 pos, List<Customer> customers) {
		
	}

	public void TryMatch() {
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		customers.OrderBy(customer => customer.remainWaitingTime);

		// 하나씩 맞춰보고 삭제
		for (int row = 0; row < ROW-1; row++) {
			for (int col = 0; col < COL-1; col++) {
				List<FoodOnTray> foodsInPart = new List<FoodOnTray>();
				foodsInPart.Add(foods[row, col]);
				foodsInPart.Add(foods[row+1, col]);
				foodsInPart.Add(foods[row, col+1]);
				foodsInPart.Add(foods[row+1, col+1]);
				foodsInPart = foodsInPart.FindAll(food => foodObj != null);

				Customer matchedCustomer = customers.Find(customer => MatchEachPartWithCustomer(foodsInPart, customer));
				if (matchedCustomer != null) {
					// 손님 보내고
					customerManager.RemoveCustomerByMatching(matchedCustomer.indexInArray);
					customers.Remove(matchedCustomer);
					// 맞춰진 음식 삭제
					foodsInPart.ForEach(food => Destroy(food.gameObject));
					// 해당되는 음식 리필 (아직 미구현)
					
				}
			}
		}

		// // 판 자동 리셋 체크
		// if (NoMatchingFoods()) {
		// 	trays.ForEach(tray => tray.Refresh());
		// }
	}

	bool MatchEachPartWithCustomer(List<FoodOnTray> foodsInPart, Customer customer) {
		List<FoodInOrder> foodsInOrder = customer.orderedFoods;
		
		List<FoodType> foodsTypeOnTray = new List<FoodType>();
		foodsInPart.ForEach(food => {
			foodsTypeOnTray.Add(food.foodType);
		});

		foreach (var foodInOrder in foodsInOrder) {
			bool isThereMatchedFoodType = foodsTypeOnTray.Any(foodTypeOnTray => foodTypeOnTray == foodInOrder.foodType);
			if (isThereMatchedFoodType) {
				foodsTypeOnTray.Remove(foodInOrder.foodType);
			}
			else {
				return false;
			}
		}
		
		return true;
	}

	float moveSpeed = 0.2f;

	IEnumerator ChangeFoodPosition(GameObject food1, GameObject food2) {
		isPlayMovingAnim = true;

		Vector3 positionOfFood1 = food1.transform.position;
		Vector3 positionOfFood2 = food2.transform.position;

		Vector3 positionTemp = food1.transform.position;

		Tween tw = food1.transform.DOMove(positionOfFood2, moveSpeed);
		food2.transform.DOMove(positionTemp, moveSpeed);

		yield return tw.WaitForCompletion();

		Vector2 coordOfFood1 = food1.GetComponent<FoodOnTray>().foodCoord;
		Vector2 coordOfFood2 = food2.GetComponent<FoodOnTray>().foodCoord;

		Vector2 tempCoord = food1.GetComponent<FoodOnTray>().foodCoord;

		FoodOnTray tempFood = foods[(int)coordOfFood1.x, (int)coordOfFood1.y];
		foods[(int)coordOfFood1.x, (int)coordOfFood1.y] = foods[(int)coordOfFood2.x, (int)coordOfFood2.y];
		foods[(int)coordOfFood2.x, (int)coordOfFood2.y] = tempFood;

		food1.GetComponent<FoodOnTray>().foodCoord = coordOfFood2;
		food2.GetComponent<FoodOnTray>().foodCoord = tempCoord;

		// 이동 성공 후 초기화
		pickedFood1 = null;
		pickedFood2 = null;

		TryMatch();

		isPlayMovingAnim = false;
	}

	void InitializeFoods() {
		List<Transform> foodPosesList = new List<Transform>();
		FindObjectsOfType<FoodPos>().ToList()
									.ForEach(fp => foodPosesList.Add(fp.transform));
		foodPosesList = foodPosesList.OrderBy(fp => fp.transform.position.y)
									 .ThenBy(fp => fp.transform.position.x)
									 .ToList();

		foodPosesList.ForEach(fp => fp.GetComponent<SpriteRenderer>().enabled = false);

		// 2차원 배열일때 array[row(층수)][col(호수)]
		foodPoses = new Transform[ROW, COL];
		for (int row = 0; row < ROW; row++) {
			for (int col = 0; col < COL; col++) {
				foodPoses[row, col] = foodPosesList[row*COL + col];
			}
		}

		foods = new FoodOnTray[ROW, COL];
		for (int row = 0; row < ROW; row++) {
			for (int col = 0; col < COL; col++) {
				GameObject newFood = Instantiate(foodObj, foodPoses[row, col].position, Quaternion.identity);
				newFood.GetComponent<FoodOnTray>().foodCoord = new Vector2(row, col);
				foods[row, col] = newFood.GetComponent<FoodOnTray>();
			}
		}
	}

	// Use this for initialization
	void Start () {
		customerManager = FindObjectOfType<CustomerManager>();

		InitializeFoods();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if ((hit.collider != null) && (!isPlayMovingAnim))
            {
				pickedFood1 = hit.collider.gameObject;
				pickedFood1.GetComponent<SpriteRenderer>().DOFade(0.5f, 0);
            }
		}	

		if (Input.GetMouseButtonUp(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
				pickedFood2 = hit.collider.gameObject;
            }

			if ((pickedFood1 != null) && (pickedFood2 != null)) {
				StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood2));
			}

			// 집었던거 초기화
			if (pickedFood1 != null)
				pickedFood1.GetComponent<SpriteRenderer>().DOFade(1, 0);
			pickedFood1 = null;
		}

		lastResetTime += Time.deltaTime;
		resetTimerImage.fillAmount = lastResetTime / resetTime;

		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (lastResetTime < resetTime) return;

			TryMatch();
			lastResetTime = 0;
			resetTimerImage.fillAmount = lastResetTime / resetTime;
		}
	}
}
