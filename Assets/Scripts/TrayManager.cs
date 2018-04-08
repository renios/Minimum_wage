using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;
using UnityEngine.UI;
using DG.Tweening;

public class TrayManager : MonoBehaviour {

	// 트레이의 행/열 수 고정
	readonly int ROW = 5;
	readonly int COL = 6;

	// 미리 트레이 내 좌표 별 트랜스폼 받아둠
	Transform[,] foodPoses;
	// 트레이 내 좌표 별 음식 추적
	FoodOnTray[,] foods;
	// 음식 prefab
	public GameObject foodObj;
	// 교환될 음식 미리 보여주는 오브젝트
	public GameObject toBeSwitched;

	// 처음 지정한 음식 오브젝트
	public GameObject pickedFood1;
	// 첫 음식과 자리 맞바꿀 음식 오브젝트
	public GameObject pickedFood2;
	// 처음 지정한 음식 오브젝트의 초기 위치 ////////////////////////////////////수정할 것(영상)////////////////////////////////////////
	Vector3 pickedFood1Origin;

	// 조작하는 음식 크기 변환한 후 되돌아갈 수 있도록 초기 스케일값 저장
	float firstScaleX;
	float firstScaleY;

	// 손님 퇴장 애니메이션에서 손님 움직임 정도를 조절하기 위한 변수
	public float exitAmount;

	// 플레이어의 조작에 의해 두 음식이 위치 변화를 하는 애니메이션 중임을 나타내는 변수
	// 이 변수가 true이면 트레이 셔플 애니메이션은 잠시 기다리고, 음식을 새로 집거나 집고 있던 음식을 놓는 것은 불가능하다
	bool isPlayingMovingAnim = false;
	// 판에서 일부 음식이 사라져 채워 넣을 때의 애니메이션이 진행 중임을 나타내는 변수
	// 이 변수가 true이면 트레이 셔플 애니메이션과 자동 매치 메소드는 잠시 기다린다.
	// 이 변수가 true인 상황에서 플레이어의 조작에 의해 음식 교체가 일어날 경우, 교체 애니메이션 역시 잠시 기다린다.
	// 이 변수가 true이 상황에서는 새로운 손님이 등장하지 않는다.
	public bool isPlayingRefillAnim = false;
	// 자동 매치가 이루어지고 있음(애니메이션 포함)을 나타내는 변수
	// 이 변수가 true이면 새로운 자동 매치는 잠시 기다린다.
	bool isTryingMatch = false;

	// 콤보 애니메이션을 위한 딜레이 시간값 미리 고정
	// 시간에 의한 콤보 애니메이션용 딜레이
	readonly float comboDelay = 2;
	// 조작에 의한 콤보 애니메이션용 딜레이
	readonly float comboDelayByMoving = 5;
	// 콤보 스택 카운팅 변수
	public int comboCount = 0;
	// 마지막으로 콤보 스택을 쌓았을 때로부터 지나간 시간
	float lastComboTime = 0;
	// 마지막으로 매칭이 이루어진 이후 플레이어가 조작한 횟수
	int moveCountAfterMatching = 0;

	// 쓰레기통 위에 마우스가 있는지 판별하는 변수: 쓰레기통 UI 위에 마우스오버하면 true, 떠나면 false가 된다.
	public bool isOnBin;

	// 콤보 텍스트 prefab
	public GameObject comboTextPrefab;

	// 음식 맞춰지는 이펙트
	public GameObject matchingEffect;

	// 매니저들 미리 받아놓기
	CustomerManager customerManager;
	MissionManager missionManager;
	GameManager gameManager;
	// FeverManager feverManager;
	GameStateManager gameStateManager;

	List<List<FoodType>> allPossibleOrders;

	List<FoodType> MakeSameOrderTray(List<FoodType> order) {
		List<FoodType> newOrder = new List<FoodType>();
		order.ForEach(food => newOrder.Add(food));
		return newOrder;
	}

	void SetAllPossibleOrders() {
		allPossibleOrders = new List<List<FoodType>>();
		for(int i = 0; i < MissionData.foodTypeCount; i++){
			for (int j = 0; j <= i; j++){
				for(int k = 0; k <= j; k++){
					for (int l = 0; l <= k; l++){
						var foodTypes = new List<FoodType>();
						foodTypes.Add((FoodType)l);
						foodTypes.Add((FoodType)k);
						foodTypes.Add((FoodType)j);
						foodTypes.Add((FoodType)i);
						allPossibleOrders.Add(foodTypes);
					}
				}
			}
		}
	}

