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

	public GameObject[] currentWaitingCustomers;

	void MakeNewCustomer(int indexInArray, Vector3 position) {
		GameObject customer = Instantiate(customerPrefab, position, Quaternion.identity);
		customer.transform.parent = this.transform;
		customer.GetComponent<Customer>().Initialize(indexInArray, this.waitingTime);
		AddCustomerInEmptySlot(customer);
		lastCustomerMakeTime = 0;
	}

	void AddCustomerInEmptySlot(GameObject newCustomer) {
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
		}		
	}
}
