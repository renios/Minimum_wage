using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

	public float waitingTime;
	float remainWaitingTime;
	public Image timerImage;

	List<GameObject> orderedFoods;

	void InitializeTimer() {
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
	void Start () {
		MakeOrder();
		InitializeTimer();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTimer();
	}
}
