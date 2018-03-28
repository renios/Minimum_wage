using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StarManager : MonoBehaviour {

	public Text text;

	int GetTotalStars() {
		int totalStars = 0;
		for (int i = 0; i < 20; i++) {
			string key = "StarsOfStage" + i;
			totalStars += PlayerPrefs.GetInt(key, 0);
		}
		return totalStars;
	}

	// Use this for initialization
	public void Start () {
		int progress = PlayerPrefs.GetInt("Progress", 1);
		int maxStars;
		if (progress < 11) maxStars = 30;
		else maxStars = 60;
		int totalStars = GetTotalStars();

		text.text = totalStars + "/" + maxStars;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
