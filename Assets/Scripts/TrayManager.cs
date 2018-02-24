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
    public GameObject toBeSwitched;

	public GameObject pickedFood1;
	public GameObject pickedFood2;
    Vector3 pickedFood1Origin;
    float firstScaleX;
    float firstScaleY;

	public float resetTime;
	float lastResetTime = 0;
	public Image resetTimerImage;
    public float exitAmount;

	bool isPlayingMovingAnim = false;
	public bool isPlayingRefillAnim = false;

	readonly float comboDelay = 1;
	readonly float comboDelayByMoving = 5;
	int comboCount = 0;
	float lastComboTime = 0;
	int moveCountAfterMatching = 0;

	public GameObject comboTextPrefab;

	CustomerManager customerManager;

	void MakeSuperfood() {
		// 제일 적은 종류의 음식 중 하나를 픽
		Dictionary<FoodType, int> counter = new Dictionary<FoodType, int>();
		List<FoodOnTray> foodList = new List<FoodOnTray>();
		for (int row = 0; row < ROW; row++) {
			for (int col = 0; col < COL; col++) {
				foodList.Add(foods[row, col]);
			}
		}
		foodList = foodList.FindAll(food => food != null && !food.isServed && !food.isSuperfood);
		foodList.ForEach(food => {
			FoodType type = food.foodType;
			if (counter.ContainsKey(type)) {
				int count = counter[type];
				counter[type] = count + 1;
			}
			else {
				counter.Add(type, 1);
			}
		});
		
		KeyValuePair<FoodType, int> maxValuePair = counter.First();
		foreach (var pair in counter) {
			if (pair.Value > maxValuePair.Value) {
				maxValuePair = pair;
			}
		}
		FoodType mostFoodType = maxValuePair.Key;
		var mostFoodTypeFoods = foodList.FindAll(food => !food.isSuperfood && food.foodType == mostFoodType);
		FoodOnTray preSuperfood = mostFoodTypeFoods[Random.Range(0, mostFoodTypeFoods.Count)];

		// 그 음식을 슈퍼푸드로 바꿈
		StartCoroutine(preSuperfood.ChangeToSuperfood());
	}

	void ShowComboText (List<FoodOnTray> foods) {
		Vector3 avgPos = Vector3.zero;
		foods.ForEach(food => avgPos += food.transform.position/4f);

		GameObject comboTextObj = Instantiate(comboTextPrefab, avgPos, Quaternion.identity);
		comboTextObj.GetComponentInChildren<Text>().text = comboCount + "Combo!";
		Destroy(comboTextObj, 2);
	}

	bool IsComboCountUp () {
		if (moveCountAfterMatching == 1) {
			if (lastComboTime < comboDelayByMoving) {
				Debug.Log("Combo by one move. ComboCount " + comboCount + " -> " + (comboCount+1));
				return true;
			}
			else {
				return false;
			}
		}
		else {
			if (lastComboTime < comboDelay) {
				Debug.Log("Combo by time. ComboCount " + comboCount + " -> " + (comboCount+1));
				return true;
			}
			else {
				return false;
			}
		}
	}

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
					foods[row, col].transform.DOMove(foodPoses[row, col].position, 0.2f);
					foods[row+1, col] = null;
				}
			}
			// 맨 윗줄일 경우
			else {
				GameObject newFood = Instantiate(foodObj, foodPoses[row, col].position, Quaternion.identity);
				newFood.GetComponent<FoodOnTray>().foodCoord = new Vector2(row, col);
				foods[row, col] = newFood.GetComponent<FoodOnTray>();
				newFood.transform.DOScale(0.1f, 0);
				newFood.transform.DOScale(0.4f, 0.2f);
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

	public class ServedPair {
		public Customer customer;
		public List<FoodOnTray> foods;

		public ServedPair (Customer customer, List<FoodOnTray> foods) {
			this.customer = customer;
			this.foods = foods;
		}
	}

	List<ServedPair> pairs = new List<ServedPair>();

	public IEnumerator TryMatch() { 
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		customers.OrderBy(customer => customer.remainWaitingTime); 
		pairs.Clear();

		float animDelay = 1;

		// 하나씩 맞춰보고 (위 > 아래, 왼쪽 > 오른쪽)
		for (int row = ROW-2; row >= 0; row--) {
			for (int col = 0; col < COL-1; col++) {
				List<FoodOnTray> foodsInPart = new List<FoodOnTray>();
				foodsInPart.Add(foods[row, col]);
				foodsInPart.Add(foods[row+1, col]);
				foodsInPart.Add(foods[row, col+1]);
				foodsInPart.Add(foods[row+1, col+1]);
				// null인 음식과 served인 음식(=다른 손님에게 서빙될 예정)을 제외
				foodsInPart = foodsInPart.FindAll(food => food != null && !food.isServed);
				
				List<Customer> matchedCustomers = customers.FindAll(customer => !customer.isServed && MatchEachPartWithCustomer(foodsInPart, customer));
				
				// 서빙받을 손님과 서빙할 음식을 미리 마킹
				if (matchedCustomers.Count > 0) {
					Customer matchedCustomer = matchedCustomers.First();
					List<FoodOnTray> matchedFoods = foodsInPart;

					matchedCustomer.isServed = true;
					matchedFoods.ForEach(food => food.isServed = true);

					ServedPair newPair = new ServedPair(matchedCustomer, matchedFoods);
					pairs.Add(newPair);
				}
			}
		}

		float comboAnimDelay = 0.2f;

		if (pairs.Count > 0) {
			customerManager.isPlayingCustomerAnim = true;
			foreach (var pair in pairs) {
				Customer matchedCustomer = pair.customer;
				List<FoodOnTray> matchedFoods = pair.foods;

				if (IsComboCountUp()) {
					comboCount++;
				}
				else {
					comboCount = 1;
				}

				lastComboTime = 0;

				if (comboCount > 1) {
					ShowComboText(matchedFoods);
				}

                StartCoroutine(MatchAnimation(matchedFoods, matchedCustomer, customers, animDelay));

				moveCountAfterMatching = 0;

				yield return new WaitForSeconds(comboAnimDelay);
			}
			yield return new WaitForSeconds(animDelay);
			customerManager.isPlayingCustomerAnim = false;
		}
			
		// 해당되는 음식 리필
		yield return StartCoroutine(RefillFoods());
	
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

    IEnumerator MatchAnimation(List<FoodOnTray> matchedFoods, Customer matchedCustomer, List<Customer> customers, float animDelay)
    {
        // 음식 날아가는 애니메이션
        foreach (var matchedFood in matchedFoods)
        {
            int posX = (int)matchedFood.foodCoord.x;
            int posY = (int)matchedFood.foodCoord.y;
            matchedFood.transform.DOMove(matchedFood.correspondent.transform.position, animDelay / 2f, false);
            foods[posX, posY] = null;
        }
		SoundManager.PlayCustomerReaction(matchedCustomer.gender, true);

        // 날아가는 동안 기다리도록: 연동이 되는 게 아니라 입력된 시간 그대로 기다리는 방식
        yield return new WaitForSeconds(animDelay / 2f);

        // 날아간 음식 제거
        foreach (var matchedFood in matchedFoods)
            Destroy(matchedFood.gameObject);

        // 주문판과 주문판에 있는 음식 제거
        foreach(var orderAspect in matchedCustomer.orderToBeDestroyed)
            orderAspect.SetActive(false);

        // 손님 보내고: 왼쪽 손님은 exitAmount만큼 왼쪽으로, 오른쪽 손님은 exitAmount만큼 오른쪽으로
        matchedCustomer.customerImage.transform.DOJump(
            new Vector3(matchedCustomer.transform.position.x > 0 ? matchedCustomer.transform.position.x + exitAmount :
            matchedCustomer.transform.position.x - exitAmount, matchedCustomer.transform.position.y, 0.0f), 0.5f, 3, animDelay);
        customerManager.RemoveCustomerByMatching(matchedCustomer.indexInArray, animDelay);
        customers.Remove(matchedCustomer);
    }

    bool MatchEachPartWithCustomer(List<FoodOnTray> foodsInPart, Customer customer) {
		List<FoodInOrder> foodsInOrder = customer.orderedFoods;

		// 만능음식은 갯수만 세어놓는다
		int numberOfSuperfood = foodsInPart.Count(food => food.isSuperfood);		
		List<FoodType> foodsTypeOnTray = new List<FoodType>();
		foodsInPart.ForEach(food => {
			if (!food.isSuperfood) {
				foodsTypeOnTray.Add(food.foodType);
			}
		});

		int remainSuperfoodCount = numberOfSuperfood;
		// 판정이 실패했을 때 만능음식이 있으면 하나 쓴다
		foreach (var foodInOrder in foodsInOrder) {
			bool isThereMatchedFoodType = foodsTypeOnTray.Any(foodTypeOnTray => foodTypeOnTray == foodInOrder.foodType);
			if (isThereMatchedFoodType) {
				foodsTypeOnTray.Remove(foodInOrder.foodType);
			}
			else {
				if (remainSuperfoodCount > 0) {
					remainSuperfoodCount -= 1;
				}
				else {
					return false;
				}
			}
		}

		// 타입이 같은 음식의 correspondent를 대응시킨다. (일반음식)
		foreach(var foodInPart in foodsInPart) {
			if (!foodInPart.isSuperfood) {
				var corrFoodInOrder = foodsInOrder.Find(FoodInOrder => 
					FoodInOrder.foodType == foodInPart.foodType && 
					!FoodInOrder.foundCorrespondent);
				foodInPart.correspondent = corrFoodInOrder;

				// 트레이 음식 여럿이 주문판 음식 하나에 계속 대응되지 않도록 마킹.
				corrFoodInOrder.foundCorrespondent = true;
			}
		}
		// 만능음식은 그냥 남아있는 아무 음식의 correspondent를 대응시킨다.
		foreach(var foodInPart in foodsInPart) {
			if (foodInPart.isSuperfood) {
				var corrFoodInOrder = foodsInOrder.Find(FoodInOrder => 
					!FoodInOrder.foundCorrespondent);
				foodInPart.correspondent = corrFoodInOrder;

				corrFoodInOrder.foundCorrespondent = true;
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
		gameManager = FindObjectOfType<GameManager>();

		InitializeFoods();
		
		lastComboTime = comboDelayByMoving + 1;
	}
	
	GameManager gameManager;

	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		if (Input.GetKeyDown(KeyCode.S)) {
			MakeSuperfood();
		}

		if (Input.GetMouseButtonDown(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if ((hit.collider != null) && (!isPlayingMovingAnim))
            {
				pickedFood1 = hit.collider.gameObject;
                if(!pickedFood1.GetComponent<FoodOnTray>().isEnlarging) {
                    StartCoroutine(EnlargePickedFood(pickedFood1));
				}
                pickedFood1.GetComponent<FoodOnTray>().isEnlarging = true;
				pickedFood1.GetComponent<HighlightBorder>().ActiveBorder();
                pickedFood1Origin = new Vector3(pickedFood1.transform.position.x, pickedFood1.transform.position.y, 0);
            }
		}
        
        if(Input.GetMouseButton(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            if (pickedFood1 != null)
            {
                // 집은 음식 마우스/손가락 따라다니게 하기
                pickedFood1.transform.position = new Vector3(worldPoint.x, worldPoint.y, -3);
                // 다른 음식과 겹치게 움직이면 겹쳐진 음식을 교체 예정으로 보고 미리 반투명하게 예상 결과를 보여준다.
                if (hit.Length > 1 && hit[1].collider != null)
                {
                    toBeSwitched.SetActive(true);
                    toBeSwitched.GetComponent<SpriteRenderer>().sprite = hit[1].collider.gameObject.GetComponent<SpriteRenderer>().sprite;
                    toBeSwitched.transform.localScale = hit[1].collider.gameObject.transform.localScale;
                    toBeSwitched.transform.position = pickedFood1Origin;
                }
            }
        }

		if (Input.GetMouseButtonUp(0)) {
            // 교체 예상 이미지를 보여주지 않도록 비활성화
            toBeSwitched.SetActive(false);

            if (pickedFood1 != null && (!isPlayingMovingAnim))
            {
                StopCoroutine(EnlargePickedFood(pickedFood1));
                pickedFood1.transform.localScale = new Vector2(firstScaleX, firstScaleY);
				pickedFood1.GetComponent<HighlightBorder>().InactiveBorder();
                
                //Get the mouse position on the screen and send a raycast into the game world from that position.
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

                //If something was hit, the RaycastHit2D.collider will not be null.
                if(hit.Length>1)
                {
                    if (hit[1].collider != null)
                    {
                        if (hit[1].collider.gameObject.tag == "Bin")
                        {
                            Destroy(pickedFood1);
                            int posX = (int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.x;
                            int posY = (int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.y;
                            foods[posX, posY] = null;
                            pickedFood1 = null;
                            StartCoroutine(RefillFoods());
                        }
                        else
                            pickedFood2 = hit[1].collider.gameObject;
                    }

                    if ((pickedFood1 != null) && (pickedFood2 != null))
                    {
						// 유효이동일 경우에만 카운트 상승
                        moveCountAfterMatching++;
						StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin, pickedFood2));
                    }
                }
                else
                {
                    StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin));
                }

                //집었던 거 초기화
                if(pickedFood1 != null)
                    pickedFood1.transform.localScale = new Vector3(firstScaleX, firstScaleY);
            }
		}

		lastComboTime += Time.deltaTime;

		lastResetTime += Time.deltaTime;
		resetTimerImage.fillAmount = lastResetTime / resetTime;
	}
}
