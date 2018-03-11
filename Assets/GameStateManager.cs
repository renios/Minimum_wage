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
	RaycastHit2D pickedFood;
	bool newCustomerTrigger = false;

	public void PickedTrigger(RaycastHit2D hit) {
		if (gameState == GameState.Idle && !pickedTrigger) {
			pickedFood = hit;
			pickedTrigger = true;
		}
	}

	public void NewCustomerTrigger() {
		if (gameState == GameState.Idle && !newCustomerTrigger) {
			newCustomerTrigger = true;
		}
	}

	IEnumerator Idle() {
		while (gameState == GameState.Idle) {
			// 서빙 불가일 때 판 리셋
			if (trayManager.NoMatchingFoods()) {
				gameState = GameState.RenewTray;
				yield return StartCoroutine(trayManager.RenewTray());
				gameState = GameState.Idle;
			}

			// 음식을 집었을 때 -> Picked
			if (pickedTrigger) {
				yield return StartCoroutine(trayManager.PickFood(pickedFood));
				pickedTrigger = false;
				gameState = GameState.Picked;
				yield return StartCoroutine(Picked());
			}

			// 새로운 손님이 들어왔을 때 -> Matching
			if (newCustomerTrigger) {
				newCustomerTrigger = false;
				gameState = GameState.Matching;
				yield return StartCoroutine(Matching());
			}

			// 아이템을 썼을 때 -> ItemManager에서 처리

			yield return null;
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

	bool validTrigger = false;
	RaycastHit2D castedObj;
	bool invalidTrigger = false;

	public void ValidTrigger(RaycastHit2D hit) {
		if (gameState == GameState.Picked && !validTrigger) {
			castedObj = hit;
			validTrigger = true;
		} 
	}

	public void InvalidTrigger() {
		if (gameState == GameState.Picked && !invalidTrigger) {
			invalidTrigger = true;
		}
	}

	IEnumerator Dropped() {
		while (gameState == GameState.Dropped) {
			// 유효한 이동일 경우 -> Change -> Matching
			if (validTrigger) {
				yield return StartCoroutine(trayManager.ValidDrop(castedObj));
				validTrigger = false;
				// gameState = GameState.Change;
				// yield return StartCoroutine(Change());
				yield return StartCoroutine(Matching());
			}

			// 유효하지 않은 이동일 경우 -> 음식을 원위치시키고 Idle로
			// 음식 원위치
			yield return StartCoroutine(trayManager.InvalidDrop());
			invalidTrigger = false;
			gameState = GameState.Idle;
			yield break;
		}
	}

	IEnumerator Matching() {
		while (gameState == GameState.Matching) {
			// 매칭 시도
			List<ServedPair> pairs = trayManager.FindMatchingPairs();
			if (pairs.Count > 0) {
				// 맞는 음식이 있을 경우, 처리할 콤보의 리스트를 만든 후 스테이트 전환 -> combo
				gameState = GameState.Combo;
				yield return StartCoroutine(Combo());
			}
			else {
				// 맞는 음식이 없을 경우, Idle로
				gameState = GameState.Idle;
				yield break;
			}
		}
	}

	IEnumerator Combo() {
		while (gameState == GameState.Combo) {
			// 순차적으로 콤보를 처리하고 피버를 올린다
			yield return StartCoroutine(trayManager.MatchingPairs());

			// 끝나면 리필
			gameState = GameState.Refill;
			yield return StartCoroutine(Refill());
		}
	}

	IEnumerator Refill() {
		while (gameState == GameState.Refill) {
			// 판에 빈 자리가 없을때까지 리필한다
			yield return StartCoroutine(trayManager.RefillFoods());

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
