using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

	public int indexInArray;

	public float waitingTime;
	float remainWaitingTime;
	public Image timerImage;

	List<GameObject> orderedFoods;

	bool initialized = false;

	void InitializeTimer(float waitingTime) {
		remainWaitingTime = waitingTime;
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void UpdateTimer() {
		remainWaitingTime -= Time.deltaTime;
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void MakeOrder() {
		orderedFoods = new List<GameObject>();
		GetComponentsInChildren<FoodInOrder>().ToList().ForEach(fio => orderedFoods.Add(fio.gameObject));
	}

	// Use this for initialization
	public void Initialize (int indexInArray, float waitingTime) {
		this.indexInArray = indexInArray;

		MakeOrder();
		InitializeTimer(waitingTime);

		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialized) return;
	
		UpdateTimer();
	}
}
