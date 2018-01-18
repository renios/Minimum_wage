using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;
using UnityEngine.UI;

public class TrayManager : MonoBehaviour {

	List<Tray> trays;
	public GameObject pickedFood1;
	public GameObject pickedFood2;

	public float resetTime;
	float lastResetTime = 0;
	public Image resetTimerImage;

	CustomerManager customerManager;

	public void TryMatch() {
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		customers.OrderBy(customer => customer.remainWaitingTime);

		// 하나씩 맞춰보고 삭제
		foreach (var tray in trays) {
			Customer matchedCustomer = customers.Find(customer => MatchTrayWithCustomer(tray, customer));
			if (matchedCustomer != null) {
				// 손님 보내고
				customerManager.RemoveCustomerByMatching(matchedCustomer.indexInArray);
				customers.Remove(matchedCustomer);
				// 해당되는 트레이 리필
				tray.Refresh();
			}
		}
	}

	bool MatchTrayWithCustomer(Tray tray, Customer customer) {
		List<FoodInOrder> foodsInOrder = customer.orderedFoods;
		List<FoodOnTray> foodsOnTray = tray.foods;
		
		List<FoodType> foodsTypeOnTray = new List<FoodType>();
		foodsOnTray.ForEach(food => {
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

	void ChangeFoodPosition(GameObject food1, GameObject food2) {
		// 트레이와 위치를 둘 다 바꿔야 함
		Transform parentOfFood1 = food1.transform.parent;
		Vector3 positionOfFood1 = food1.transform.position;
		Transform parentOfFood2 = food2.transform.parent;
		Vector3 positionOfFood2 = food2.transform.position;

		Transform parentTemp = food1.transform.parent;
		Vector3 positionTemp = food1.transform.position;

		food1.transform.parent = parentOfFood2;
		food2.transform.parent = parentTemp;

		food1.transform.position = positionOfFood2;
		food2.transform.position = positionTemp;

		// 이동 성공 후 초기화
		pickedFood1 = null;
		pickedFood2 = null;

		trays.ForEach(tray => tray.UpdateFoodListOnTray());

		TryMatch();
	}

	// Use this for initialization
	void Start () {
		customerManager = FindObjectOfType<CustomerManager>();
		trays = FindObjectsOfType<Tray>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
				pickedFood1 = hit.collider.gameObject;
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
				ChangeFoodPosition(pickedFood1, pickedFood2);
			}
		}

		lastResetTime += Time.deltaTime;
		resetTimerImage.fillAmount = lastResetTime / resetTime;

		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (lastResetTime < resetTime) return;

			trays.ForEach(tray => tray.Refresh());
			TryMatch();
			lastResetTime = 0;
			resetTimerImage.fillAmount = lastResetTime / resetTime;
		}
	}
}
