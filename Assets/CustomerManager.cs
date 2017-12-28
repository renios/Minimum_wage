using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerManager : MonoBehaviour {

	public GameObject customerPrefab;
	public List<Transform> customerSlot;

	// Use this for initialization
	void Start () {
		customerSlot.ForEach(t => {
			GameObject customer = Instantiate(customerPrefab, t.position, Quaternion.identity);
			customer.transform.parent = this.transform;
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
