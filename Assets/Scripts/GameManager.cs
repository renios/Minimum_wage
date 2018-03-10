using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public GameObject gameoverCanvas;
    public GameObject startCanvas;
	public Image bgPanel;
	public Image mainPanel;
	public Text textInCanvas;
    public Sprite gameoverSprite;
    public Sprite clearSprite;

	public bool isPlaying = false;

	bool isEnd;

	float delay = 0.5f;

	public IEnumerator ShowGameoverCanvas() {
        isPlaying = false;
        isPlayingAnim = true;
        SoundManager.Play(MusicType.StageOver);
		gameoverCanvas.SetActive(true);
        mainPanel.sprite = gameoverSprite;
        //textInCanvas.text = "Game Over" + '\n' + '\n' + "Touch the Screen";
        bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return new WaitForSeconds(delay);
        isPlayingAnim = false;
    }

	public IEnumerator ShowClearCanvas() {
        isPlaying = false;
        isPlayingAnim = true;
        SoundManager.Play(MusicType.StageClear);
		gameoverCanvas.SetActive(true);
        mainPanel.sprite = clearSprite;
        //textInCanvas.text = "Mission Clear" + '\n' + '\n' + "Touch the Screen";
        bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return new WaitForSeconds(delay);
		isPlayingAnim = false;
    }

	IEnumerator ShowMissionStartCanvas() {
		gameoverCanvas.SetActive(true);
        textInCanvas.text = "Mission Start!" + '\n' + '\n' + "Touch the Screen";
		bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return new WaitForSeconds(delay);
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

	// Use this for initialization
	void Start () {
		isEnd = false;
        //StartCoroutine(ShowMissionStartCanvas());
        StartCoroutine(StartAnimation());
    }
	
	// Update is called once per frame
	void Update () {
		if (gameoverCanvas.activeInHierarchy && !isPlaying) {
            if (Input.anyKeyDown && !isPlayingAnim)
                StartCoroutine(HideCanvas());
		}
	}

	public bool isPlayingAnim = false;

	IEnumerator HideCanvas () {
		isPlayingAnim = true;
		bgPanel.DOFade(0, delay);
		Vector3 endPos = new Vector3(Screen.width*1.5f, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(0, delay);
		yield return new WaitForSeconds(delay);
		gameoverCanvas.SetActive(false);
        isPlaying = false;
		isPlayingAnim = false;
        MissionData.gotSuperfood = false;
        MissionData.gotTimeItem = false;
        MissionData.gotTrayItem = false;
        SceneManager.LoadScene("World");
    }
}
