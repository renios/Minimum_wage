using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Enums;
using System.Linq;

public class GameManager : MonoBehaviour {

	public GameObject gameEndCanvas;
	public GameObject[] starObjects;
	public Sprite starSprite;
	public GameObject startCanvas;
	public Image mainPanel;
	public Sprite gameoverSprite;
	public Sprite clearSprite;

	public bool isPlaying = false;

	float delay = 0.5f;

	GameStateManager gameStateManager;

	public IEnumerator ShowGameoverCanvas() {
		isPlaying = false;
		SoundManager.Play(MusicType.StageOver);
		gameEndCanvas.SetActive(true);
		Vector3 startPos = new Vector3(0, 19.2f, 0);
		mainPanel.transform.DOMove(startPos, 0);
		mainPanel.sprite = gameoverSprite;
		Tween tw = mainPanel.transform.DOMove(Vector3.zero, delay);
		mainPanel.DOFade(1, delay);
		yield return tw.WaitForCompletion();
	}

	public IEnumerator ShowClearCanvas() {
		isPlaying = false;
		SoundManager.Play(MusicType.StageClear);
		gameEndCanvas.SetActive(true);
		Vector3 startPos = new Vector3(0, 19.2f, 0);
		mainPanel.transform.DOMove(startPos, 0);
		mainPanel.sprite = clearSprite;
		Tween tw = mainPanel.transform.DOMove(Vector3.zero, delay);
		mainPanel.DOFade(1, delay);
		yield return tw.WaitForCompletion();
		yield return StartCoroutine(ShowStars());
	}

	float jumpPower = 3;
	float duration = 1;

	public IEnumerator ShowStars() {
		ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
		int numberOfStars = scoreManager.numberOfStars;
		for (int i = 0; i < numberOfStars; i++) {
			GameObject star = starObjects[i];
			star.GetComponent<Image>().enabled = true;
			star.GetComponent<Image>().sprite = starSprite;
			star.GetComponent<Image>().color = Color.yellow;
			star.GetComponentInChildren<ParticleSystem>().Play();
			
			Vector3 originPos = star.transform.position;
			Tween tw = star.transform.DOJump(originPos, jumpPower, 1, duration);
			yield return tw.WaitForCompletion();
		}
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
		starObjects.ToList().ForEach(star => {
			star.GetComponentInChildren<ParticleSystem>().Stop();
			star.GetComponent<Image>().color = Color.gray;
			star.GetComponent<Image>().enabled = false;
		});
	}

	// Use this for initialization
	public IEnumerator StartByGSM () {
		yield return StartCoroutine(StartAnimation());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStateManager.gameState != GameState.End) return;

		if (gameEndCanvas.activeInHierarchy) {
			if (Input.anyKeyDown && gameStateManager.gameState == GameState.End)
				StartCoroutine(HideCanvas());
		}
	}

	IEnumerator HideCanvas () {
		starObjects.ToList().ForEach(star => {
			if (star.GetComponent<Image>().enabled) {
				star.GetComponent<Image>().DOColor(Color.black, delay);
			}
		});
		Tween tw = mainPanel.DOColor(Color.black, delay);
		yield return tw.WaitForCompletion();
		isPlaying = false;
		MissionData.gotSuperfood = false;
		MissionData.gotTimeItem = false;
		MissionData.gotTrayItem = false;
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("World");
	}
}
