using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class StarViewer : MonoBehaviour {

	public List<Image> starImages;
	public Sprite activeSprite;
	public Sprite inactiveSprite;

	// Use this for initialization
	public void Start () {
		string[] parsedNameString = transform.parent.name.Split('-');
		int stageIndex = 10*(Convert.ToInt32(parsedNameString[0])-1) + Convert.ToInt32(parsedNameString[1]);
		int starsOfStage = PlayerPrefs.GetInt("StarsOfStage" + stageIndex, 0);

		starImages.ForEach(star => star.GetComponent<Image>().sprite = inactiveSprite);
		for (int i = 0; i < starsOfStage; i++) {
			starImages[i].GetComponent<Image>().sprite = activeSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
