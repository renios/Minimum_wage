using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

public class CustomerManager : MonoBehaviour {

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

	GameManager gameManager;
	GameStateManager gameStateManager;
	HeartManager heartManager;
	TrayManager trayManager;
	CoinManager coinManager;
	MissionManager missionManager;

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
		Destroy(currentWaitingCustomers[indexInArray].gameObject);
		currentWaitingCustomers[indexInArray] = null;
		heartManager.ReduceHeart(1);
	}

	void MakeCoinParticle(Vector3 pos, float delay) {
		Vector3 prefabPos = pos + Vector3.down/2f;
		Instantiate(successEffectPrefab, prefabPos, Quaternion.identity);
		Instantiate(coinPrefab, prefabPos, Quaternion.identity);
	}

	public void RemoveCustomerByMatching(int indexInArray, float delay) {
		currentWaitingCustomers[indexInArray].isServeCompleted = true;

		MakeCoinParticle(currentWaitingCustomers[indexInArray].transform.position, delay);

		Destroy(currentWaitingCustomers[indexInArray].gameObject, delay);
		currentWaitingCustomers[indexInArray] = null;
		missionManager.successCustomerCount++;
		coinManager.AddCoin(100);
	}

	void MakeNewCustomer(int indexInArray, Transform parentTransform) {
		GameObject customerObj = Instantiate(customerPrefab, parentTransform.position, Quaternion.identity);
		customerObj.transform.parent = parentTransform;
		customerObj.transform.localScale = Vector3.one;
		Customer customer = customerObj.GetComponent<Customer>();
		customer.Initialize(indexInArray, this.waitingTime);
		customer.toleranceRate = toleranceRate;
		customer.maxFuryRate = maxFuryRate;
		if(IsCustomerSlotEmpty()){
			var lists = trayManager.GetTraysNotOnFoods();
			customer.SetOrder(lists[Random.Range(0,lists.Count)]);
		}
		AddCustomerInEmptySlot(customer);
		lastCustomerMakeTime = 0;
	}

	void AddCustomerInEmptySlot(Customer newCustomer) {
		for (int i = 0; i < currentWaitingCustomers.Length; i++) {
			if (currentWaitingCustomers[i] == null) {
				currentWaitingCustomers[i] = newCustomer;
				return;
			}
		}
		Debug.LogError("Cannot add new customer in slot");
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

	void Awake () {
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.waitingTime)) {
			waitingTime = missionDataDict[MissionDataType.waitingTime];
		}
		if (missionDataDict.ContainsKey(MissionDataType.customerCooldown)) {
			customerCooldown = missionDataDict[MissionDataType.customerCooldown];
		}
	}

	// Use this for initialization
	void Start () {
		heartManager = FindObjectOfType<HeartManager>();
		trayManager = FindObjectOfType<TrayManager>();
		coinManager = FindObjectOfType<CoinManager>();
		gameManager = FindObjectOfType<GameManager>();
		missionManager = FindObjectOfType<MissionManager>();
		gameStateManager = FindObjectOfType<GameStateManager>();

		lastCustomerMakeTime = customerCooldown - 0.5f;
		isPlayingCustomerAnim = false;
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;

		if (IsEmptyPosInCustomerSlot()) {
			// 손님 리필 쿨타임은 자리가 비어있을 때만 돌아간다
			lastCustomerMakeTime += Time.deltaTime; 

			// 손으로 음식을 집어든 도중에는 손님이 오지 않는다 -> 손님은 오지만, 매칭은 idle 상태가 아니면 하지 않는다
			// if (trayManager.pickedFood1 != null) return;

			if (lastCustomerMakeTime < customerCooldown) return;

			// 손님 추가는 항상 된다
			int emptySlotIndex = GetFirstEmptyPosInCustomerSlot();
			MakeNewCustomer(emptySlotIndex, customerSlot[emptySlotIndex]);
			FindObjectOfType<GameStateManager>().NewCustomerTrigger();

			// 손님 추가는 Idle, Picked 상태일 때만 된다
			// if (gameStateManager.gameState == GameState.Idle || gameStateManager.gameState == GameState.Picked) {
			// 	int emptySlotIndex = GetFirstEmptyPosInCustomerSlot();
			// 	MakeNewCustomer(emptySlotIndex, customerSlot[emptySlotIndex]);
			// 	FindObjectOfType<GameStateManager>().NewCustomerTrigger();
			// }
		}
	}
}
