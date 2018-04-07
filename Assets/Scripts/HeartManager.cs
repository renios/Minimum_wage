using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

public class HeartManager : MonoBehaviour {

	public GameObject heartPrefab;
	public GameObject heartBrokenParticle;
	public List<Transform> heartSlot;
	List<GameObject> hearts = new List<GameObject>();
	int maxHeart = 3;

	GameManager gameManager;
	GameStateManager gameStateManager;

	public void ReduceAllHearts() {
		while (hearts.Count > 0) {
			GameObject lastHeart = hearts.Last();
			GameObject particle = Instantiate(heartBrokenParticle, lastHeart.transform.position, Quaternion.identity);
			hearts.Remove(lastHeart);
			Destroy(particle, 1);
			Destroy(lastHeart);
		}
	}

	public void ReduceHeart(int amount) {
		for (int i = 0; i < amount; i++) {
			if (hearts.Count == 0) return;

			GameObject lastHeart = hearts.Last();
			GameObject particle = Instantiate(heartBrokenParticle, lastHeart.transform.position, Quaternion.identity);
			hearts.Remove(lastHeart);
			Destroy(particle, 1);
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
		gameStateManager = FindObjectOfType<GameStateManager>();

		for (int i = 0; i < maxHeart; i++) {
			GameObject heart = Instantiate(heartPrefab, heartSlot[i]);
			hearts.Add(heart);
		}
	}
	
	public IEnumerator CheckGameEnd() {
		if (gameStateManager.gameState != GameState.Idle) yield break;

		if (hearts.Count <= 0 && !gameManager.gameEndCanvas.activeInHierarchy) {
			gameStateManager.gameState = GameState.Result;
			yield return StartCoroutine(gameManager.ShowGameoverCanvas());
			gameStateManager.gameState = GameState.End;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
