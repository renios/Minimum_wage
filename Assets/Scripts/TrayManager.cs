using System.Collections;
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
    Vector3 pickedFood1Origin;
    float firstScaleX;
    float firstScaleY;

	public float resetTime;
	float lastResetTime = 0;
	public Image resetTimerImage;

	bool isPlayingMovingAnim = false;
	public bool isPlayingRefillAnim = false;

	CustomerManager customerManager;

	bool NoMatchingFoods() {
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		List<FoodOnTray> foodsList = new List<FoodOnTray>();

		if (customers.Count == 0) return false;

		bool result = true;

		// 모든 손님이 전부 주문 불가일 경우에만 true
		foreach (var customer in customers) {
			bool partialResult = false;

			foodsList.Clear();
			for (int row = 0; row < ROW; row++) {
				for (int col = 0; col < COL; col++) {
					foodsList.Add(foods[row, col]);
				}
			}

			foreach (var orderedFood in customer.orderedFoods) {
				var matchedFood = foodsList.Find(food => food.foodType == orderedFood.foodType);
				if (matchedFood != null) {
					foodsList.Remove(matchedFood);
				}
				else {
					partialResult = true;
				}
			}

			if (!partialResult) result = partialResult;
		}

		return result;
	}

	void CheckAndRefill(int row, int col) {
		if (foods[row, col] == null) {
			// 맨 윗줄이 아닐 경우
			if (row+1 < ROW) {
				foods[row, col] = foods[row+1, col];
				
				if (foods[row+1, col] != null) {
					foods[row, col].foodCoord = new Vector2(row, col);
					Tween tw = foods[row, col].transform.DOMove(foodPoses[row, col].position, 0.2f);
					foods[row+1, col] = null;
					// yield return tw.WaitForCompletion();
				}
			}
			// 맨 윗줄일 경우
			else {
				GameObject newFood = Instantiate(foodObj, foodPoses[row, col].position, Quaternion.identity);
				newFood.GetComponent<FoodOnTray>().foodCoord = new Vector2(row, col);
				foods[row, col] = newFood.GetComponent<FoodOnTray>();
				newFood.transform.DOScale(0.1f, 0);
				Tween tw = newFood.transform.DOScale(0.4f, 0.2f);
				// yield return tw.WaitForCompletion();
			}
		} 
	}

	bool IsTrayFull() {
		for (int row = 0; row < ROW; row++) {
			for (int col = 0; col < COL; col++) {
				if (foods[row, col] == null) return false;
			}
		}
		return true;
	}

	IEnumerator RefillFoods() {
		isPlayingRefillAnim = true;
		while (!IsTrayFull()) {	
			for (int row = 0; row < ROW; row++) {
				for (int col = 0; col < COL; col++) {
					CheckAndRefill(row, col);
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
		isPlayingRefillAnim = false;
	}

	public IEnumerator TryMatch() { 
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
				foodsInPart = foodsInPart.FindAll(food => food != null);
				
				float animDelay = 1;
				List<Customer> matchedCustomers = customers.FindAll(customer => MatchEachPartWithCustomer(foodsInPart, customer));
				if (matchedCustomers.Count > 0) {
					Customer matchedCustomer = matchedCustomers.First(); 
					// 손님 보내고
					customerManager.isPlayingCustomerAnim = true;
					matchedCustomer.transform.DOLocalJump(matchedCustomer.transform.position, 0.5f, 3, animDelay);
					customerManager.RemoveCustomerByMatching(matchedCustomer.indexInArray, animDelay);
					customers.Remove(matchedCustomer);
					// 맞춰진 음식 삭제
					foodsInPart.ForEach(food => {
						int posX = (int)food.foodCoord.x;
						int posY = (int)food.foodCoord.y;
						foods[posX, posY].transform.DOLocalJump(foods[posX, posY].transform.position, 1, 1, animDelay);
						foods[posX, posY] = null;
						Destroy(food.gameObject, animDelay);
					});
					yield return new WaitForSeconds(animDelay);
					FindObjectOfType<MissionManager_temp>().successCustomer++;
					customerManager.isPlayingCustomerAnim = false;
					
					// 해당되는 음식 리필
					yield return StartCoroutine(RefillFoods());
				}
			}
		}

		// 판 자동 리셋 체크
		if (NoMatchingFoods()) {
			for (int row = 0; row < ROW; row++) {
				for (int col = 0; col < COL; col++) {
					Destroy(foods[row, col].gameObject);
					foods[row, col] = null;
				}
			}

			yield return StartCoroutine(RefillFoods());
		}
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

	IEnumerator ChangeFoodPosition(GameObject food1, Vector3 food1Origin, GameObject food2 = null ) {
		isPlayingMovingAnim = true;

        if(food2!=null)
        {
            Vector3 positionOfFood2 = food2.transform.position;

            Tween tw = food1.transform.DOMove(positionOfFood2, moveSpeed);
            food2.transform.DOMove(food1Origin, moveSpeed);
            yield return tw.WaitForCompletion();

            Vector2 coordOfFood1 = food1.GetComponent<FoodOnTray>().foodCoord;
            Vector2 coordOfFood2 = food2.GetComponent<FoodOnTray>().foodCoord;

            Vector2 tempCoord = food1.GetComponent<FoodOnTray>().foodCoord;

            FoodOnTray tempFood = foods[(int)coordOfFood1.x, (int)coordOfFood1.y];
            foods[(int)coordOfFood1.x, (int)coordOfFood1.y] = foods[(int)coordOfFood2.x, (int)coordOfFood2.y];
            foods[(int)coordOfFood2.x, (int)coordOfFood2.y] = tempFood;

            food1.GetComponent<FoodOnTray>().foodCoord = coordOfFood2;
            food2.GetComponent<FoodOnTray>().foodCoord = tempCoord;
        }
        else
        {
            Tween tw = food1.transform.DOMove(food1Origin, moveSpeed);
            yield return tw.WaitForCompletion();
        }

		// 이동 성공 후 초기화
		pickedFood1 = null;
		pickedFood2 = null;
		yield return StartCoroutine(TryMatch());

		isPlayingMovingAnim = false;
	}

    IEnumerator EnlargePickedFood(GameObject food)
    {
        float mulFactor = 1f;
        firstScaleX = food.transform.localScale.x;
        firstScaleY = food.transform.localScale.y;
        while (mulFactor < 1.4f)
        {
            mulFactor = Mathf.Lerp(mulFactor, 2f, 0.1f);
            food.transform.localScale = new Vector3(firstScaleX*mulFactor, firstScaleY*mulFactor);
            yield return null;
        }
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
            if ((hit.collider != null) && (!isPlayingMovingAnim))
            {
				pickedFood1 = hit.collider.gameObject;
				pickedFood1.GetComponent<SpriteRenderer>().DOColor(Color.blue, 0);
                if(pickedFood1.GetComponent<FoodOnTray>().isEnlarging==false)
                    StartCoroutine(EnlargePickedFood(pickedFood1));
                pickedFood1.GetComponent<FoodOnTray>().isEnlarging = true;
                pickedFood1Origin = new Vector3(pickedFood1.transform.position.x, pickedFood1.transform.position.y, 0);
            }
		}
        
        if(Input.GetMouseButton(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(pickedFood1!=null)
               pickedFood1.transform.position = new Vector3(worldPoint.x, worldPoint.y, -3);
        }

		if (Input.GetMouseButtonUp(0)) {
            if (pickedFood1 != null && (!isPlayingMovingAnim))
            {
                StopCoroutine(EnlargePickedFood(pickedFood1));
                pickedFood1.transform.localScale = new Vector2(firstScaleX, firstScaleY);
                
                //Get the mouse position on the screen and send a raycast into the game world from that position.
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

                //If something was hit, the RaycastHit2D.collider will not be null.
                if(hit.Length>1)
                {
                    if (hit[1].collider != null)
                    {
                        pickedFood2 = hit[1].collider.gameObject;
                    }

                    if ((pickedFood1 != null) && (pickedFood2 != null))
                    {
                        StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin, pickedFood2));
                    }
                }
                else
                {
                    StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin));
                }

                //집었던 거 초기화
                pickedFood1.GetComponent<SpriteRenderer>().DOColor(Color.white, 0);
                pickedFood1.transform.localScale = new Vector3(firstScaleX, firstScaleY);

            }
		}

		lastResetTime += Time.deltaTime;
		resetTimerImage.fillAmount = lastResetTime / resetTime;
	}
}
