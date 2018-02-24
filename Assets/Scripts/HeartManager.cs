﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeartManager : MonoBehaviour {

	public GameObject heartPrefab;
	public List<Transform> heartSlot;
	List<GameObject> hearts = new List<GameObject>();
	int maxHeart = 3;

	GameManager gameManager;

	public void ReduceHeart(int amount) {
		for (int i = 0; i < amount; i++) {
			if (hearts.Count == 0) return;

			GameObject lastHeart = hearts.Last();
			hearts.Remove(lastHeart);
			Destroy(lastHeart);
		}
	}

	void Awake () {
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.maxHeart)) {
			maxHeart = missionDataDict[MissionDataType.maxHeart];
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();

		for (int i = 0; i < maxHeart; i++) {
			GameObject heart = Instantiate(heartPrefab, heartSlot[i]);
			hearts.Add(heart);
		}
	}
	
	

	// Update is called once per frame
	void Update () {
		if (hearts.Count <= 0 && !gameManager.gameoverCanvas.activeInHierarchy) {
			StartCoroutine(gameManager.ShowGameoverCanvas());
		}
	}
}
