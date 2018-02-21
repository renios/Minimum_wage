using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StageButton : MonoBehaviour {

	public int stageIndex;

	public void Initialize() {
		string[] parsedNameString = gameObject.name.Split('-');
		stageIndex = 10*(Convert.ToInt32(parsedNameString[0])-1) + Convert.ToInt32(parsedNameString[1]);
		GetComponent<Button>().interactable = false;
	}

	public void Active() {
		GetComponent<Button>().interactable = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
