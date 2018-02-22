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

	public bool isPlaying = false;

	bool isEnd;

	float delay = 0.5f;

	public IEnumerator ShowGameoverCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Game Over" + '\n' + '\n' + "Touch the Screen";
		bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return new WaitForSeconds(delay);
		isPlaying = false;
	}

	public IEnumerator ShowClearCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Mission Clear" + '\n' + '\n' + "Touch the Screen";
		bgPanel.DOFade(0.4f, delay);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(1, delay);
		yield return new WaitForSeconds(delay);
		isPlaying = false;
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
        startCanvas.SetActive(true);
        string[] countdownTexts = { "3", "2", "1", "Start!" };
        Text startText = startCanvas.GetComponentInChildren<Text>();
        startText.text = "";
        int startFontSize = 60;
        bool isShrinking = false;

        for(int i = 0; i < 4; i++)
        {
            isShrinking = true;
            startText.text = countdownTexts[i];
            startText.fontSize = startFontSize;
            while (isShrinking)
            {
                startText.fontSize--;
                yield return new WaitForSeconds(0.015f);
                if (startText.fontSize < 40) isShrinking = false;
            }
            startText.text = "";
        }

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
		SoundManager.Play(MusicType.Main);
        //StartCoroutine(ShowMissionStartCanvas());
        StartCoroutine(StartAnimation());
    }
	
	// Update is called once per frame
	void Update () {
		if (gameoverCanvas.activeInHierarchy && !isPlaying && Input.anyKeyDown) {
			if (isEnd)
				SceneManager.LoadScene("World");
			else {
				if (isPlayingAnim) return;
				else
					StartCoroutine(HideCanvas());
			}
		}
	}

	bool isPlayingAnim = false;

	IEnumerator HideCanvas () {
		isPlayingAnim = true;
		bgPanel.DOFade(0, delay);
		Vector3 endPos = new Vector3(Screen.width*1.5f, Screen.height/2, 0);
		mainPanel.transform.DOMove(endPos, delay);
		mainPanel.DOFade(0, delay);
		yield return new WaitForSeconds(delay);
		gameoverCanvas.SetActive(false);
		isEnd = true;
		isPlaying = true;
		isPlayingAnim = false;
	}
}