	int CountVariableOfOrder(List<FoodType> order) {
		List<FoodType> foodTypes = new List<FoodType>();
		foreach (var foodType in order) {
			if (!foodTypes.Contains(foodType)){
				foodTypes.Add(foodType);
			}
		}
		return foodTypes.Count();
	}

	public List<FoodType> MakeOrderTray(List<int> variablesOfOrderFood, int autoServedProb = 100) {
		int randNum = Random.Range(0, 100) + 1;
		// Debug.Log(randNum + " / " + autoServedProb);
		if (randNum < autoServedProb) {
			return GetRandomTray(variablesOfOrderFood);
		}
		else {
			return GetTraysNotOnFoods(variablesOfOrderFood);
		}
	}

	public List<FoodType> GetRandomTray(List<int> variablesOfOrderFood){
		int variableOfOrderFood = variablesOfOrderFood.OrderBy(a => Random.value).ToList().First();

		allPossibleOrders = allPossibleOrders.OrderBy<List<FoodType>, float>(a => Random.value).ToList();

		foreach(var order in allPossibleOrders) {
			if (CountVariableOfOrder(order) == variableOfOrderFood) {
				return MakeSameOrderTray(order);
			}
		}

		// 여기까지 아마 안 올듯
		Debug.LogWarning("In GetRandomTray");
		return MakeSameOrderTray(allPossibleOrders.First());
	}

	public List<FoodType> GetTraysNotOnFoods(List<int> variablesOfOrderFood){
		int variableOfOrderFood = variablesOfOrderFood.OrderBy(a => Random.value).ToList().First();

		allPossibleOrders = allPossibleOrders.OrderBy<List<FoodType>, float>(a => Random.value).ToList();
		
		var filteredResult = allPossibleOrders.FindAll(order => CountVariableOfOrder(order) == variableOfOrderFood);
		foreach(var order in filteredResult){
			for (int row = 0; row < ROW-1; row++){
				for(int col = 0; col < COL-1; col++){
					var foodsOnTray = new List<FoodOnTray>();
					if (foods[row, col] == null || foods[row+1, col] == null ||
						foods[row, col+1] == null || foods[row+1, col+1] == null) 
						continue;
					foodsOnTray.Add(foods[row, col]);
					foodsOnTray.Add(foods[row+1, col]);
					foodsOnTray.Add(foods[row, col+1]);
					foodsOnTray.Add(foods[row+1, col+1]);
					if(!MatchEachPartWithCustomer(foodsOnTray, order)) {
						return MakeSameOrderTray(order);
					}
				}
			}
		}

		// 주문 음식 종류 수가 1 초과인 손님은 다른쪽도 체크한다
		if (variablesOfOrderFood.Count == 1) {
			return MakeSameOrderTray(allPossibleOrders.First());
		}

		int anotherVariableOfOrderFood = variablesOfOrderFood.Find(num => num != variableOfOrderFood);
		filteredResult = allPossibleOrders.FindAll(order => CountVariableOfOrder(order) == anotherVariableOfOrderFood);
		foreach(var order in filteredResult){
			for (int row = 0; row < ROW-1; row++){
				for(int col = 0; col < COL-1; col++){
					var foodsOnTray = new List<FoodOnTray>();
					if (foods[row, col] == null || foods[row+1, col] == null ||
						foods[row, col+1] == null || foods[row+1, col+1] == null) 
						continue;
					foodsOnTray.Add(foods[row, col]);
					foodsOnTray.Add(foods[row+1, col]);
					foodsOnTray.Add(foods[row, col+1]);
					foodsOnTray.Add(foods[row+1, col+1]);
					if(!MatchEachPartWithCustomer(foodsOnTray, order)) {
						return MakeSameOrderTray(order);
					}
				}
			}
		}

		// 다른쪽도 중복이라면, 나중에 체크한 쪽의 트레이 중 하나를 그냥 낸다
		return MakeSameOrderTray(filteredResult.First());
	}

