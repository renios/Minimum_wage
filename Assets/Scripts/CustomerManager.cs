using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerManager : MonoBehaviour {

	public GameObject customerPrefab;
	public List<Transform> customerSlot;
	public float waitingTime;
	public float customerCooldown;
	float lastCustomerMakeTime;

	public Customer[] currentWaitingCustomers;

	HeartManager heartManager;
	TrayManager trayManager;
	CoinManager coinManager;

	public void RemoveCustomerByTimeout(int indexInArray) {
		Destroy(currentWaitingCustomers[indexInArray].gameObject);
		currentWaitingCustomers[indexInArray] = null;
		heartManager.ReduceHeart(1);
	}

	public void RemoveCustomerByMatching(int indexInArray, float delay) {
		Destroy(currentWaitingCustomers[indexInArray].gameObject, delay);
		currentWaitingCustomers[indexInArray] = null;
		coinManager.AddCoin(100);
	}

	void MakeNewCustomer(int indexInArray, Vector3 position) {
		GameObject customerObj = Instantiate(customerPrefab, position, Quaternion.identity);
		customerObj.transform.parent = this.transform;
		Customer customer = customerObj.GetComponent<Customer>();
		customer.Initialize(indexInArray, this.waitingTime);
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

	// Use this for initialization
	void Start () {
		heartManager = FindObjectOfType<HeartManager>();
		trayManager = FindObjectOfType<TrayManager>();
		coinManager = FindObjectOfType<CoinManager>();

		lastCustomerMakeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsEmptyPosInCustomerSlot()) {
			// 손님 리필 쿨타임은 자리가 비어있을 때만 돌아간다
			lastCustomerMakeTime += Time.deltaTime; 

			if (lastCustomerMakeTime < customerCooldown) return;

			int emptySlotIndex = GetFirstEmptyPosInCustomerSlot();
			MakeNewCustomer(emptySlotIndex, customerSlot[emptySlotIndex].position);
			// trayManager.TryMatch();
		}		
	}
}
