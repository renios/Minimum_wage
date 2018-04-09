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

    // 아이템 보상 주어야 하는지 체크(MissionManager에서 true로 바꿈)
    public bool needsReward = false;
    public GameObject rewardCanvas;

	float delay = 0.5f;

	GameStateManager gameStateManager;

	public IEnumerator ShowGameoverCanvas() {
		// 가지고 들어온 아이템을 사용하지 않았다면 소지 아이템으로 다시 돌려 준다
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
		starObjects.ToList().ForEach(star => {
			star.GetComponent<Image>().enabled = true;
			star.GetComponent<Image>().DOFade(0, 0);
		});
		Vector3 startPos = new Vector3(0, 19.2f, 0);
		mainPanel.transform.DOMove(startPos, 0);
		mainPanel.sprite = clearSprite;
		Tween tw = mainPanel.transform.DOMove(Vector3.zero, delay);
		mainPanel.DOFade(1, delay);
		starObjects.ToList().ForEach(star => star.GetComponent<Image>().DOFade(1, delay));
		yield return tw.WaitForCompletion();
		yield return StartCoroutine(ShowStars());
	}

	void UpdateStarsOfStage(int numberOfStars) {
		int starsOfStage = PlayerPrefs.GetInt("StarsOfStage" + MissionData.stageIndex, 0);
		if (numberOfStars > starsOfStage) {
			PlayerPrefs.SetInt("StarsOfStage" + MissionData.stageIndex, numberOfStars);
			Debug.Log("Update starsOfStage" + MissionData.stageIndex + " : " + starsOfStage + " -> " + numberOfStars);
		}
	}

	float jumpPower = 3;
	float duration = 1;

	public IEnumerator ShowStars() {
		ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
		int numberOfStars = scoreManager.numberOfStars;
		UpdateStarsOfStage(numberOfStars);
		for (int i = 0; i < numberOfStars; i++) {
			GameObject star = starObjects[i];
			star.GetComponent<Image>().enabled = true;
			star.GetComponent<Image>().sprite = starSprite;
			star.GetComponent<Image>().color = Color.white;
			star.GetComponentInChildren<ParticleSystem>().Play();
			
			Vector3 originPos = star.transform.position;
			star.transform.DOJump(originPos, jumpPower, 1, duration);
			yield return new WaitForSeconds(duration*0.5f);
		}
		yield return new WaitForSeconds(duration*0.5f);
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
			star.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
			star.GetComponent<Image>().enabled = false;
		});
		rewardCanvas.SetActive(false);
	}

	// Use this for initialization
	public IEnumerator StartByGSM () {
		RabbitInformation.ResetRabbitIndex();
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
		// starObjects.ToList().ForEach(star => {
		// 	if (star.GetComponent<Image>().enabled) {
		// 		star.GetComponent<Image>().DOColor(Color.black, delay);
		// 		star.GetComponentInChildren<ParticleSystem>().Stop();
		// 	}
		// });
		MissionData.gotSuperfood = false;
		MissionData.gotTimeItem = false;
		MissionData.gotTrayItem = false;
		isPlaying = false;
		yield return new WaitForSeconds(1f);
		if (needsReward)
		{
			// 아이템 주는 패널 보여주기
			rewardCanvas.SetActive(true);
		}
		else
			SceneManager.LoadScene("World");
		// Tween tw = mainPanel.DOColor(Color.black, delay);
		// yield return tw.WaitForCompletion();
	}
}
