﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameStateManager : MonoBehaviour {

	public GameState gameState;

	GameManager gameManager;
	TrayManager trayManager;
	FeverManager feverManager;
	MissionManager missionManager;
	HeartManager heartManager;
	CustomerManager customerManager;
	TutorialManager tutorialManager;

	IEnumerator StartGame() {
		// 카운트다운을 세고 게임을 시작한다
		yield return StartCoroutine(gameManager.StartByGSM());
		gameState = GameState.Idle;
		yield return StartCoroutine(Idle());
	}

	bool pickedTrigger = false;
	RaycastHit2D pickedFood;
	bool newCustomerTrigger = false;

	public void PickedTrigger(RaycastHit2D hit) {
		if (gameState == GameState.Idle && !pickedTrigger) {
			// 음식이 아닌 경우 집지 않음
			if (!hit.collider.GetComponent<FoodOnTray>().isFood) return;

			// 튜토리얼 스텝에 따라 집을지 결정
			if (tutorialManager != null) {
				if (tutorialManager.tutorialStep == 2 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(1, 3)) 
					return;

				if (tutorialManager.tutorialStep == 7 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(4, 3)) 
					return;

				if (tutorialManager.tutorialStep == 10 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(2, 1)) 
					return;

				if (tutorialManager.tutorialStep != 2 &&
				    tutorialManager.tutorialStep != 7 &&
				    tutorialManager.tutorialStep != 10 &&
				    tutorialManager.tutorialStep < 18)
					return;
			}
			pickedFood = hit;
			pickedTrigger = true;
		}
	}

	public void NewCustomerTrigger() {
		if (gameState == GameState.Idle && !newCustomerTrigger) {
			newCustomerTrigger = true;
		}
	}

	public IEnumerator Idle() {
		while (gameState == GameState.Idle || gameState == GameState.UseItem
		|| gameState == GameState.Paused) {
			// 아이템을 썼거나 옵션 버튼을 누르면 GSM이 아닌 바깥에서 state 통제(대신 이 코루틴이 끝나버리지 않도록 홀드)
			if (gameState == GameState.UseItem || gameState == GameState.Paused)
			{
				yield return new WaitUntil(() => gameState == GameState.Idle);
			}

			// 게임 종료 조건 체크
			yield return StartCoroutine(missionManager.CheckGameEnd());
			yield return StartCoroutine(heartManager.CheckGameEnd());

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
			// 유효픽일 경우 튜토리얼 진행도 올림
			if (tutorialManager != null) {
				if ((tutorialManager.tutorialStep == 2) ||
					(tutorialManager.tutorialStep == 7) ||
					(tutorialManager.tutorialStep == 10))
				{
					tutorialManager.tutorialStep += 1;
				}
			}

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
	bool binTrigger = false;
	bool invalidTrigger = false;

	public void ValidTrigger(RaycastHit2D hit) {
		if (gameState == GameState.Picked && !validTrigger) {
			// 음식이 아닌 경우 유효이동 처리 안함
			if (!hit.collider.GetComponent<FoodOnTray>().isFood) return;

			// 튜토리얼 스텝에 따라 유효이동이 아닌 경우가 있음
			if (tutorialManager != null) {
				if (tutorialManager.tutorialStep == 3 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(3, 3)) 
					return;

				if (tutorialManager.tutorialStep == 8 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(2, 4)) 
					return;

				if (tutorialManager.tutorialStep == 11 && 
					hit.collider.GetComponent<FoodOnTray>().foodCoord != new Vector2(1, 4)) 
					return;
			}

			castedObj = hit;
			validTrigger = true;
		}
	}

	public void BinTrigger() {
		if (gameState == GameState.Picked && !binTrigger) {
			// 튜토리얼에서는 쓰레기통 비활성화
			if (tutorialManager != null) return;

			binTrigger = true;
		}
	}

	IEnumerator Dropped() {
		while (gameState == GameState.Dropped) {
			// 유효한 이동일 경우 -> Change -> Matching
			if (validTrigger) {
				// 유효드랍일 경우 튜토리얼 진행도 올림
				if (tutorialManager != null) {
					if ((tutorialManager.tutorialStep == 3) ||
						(tutorialManager.tutorialStep == 8) ||
						(tutorialManager.tutorialStep == 11))
					{
						tutorialManager.tutorialStep += 1;
					}
				}

				yield return StartCoroutine(trayManager.ValidDrop(castedObj));
				validTrigger = false;
				yield return StartCoroutine(Matching());
			}

			// 쓰레기통에 버릴 경우 -> Refill
			if (binTrigger) {
				trayManager.BinDrop();
				binTrigger = false;
				gameState = GameState.Refill;
				yield return StartCoroutine(Refill(GameState.Dropped));
			}

			// 유효하지 않은 이동일 경우 -> 음식을 원위치시키고 Idle로
			// 무효 드랍일 경우 튜토리얼 진행도 내림
			if (tutorialManager != null) {
				if ((tutorialManager.tutorialStep == 3) ||
					(tutorialManager.tutorialStep == 8) ||
					(tutorialManager.tutorialStep == 11))
				{
					tutorialManager.tutorialStep -= 1;
				}
			}
			// 음식 원위치
			yield return StartCoroutine(trayManager.InvalidDrop());
			gameState = GameState.Idle;
			yield break;
		}
	}

	IEnumerator Matching() {
		while (gameState == GameState.Matching) {
			// 매칭 시도
			// 각 손님의 모든 foundCorrespondent를 초기화
			customerManager.ResetFoundCorrespondentEachOrder();
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
			yield return StartCoroutine(Refill(GameState.Combo));
		}
	}

	IEnumerator Refill(GameState preState) {
		while (gameState == GameState.Refill) {
			// 매칭이 끝나고, 피버 보너스 만능음식이 생성될 경우 생성한다
			// gameState = GameState.FeverBonus;
			// yield return StartCoroutine(FeverBonus());
			// gameState = GameState.Matching;

			// 리필이 끝나면, 다시 매칭
			// 쓰레기통에 버린 경우 : 가장 적은 종류의 음식을 생성하고 강제로 matching으로.
			if (preState == GameState.Dropped) {
				yield return StartCoroutine(trayManager.RefillFoodByBin());
				gameState = GameState.Matching;
				yield return StartCoroutine(Matching());
			}
			// 일반적인 경우 : 판에 빈 자리가 없을때까지 리필한다. 자연스럽게 matching으로 돌아간다.
			else {
				yield return StartCoroutine(trayManager.RefillFoods());
				gameState = GameState.Matching;
				yield break;
			}
		}
	}

	IEnumerator FeverBonus() {
		yield return StartCoroutine(feverManager.CheckFeverPoint());
	}

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		trayManager = FindObjectOfType<TrayManager>();
		feverManager = FindObjectOfType<FeverManager>();
		missionManager = FindObjectOfType<MissionManager>();
		heartManager = FindObjectOfType<HeartManager>();
		customerManager = FindObjectOfType<CustomerManager>();
		tutorialManager = FindObjectOfType<TutorialManager>();

		gameState = GameState.Start;
		StartCoroutine(StartGame());
	}

	// Update is called once per frame
	void Update () {
		
	}
}
