using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Enums;

public class MenuManager : MonoBehaviour {

	public GameObject panel;
	public Image panelBg;
	public Image fadeoutPanel;

	float delay = 0.5f;
	Vector3 originPos;

	GameStateManager gameStateManager;
	GameState prevGameState;

	public void ActiveMenuPanel() {
		prevGameState = gameStateManager.gameState;
		gameStateManager.gameState = GameState.Paused;
		StartCoroutine(ActiveMenuPanelCoroutine());
	}

	IEnumerator ActiveMenuPanelCoroutine() {
		Time.timeScale = 0;
		SoundManager.PauseSoundPlayers();
		panelBg.GetComponent<Image>().raycastTarget = true;
		panelBg.DOFade(0.7f, delay).SetUpdate(UpdateType.Normal, true);
		panel.GetComponent<RectTransform>().DOMove(Vector3.zero, delay).SetUpdate(UpdateType.Normal, true);
		yield return null;
	}

	public void InactiveMenuPanel() {
		StartCoroutine(InactiveMenuPanelCoroutine());
	}

	IEnumerator InactiveMenuPanelCoroutine() {
		panelBg.DOFade(0, delay).SetUpdate(UpdateType.Normal, true);
		Tween tw = panel.GetComponent<RectTransform>().DOMove(originPos, delay).SetUpdate(UpdateType.Normal, true);
		yield return tw.WaitForCompletion();
		panelBg.GetComponent<Image>().raycastTarget = false;
		SoundManager.UnpauseSoundPlayers();
		Time.timeScale = 1;

		gameStateManager.gameState = prevGameState;
	}

	public void GoToWorld() {
		StartCoroutine(GoToWorldCoroutine());
	}

	IEnumerator GoToWorldCoroutine() {
		Tween tw = fadeoutPanel.DOFade(1, delay).SetUpdate(UpdateType.Normal, true);
        if (MissionData.gotTimeItem)
        {
            PlayerPrefs.SetInt("TimerReset", PlayerPrefs.GetInt("TimerReset", 0) + 1);
            MissionData.gotTimeItem = false;
        }
        if (MissionData.gotSuperfood)
        {
            PlayerPrefs.SetInt("Superfood", PlayerPrefs.GetInt("Superfood", 0) + 1);
            MissionData.gotSuperfood = false;
        }
        if (MissionData.gotTrayItem)
        {
            PlayerPrefs.SetInt("TrayReset", PlayerPrefs.GetInt("TrayReset", 0) + 1);
            MissionData.gotTrayItem = false;
        }
		yield return tw.WaitForCompletion();
		tw = fadeoutPanel.DOFade(1, 1).SetUpdate(UpdateType.Normal, true);
		yield return tw.WaitForCompletion();
		Time.timeScale = 1;
		SceneManager.LoadScene("World");
	}

	// Use this for initialization
	void Start () {
		gameStateManager = FindObjectOfType<GameStateManager>();
		originPos = panel.GetComponent<RectTransform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameStateManager.gameState != GameState.Paused)
                ActiveMenuPanel();
            else if (gameStateManager.gameState == GameState.Paused)
                InactiveMenuPanel();
        }
	}
}