	public GameObject FindSuperfoodTarget() {
		// 제일 많은 종류의 음식 중 하나를 픽
		Dictionary<FoodType, int> counter = new Dictionary<FoodType, int>();
		List<FoodOnTray> foodList = new List<FoodOnTray>();
		for (int row = 0; row < ROW; row++)
		{
			for (int col = 0; col < COL; col++)
			{
				foodList.Add(foods[row, col]);
			}
		}
		foodList = foodList.FindAll(food => food != null && !food.isServed && !food.isSuperfood);
		foodList.ForEach(food => {
			FoodType type = food.foodType;
			if (counter.ContainsKey(type))
			{
				int count = counter[type];
				counter[type] = count + 1;
			}
			else
			{
				counter.Add(type, 1);
			}
		});

		KeyValuePair<FoodType, int> maxValuePair = counter.First();
		foreach (var pair in counter)
		{
			if (pair.Value > maxValuePair.Value)
			{
				maxValuePair = pair;
			}
		}
		FoodType mostFoodType = maxValuePair.Key;
		var mostFoodTypeFoods = foodList.FindAll(food => !food.isSuperfood && food.foodType == mostFoodType);
		FoodOnTray preSuperfood = mostFoodTypeFoods[Random.Range(0, mostFoodTypeFoods.Count)];

		return preSuperfood.gameObject;
	}

	public void StartRenewTray()
	{
		if (MissionData.gotTrayItem == true)
		{
			StartCoroutine(RenewTray());
			MissionData.gotTrayItem = false;
		}
	}

	public IEnumerator RenewTray()
	{
		yield return new WaitWhile(() => isPlayingRefillAnim);
		yield return new WaitWhile(() => isPlayingMovingAnim);

		for (int row = 0; row < ROW; row++)
		{
			for (int col = 0; col < COL; col++)
			{
				if(!foods[row, col].gameObject.GetComponent<FoodOnTray>().isSuperfood)
				{
					Destroy(foods[row, col].gameObject);
					foods[row, col] = null;
				}
			}
		}

		yield return StartCoroutine(RefillFoods());
	}

	void ShowMatchingEffect(List<FoodOnTray> foods) {
		Vector3 avgPos = Vector3.zero;
		foods.ForEach(food => avgPos += food.transform.position/4f);

		Instantiate(matchingEffect, avgPos, Quaternion.identity);
	}

	void ShowComboText (List<FoodOnTray> foods) {
		Vector3 avgPos = Vector3.zero;
		foods.ForEach(food => avgPos += food.transform.position/4f);

		GameObject comboTextObj = Instantiate(comboTextPrefab, avgPos, Quaternion.identity);
		comboTextObj.GetComponentInChildren<Text>().text = comboCount + "Combo!";
		comboTextObj.transform.DOJump(avgPos, 1, 1, 0.5f);
		Destroy(comboTextObj, 1);
	}

	bool IsComboCountUp () {
		if (moveCountAfterMatching == 1) {
			if (lastComboTime < comboDelayByMoving) {
				// Debug.Log("Combo by one move. ComboCount " + comboCount + " -> " + (comboCount+1));
				return true;
			}
			else {
				return false;
			}
		}
		else {
			if (lastComboTime < comboDelay) {
				// Debug.Log("Combo by time. ComboCount " + comboCount + " -> " + (comboCount+1));
				return true;
			}
			else {
				return false;
			}
		}
	}

