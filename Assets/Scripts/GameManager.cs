using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Enums;

public class GameManager : MonoBehaviour {

	public GameObject gameoverCanvas;
	public GameObject startCanvas;
	public Image bgPanel;
	public Image mainPanel;
	public Text textInCanvas;
	public Sprite gameoverSprite;
	public Sprite clearSprite;

	public bool isPlaying = false;

	float delay = 0.5f;

	GameStateManager gameStateManager;

	public IEnumerator ShowGameoverCanvas() {
		isPlaying = false;
		SoundManager.Play(MusicType.StageOver);
		gameoverCanvas.SetActive(true);
		Vector3 startPos = new Vector3(Screen.width / 2, Screen.height * 1.5f, 0);
		mainPanel.transform.DOMove(startPos, 0);
		mainPanel.sprite = gameoverSprite;
		//textInCanvas.text = "Game Over" + '\n' + '\n' + "Touch the Screen";
		bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		Tween tw = mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return tw.WaitForCompletion();
	}

	public IEnumerator ShowClearCanvas() {
		isPlaying = false;
		SoundManager.Play(MusicType.StageClear);
		gameoverCanvas.SetActive(true);
		Vector3 startPos = new Vector3(Screen.width / 2, Screen.height * 1.5f, 0);
		mainPanel.transform.DOMove(startPos, 0);
		mainPanel.sprite = clearSprite;
		//textInCanvas.text = "Mission Clear" + '\n' + '\n' + "Touch the Screen";
		bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
		Tween tw = mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return tw.WaitForCompletion();
	}

	IEnumerator StartAnimation()
	{
		SoundManager.Play(MusicType.Start);
		startCanvas.SetActive(true);
		startCanvas.GetComponent<Animator>().Play("StartCountdown");

		// 숫자로 말고 애니메이션 크기에 따라서 애니메이션 끝날 때까지 기다리게 하고 싶은데 어떻게 해야 하는지 모르겠음
		yield return new WaitForSeconds(4f);

		SoundManager.Play(MusicType.Ambient);
		startCanvas.SetActive(false);
		isPlaying = true;
		yield return null;
	}

	void Awake() {
		isPlaying = false;
	}

	void Start () {
		gameStateManager = FindObjectOfType<GameStateManager>();
	}

	// Use this for initialization
	public IEnumerator StartByGSM () {
		yield return StartCoroutine(StartAnimation());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStateManager.gameState != GameState.End) return;

		if (gameoverCanvas.activeInHierarchy) {
			if (Input.anyKeyDown && gameStateManager.gameState == GameState.End)
				StartCoroutine(HideCanvas());
		}
	}

	IEnumerator HideCanvas () {
		// bgPanel.DOFade(0, delay);
		Tween tw = mainPanel.DOColor(Color.black, delay);
		yield return tw.WaitForCompletion();
		isPlaying = false;
		MissionData.gotSuperfood = false;
		MissionData.gotTimeItem = false;
		MissionData.gotTrayItem = false;
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("World");
		// yield return null;
	}
}
