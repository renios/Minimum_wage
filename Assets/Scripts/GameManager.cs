using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject gameoverCanvas;
	public Text textInCanvas;

	bool isEnd;

	public void ShowGameoverCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Game Over" + '\n' + '\n' + "Touch the Screen";
		Time.timeScale = 0;
	}

	public void ShowClearCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Mission Clear" + '\n' + '\n' + "Touch the Screen";
		Time.timeScale = 0;
	}

	void ShowMissionStartCanvas() {
		Time.timeScale = 0;
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Mission Start!" + '\n' + '\n' + "Touch the Screen";
	}

	// Use this for initialization
	void Start () {
		isEnd = false;
		Time.timeScale = 1;
		ShowMissionStartCanvas();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameoverCanvas.activeInHierarchy && Input.anyKeyDown) {
			if (isEnd)
				SceneManager.LoadScene("World");
			else {
				gameoverCanvas.SetActive(false);
				Time.timeScale = 1;
				isEnd = true;
			}
		}
	}
}