	public bool NoMatchingFoods() {
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
					foods[row, col].isFoodMoving = true;
					foods[row+1, col] = null;
				}
			}
			// 맨 윗줄일 경우
			else {
				GameObject newFood = Instantiate(foodObj, foodPoses[row, col].position, Quaternion.identity);
				newFood.GetComponent<FoodOnTray>().foodCoord = new Vector2(row, col);
				foods[row, col] = newFood.GetComponent<FoodOnTray>();
				// 가장 적은 음식을 생성해야 하는 보정이 있으면 그 음식을 지정해서 생성
				if (specialCountAtRefill > 0) {
					FoodType leastType = FindLeastFoodType();
					newFood.GetComponent<FoodOnTray>().Initialize(leastType);
				}
				else {
					newFood.GetComponent<FoodOnTray>().Initialize();
					newFood.GetComponent<FoodOnTray>().isFoodMoving = true;
				}
				newFood.transform.DOScale(0.1f, 0);
				newFood.transform.DOScale(1, 0.2f);
			}
			// 플레이어가 들고 있는 음식이 판에서 움직이게 될 경우 돌아갈 자리를 재선정
			if (pickedFood1 != null)
			{
				if (pickedFood1.GetComponent<FoodOnTray>().isFoodMoving)
				{
					pickedFood1Origin = foodPoses[(int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.x,
					(int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.y].position;
				}
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

	public IEnumerator RefillFoods() {
		isPlayingRefillAnim = true;
		specialCountAtRefill = 0;
		while (!IsTrayFull()) {
			for (int row = 0; row < ROW; row++) {
				for (int col = 0; col < COL; col++) {
					CheckAndRefill(row, col);
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
		foreach (var food in foods)
			food.isFoodMoving = false;
		
		// 판 자동 리셋 체크
		if (NoMatchingFoods())
		{
			for (int row = 0; row < ROW; row++)
			{
				for (int col = 0; col < COL; col++)
				{
					Destroy(foods[row, col].gameObject);
					foods[row, col] = null;
				}
			}

			yield return StartCoroutine(RefillFoods());
		}

		isPlayingRefillAnim = false;
	}

	int specialCountAtRefill = 0;

	public IEnumerator RefillFoodByBin() {
		isPlayingRefillAnim = true;
		specialCountAtRefill = 1;
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

	FoodType FindLeastFoodType() {
		// 제일 적은 종류의 음식타입을 찾음 (만능음식 제외)
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
		// 한종류도 없는 음식을 보정
		for (int i = 0; i < MissionData.foodTypeCount; i++) {
			FoodType type = (FoodType)i;
			if (!counter.ContainsKey(type)) {
				counter.Add(type, 0);
			}
		}
		
		KeyValuePair<FoodType, int> minValuePair = counter.First();
		foreach (var pair in counter) {
			if (pair.Value < minValuePair.Value) {
				minValuePair = pair;
			}
		}
		FoodType leastFoodType = minValuePair.Key;

		return leastFoodType;
	}

	List<ServedPair> pairs = new List<ServedPair>();

	// 맞는 페어가 있는지 찾는 함수를 따로 분리
	public List<ServedPair> FindMatchingPairs() {
		List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
		customers = customers.OrderBy(customer => customer.remainWaitingTime).ToList(); 
		pairs.Clear();

		// float animDelay = 1;

		// 하나씩 맞춰보고 (위 > 아래, 왼쪽 > 오른쪽)
		for (int row = ROW-2; row >= 0; row--) {
			for (int col = 0; col < COL-1; col++) {
				List<FoodOnTray> foodsInPart = new List<FoodOnTray>();
				foodsInPart.Add(foods[row, col]);
				foodsInPart.Add(foods[row+1, col]);
				foodsInPart.Add(foods[row, col+1]);
				foodsInPart.Add(foods[row+1, col+1]);
				// null인 음식과 served인 음식(=다른 손님에게 서빙될 예정), 플레이어가 집고 있는 음식을 제외
				if (pickedFood1 != null)
					foodsInPart = foodsInPart.FindAll(food => food != null && !food.isServed
						&& food.foodCoord != pickedFood1.GetComponent<FoodOnTray>().foodCoord);
				else foodsInPart = foodsInPart.FindAll(food => food != null && !food.isServed);
			
				List<Customer> matchedCustomers = customers.FindAll(customer => 
					!customer.isServed 
					&& MatchEachPartWithCustomer(foodsInPart, customer.orderedFoods.Select(orderedFood => orderedFood.foodType).ToList()));
				
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

		return pairs;
	}

	public IEnumerator MatchingPairs() {
		float animDelay = 1;
		// float comboAnimDelay = 0.2f;

		customerManager.isPlayingCustomerAnim = true;

		foreach (var pair in pairs) {
			Customer matchedCustomer = pair.customer;
			List<FoodOnTray> matchedFoods = pair.foods;

			// 자신의 남은 참을성에 비례해 피버게이지 오르는 부분 (쓰지 않음)
			// feverManager.AddFeverAmountByCustomer(matchedCustomer);
			// 남은 손님의 참을성에 비례해 피버게이지 오르는 부분 (쓰지 않음)
			// List<Customer> remainCustomer = customerManager.currentWaitingCustomers.ToList()
			// 								.FindAll(customer => customer != null && !customer.isServed);
			// remainCustomer.ForEach(customer => feverManager.AddFeverAmountByCustomer(customer));

			// 4개 중 이미 마킹된 음식이 있으면 무조건 매칭 false를 리턴한다
			if (matchedCustomer.orderedFoods.Any(food => food.foundCorrespondent)) yield break;

			// 타입이 같은 음식의 correspondent를 대응시킨다. (일반음식)
			foreach (var matchedFood in matchedFoods)
			{
				if (!matchedFood.isSuperfood)
				{
					var corrFoodInOrder = matchedCustomer.orderedFoods.Find(FoodInOrder =>
						FoodInOrder.foodType == matchedFood.foodType &&
						!FoodInOrder.foundCorrespondent);
					// 완벽히 같은 손님과 매칭되지 않도록 함
					if (matchedFood.correspondent == null)
					{
						matchedFood.correspondent = corrFoodInOrder;

						// 트레이 음식 여럿이 주문판 음식 하나에 계속 대응되지 않도록 마킹.
						if (!corrFoodInOrder.foundCorrespondent)
							corrFoodInOrder.foundCorrespondent = true;
					}
				}
			}
			// 만능음식은 그냥 남아있는 아무 음식의 correspondent를 대응시킨다.
			foreach (var matchedFood in matchedFoods)
			{
				if (matchedFood.isSuperfood)
				{
					var corrFoodInOrder = matchedCustomer.orderedFoods.Find(FoodInOrder =>
						!FoodInOrder.foundCorrespondent);
					matchedFood.correspondent = corrFoodInOrder;

					corrFoodInOrder.foundCorrespondent = true;
				}
			}

			ShowMatchingEffect(matchedFoods);

			if (IsComboCountUp()) {
				comboCount++;
			}
			else {
				comboCount = 1;
			}
			SoundManager.PlayCombo(comboCount);

			lastComboTime = 0;

			if (comboCount > 1) {
				ShowComboText(matchedFoods);
				// 콤보에 따라 피버게이지 오르는 부분 (쓰지 않음)
				// feverManager.AddFeverAmountByCombo(comboCount-1);
			}

			List<Customer> customers = customerManager.currentWaitingCustomers.ToList().FindAll(customer => customer != null);
			yield return StartCoroutine(MatchAnimation(matchedFoods, matchedCustomer, customers, animDelay));

			moveCountAfterMatching = 0;

			// yield return new WaitForSeconds(comboAnimDelay);
		}
		yield return new WaitForSeconds(animDelay);
		customerManager.isPlayingCustomerAnim = false;
	}

	public IEnumerator TryMatch() {
		if (isPlayingRefillAnim)
			yield return new WaitWhile(() => isPlayingRefillAnim);
		if (isTryingMatch)
			yield return new WaitWhile(() => isTryingMatch);

		isTryingMatch = true;

		FindMatchingPairs();

		if (pairs.Count > 0) {
			yield return StartCoroutine(MatchingPairs());
		}
			
		// 해당되는 음식 리필
		yield return StartCoroutine(RefillFoods());

		isTryingMatch = false;
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

		if(matchedCustomer != null)
		{
			SoundManager.PlayCustomerReaction(matchedCustomer.rabbitData.gender, true);

			//매칭되어 나가는 도중 분노 떨기를 시작하지 못하도록 함
			matchedCustomer.isServeCompleted = true;
			matchedCustomer.timerImage.fillMethod = Image.FillMethod.Vertical;
			matchedCustomer.timerImage.fillAmount = 0f;
			matchedCustomer.timerImage.DOFillAmount(1f, animDelay/2f);
			Tween tw = matchedCustomer.timerImage.DOColor(Color.yellow, animDelay / 2f);
			yield return tw.WaitForCompletion();
			// 날아가는 동안 기다리도록: 연동이 되는 게 아니라 입력된 시간 그대로 기다리는 방식
			// yield return new WaitForSeconds(animDelay / 2f);

			// 날아간 음식 제거
			foreach (var matchedFood in matchedFoods)
				Destroy(matchedFood.gameObject);

			if(matchedCustomer != null)
			{
				// vip 이미지 제거
				matchedCustomer.vipImage.enabled = false;

				// 주문판과 주문판에 있는 음식 제거
				foreach (var orderAspect in matchedCustomer.orderToBeDestroyed)
					orderAspect.SetActive(false);
				
				if(matchedCustomer != null)
				{					
					// 손님 보내고: 왼쪽 손님은 exitAmount만큼 왼쪽으로, 오른쪽 손님은 exitAmount만큼 오른쪽으로
					matchedCustomer.customerImage.transform.DOJump(
						new Vector3(matchedCustomer.transform.position.x > 0 ? matchedCustomer.transform.position.x + exitAmount :
						matchedCustomer.transform.position.x - exitAmount, matchedCustomer.transform.position.y, 0.0f), 0.5f, 3, animDelay);
					customerManager.RemoveCustomerByMatching(matchedCustomer.indexInArray, animDelay);
					customers.Remove(matchedCustomer);
					// 이미지 사용중이라는 정보 제거
					// RabbitInformation.RemoveRabbitIndex(matchedCustomer.rabbitIndex);
				}

			}

		}
		else
		{
			foreach (var matchedFood in matchedFoods)
				Destroy(matchedFood.gameObject);
		}

	}

	bool MatchEachPartWithCustomer(List<FoodOnTray> foodsInPart, List<FoodType> orderedFood) {
		// 빈칸 등의 이유로 판에 있는 2*2 영역의 음식 갯수가 4보다 작을 경우에는 무조건 매칭 false 리턴
		if (foodsInPart.Count < 4) return false;

		// if (!corrFoodInOrder.foundCorrespondent) 부분의 null 에러를 막기 위해
		// 4개 중 이미 마킹된 음식이 있으면 무조건 매칭 false를 리턴한다
		// if (foodsInOrder.Any(food => food.foundCorrespondent)) return false;
		// Matching state마다 전체 손님 오더의 foundCorrestpondent를 리셋하도록 코드 추가.
		// 여기서 더이상 체크하지 않음. 혹시 나중에 문제생길 경우를 대비해서 보존.

		// 만능음식은 갯수만 세어놓는다
		int numberOfSuperfood = foodsInPart.Count(food => food.isSuperfood);		
		List<FoodType> foodsTypeOnTray = new List<FoodType>();
		foodsInPart.ForEach(food => {
			if (!food.isSuperfood) {
				foodsTypeOnTray.Add(food.foodType);
			}
		});

		int remainSuperfoodCount = numberOfSuperfood;
		// 주문판의 4개 음식이 트레이 2*2 영역에 있는 음식과 일치하는지 판정하는 부분
		// 판정이 실패했을 때 만능음식이 있으면 하나 쓴다
		foreach (var foodInOrder in orderedFood) {
			bool isThereMatchedFoodType = foodsTypeOnTray.Any(foodTypeOnTray => foodTypeOnTray == foodInOrder);
			if (isThereMatchedFoodType) {
				foodsTypeOnTray.Remove(foodInOrder);
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


		return true;
	}

	float moveSpeed = 0.2f;

	IEnumerator ChangeFoodPosition(GameObject food1, Vector3 food1Origin, GameObject food2 = null ) {
		gameStateManager.gameState = GameState.Change;

		isPlayingMovingAnim = true;

		// food1이 움직이고 있는 경우, food1Origin을 옮기고 기다렸다가 작동
		if (food1.GetComponent<FoodOnTray>().isFoodMoving)
		{
			yield return new WaitWhile(() => isPlayingRefillAnim);
		}

		if(food2 != null)
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

		gameStateManager.gameState = GameState.Matching;

		// yield return StartCoroutine(TryMatch());

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
				newFood.GetComponent<FoodOnTray>().Initialize();
			}
		}

		// 맨 아랫줄 막기
		for (int row = 0; row < 1; row++) {
			for (int col = 0; col < COL; col++) {
				foods[row, col].GetComponent<FoodOnTray>().InitializeBlockObject();
			}
		}
	}

	// Use this for initialization
	void Start () {
		customerManager = FindObjectOfType<CustomerManager>();
		gameManager = FindObjectOfType<GameManager>();
		missionManager = FindObjectOfType<MissionManager>();
		// feverManager = FindObjectOfType<FeverManager>();
		gameStateManager = FindObjectOfType<GameStateManager>();

		SetAllPossibleOrders();

		InitializeFoods();
		
		lastComboTime = comboDelayByMoving + 1;
	}

	public IEnumerator PickFood(RaycastHit2D hit) {		
		SoundManager.Play(SoundType.Swap);

		pickedFood1 = hit.collider.gameObject;
		if(!pickedFood1.GetComponent<FoodOnTray>().isEnlarging) {
			yield return StartCoroutine(EnlargePickedFood(pickedFood1));
		}
		pickedFood1.GetComponent<FoodOnTray>().isEnlarging = true;
		pickedFood1.GetComponent<HighlightBorder>().ActiveBorder();
		pickedFood1Origin = foodPoses[(int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.x, 
										(int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.y].position;
	}

	public void ViewPickedFood() {
		Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

		if (pickedFood1 != null)
		{
			// 집은 음식 마우스/손가락 따라다니게 하기
			pickedFood1.transform.position = new Vector3(worldPoint.x, worldPoint.y, -3);
			// 다른 음식과 겹치게 움직이면 겹쳐진 음식을 교체 예정으로 보고 미리 반투명하게 예상 결과를 보여준다.
			if (hit.Length > 1 && hit[1].collider != null)
			{
				// 음식이 아닌 경우 보여주지 않음
				if (!hit[1].collider.GetComponent<FoodOnTray>().isFood) return;
				toBeSwitched.SetActive(true);
				toBeSwitched.GetComponent<SpriteRenderer>().sprite = hit[1].collider.gameObject.GetComponent<SpriteRenderer>().sprite;
				toBeSwitched.transform.localScale = hit[1].collider.gameObject.transform.localScale;
				toBeSwitched.transform.position = pickedFood1Origin;
			}
		}
	}

	public IEnumerator ValidDrop(RaycastHit2D hit) {
		if (hit.collider != null && !hit.collider.gameObject.GetComponent<FoodOnTray>().isFoodMoving)
			pickedFood2 = hit.collider.gameObject;

		if ((pickedFood1 != null) && (pickedFood2 != null))
		{
			// 유효이동일 경우에만 카운트 상승
			SoundManager.Play(SoundType.Swap);
			moveCountAfterMatching++;
			// 이동 성공 시 터치카운트를 1 올림
			missionManager.currentTouchCount++;
			missionManager.touchText.text = missionManager.currentTouchCount.ToString("N0") + "/" + missionManager.touchCount;
			StartCoroutine(missionManager.TextAnimation(missionManager.touchText));
			yield return StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin, pickedFood2));
		}

		//집었던 거 초기화
		if (pickedFood1 != null)
			pickedFood1.transform.localScale = new Vector3(firstScaleX, firstScaleY);
	}

	public void BinDrop() {
		// 쓰레기통에 버려도 터치카운트를 1 올림
		missionManager.currentTouchCount++;
		missionManager.touchText.text = missionManager.currentTouchCount.ToString("N0") + "/" + missionManager.touchCount;
		StartCoroutine(missionManager.TextAnimation(missionManager.touchText));
		Destroy(pickedFood1);
		int posX = (int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.x;
		int posY = (int)pickedFood1.GetComponent<FoodOnTray>().foodCoord.y;
		foods[posX, posY] = null;
		pickedFood1 = null;

		//집었던 거 초기화
		if(pickedFood1 != null)
			pickedFood1.transform.localScale = new Vector3(firstScaleX, firstScaleY);
	}

	public IEnumerator InvalidDrop() {
		//집었던 거 초기화
		if (pickedFood1 != null) {
			pickedFood1.transform.localScale = new Vector3(firstScaleX, firstScaleY);
			yield return StartCoroutine(ChangeFoodPosition(pickedFood1, pickedFood1Origin));
		}
	}

	public void DropFood() {
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
				gameStateManager.ValidTrigger(hit[1]);
			}
			else if(isOnBin)
			{
				gameStateManager.BinTrigger();
			}
			else
			{
				// gameStateManager.InvalidTrigger();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		if (Input.GetMouseButtonDown(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

			//If something was hit, the RaycastHit2D.collider will not be null.
			if ((hit.collider != null) && (!isPlayingMovingAnim))
			{
				gameStateManager.PickedTrigger(hit);
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			gameStateManager.DroppedTrigger();
		}

		if (!Input.anyKey) {
			gameStateManager.DroppedTrigger();	
		}

		if (gameStateManager.gameState == GameState.Idle || gameStateManager.gameState == GameState.Picked)
			lastComboTime += Time.deltaTime;
	}
}
