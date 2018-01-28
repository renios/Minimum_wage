using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager_temp : MonoBehaviour {

	public Text timeText;
	public Text customerText;

	public float remainTime = 90;
	public int successCustomer = 0;
	public int missionCustomer = 20;

	GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();

		timeText.text = (int)(remainTime / 60) + ":" + (int)(remainTime % 60);
		customerText.text = successCustomer + "/" + missionCustomer;
	}
	
	// Update is called once per frame
	void Update () {
		remainTime -= Time.deltaTime;	

		timeText.text = (int)(remainTime / 60) + ":" + (int)(remainTime % 60);
		customerText.text = successCustomer + "/" + missionCustomer;

		if (remainTime <= 0) {
			gameManager.ShowGameoverCanvas();
		}	

		if (successCustomer == missionCustomer) {
			gameManager.ShowClearCanvas();
		}
	}
}
