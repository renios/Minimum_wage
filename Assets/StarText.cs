using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StarText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string[] parsedNameString = transform.parent.name.Split('-');
		int stageIndex = 10*(Convert.ToInt32(parsedNameString[0])-1) + Convert.ToInt32(parsedNameString[1]);
		int starsOfStage = PlayerPrefs.GetInt("StarsOfStage" + stageIndex, 0);
		GetComponent<Text>().text = starsOfStage.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
