using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameStateManager : MonoBehaviour {

	public GameState gameState;

	GameManager gameManager;
	TrayManager trayManager;

	IEnumerator StartGame() {
		// 카운트다운을 세고 게임을 시작한다
		gameManager.StartByGSM();
		gameState = GameState.Idle;
		yield return StartCoroutine(Idle());
	}

	bool pickedTrigger = false;

	public void PickedTrigger() {
		if (gameState == GameState.Idle && !pickedTrigger) {
			pickedTrigger = true;
		}
	}

	IEnumerator Idle() {
		while (gameState == GameState.Idle) {
			// 음식을 집었을 때 -> Picked
			if (pickedTrigger) {
				trayManager.PickFood();
				pickedTrigger = false;
				gameState = GameState.Picked;
				yield return StartCoroutine(Picked());
			}

			// 새로운 손님이 들어왔을 때 -> Matching
			gameState = GameState.Matching;
			yield return StartCoroutine(Matching());

			// 아이템을 썼을 때
		}
	}

	bool droppedTrigger = false;

	public void DroppedTrigger() {
		if (gameState == GameState.Picked && !droppedTrigger) {
			droppedTrigger = true;
		}
	}

	IEnumerator Picked() {
		while (gameState == GameState.Picked) {
			trayManager.ViewPickedFood();

			// 음식을 놓았을 때 -> Dropped
			if (droppedTrigger) {
				trayManager.DropFood();
				droppedTrigger = false;
				gameState = GameState.Dropped;
				yield return StartCoroutine(Dropped());
			}

			yield return null;
		}
	}

	IEnumerator Dropped() {
		while (gameState == GameState.Dropped) {
			// 유효한 이동일 경우 -> Change
			gameState = GameState.Change;
			yield return StartCoroutine(Change());

			// 유효하지 않은 이동일 경우 -> 음식을 원위치시키고 Idle로
			// 음식 원위치

			gameState = GameState.Idle;
		}
	}

	IEnumerator Change() {
		while (gameState == GameState.Change) {
			// 두 음식의 위치를 바꾸고, Matching으로
			// 음식의 위치 서로 바꾸기

			gameState = GameState.Matching;
			yield return StartCoroutine(Matching());
		}
	}

	IEnumerator Matching() {
		while (gameState == GameState.Matching) {
			// 매칭 시도

			// 맞는 음식이 없을 경우, Idle로
			gameState = GameState.Idle;
			yield break;

			// 맞는 음식이 있을 경우, 처리할 콤보의 리스트를 만든 후 스테이트 전환 -> combo
			gameState = GameState.Combo;
			yield return StartCoroutine(Combo());
		}
	}

	IEnumerator Combo() {
		while (gameState == GameState.Combo) {
			// 순차적으로 콤보를 처리하고 피버를 올린다

			// 끝나면 리필
			gameState = GameState.Refill;
			yield return StartCoroutine(Refill());
		}
	}

	IEnumerator Refill() {
		while (gameState == GameState.Refill) {
			// 판에 빈 자리가 없을때까지 리필한다

			// 리필이 끝나면, 다시 매칭
			gameState = GameState.Matching;
			yield break;
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		trayManager = FindObjectOfType<TrayManager>();

		gameState = GameState.Start;
		StartCoroutine(StartGame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
