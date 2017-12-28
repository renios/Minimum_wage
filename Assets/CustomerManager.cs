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
		heartManager.ReduceHeart(1);
	}

	public void RemoveCustomerByMatching(int indexInArray) {
		Destroy(currentWaitingCustomers[indexInArray].gameObject);
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
		// customerSlot.ForEach(t => {
		// 	MakeNewCustomer(t.position);
		// });
	}
	
	// Update is called once per frame
	void Update () {
		lastCustomerMakeTime += Time.deltaTime;

		if (IsEmptyPosInCustomerSlot()) {
			if (lastCustomerMakeTime < customerCooldown) return;

			int emptySlotIndex = GetFirstEmptyPosInCustomerSlot();
			MakeNewCustomer(emptySlotIndex, customerSlot[emptySlotIndex].position);
		
			trayManager.TryMatch();
		}		
	}
}
