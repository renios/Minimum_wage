using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject gameoverCanvas;
	public Text textInCanvas;

	public void ShowGameoverCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Game Over" + '\n' + '\n' + "Touch to Restart";
		Time.timeScale = 0;
	}

	public void ShowClearCanvas() {
		gameoverCanvas.SetActive(true);
		textInCanvas.text = "Mission Clear" + '\n' + '\n' + "Touch to Restart";
		Time.timeScale = 0;
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		gameoverCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameoverCanvas.activeInHierarchy && Input.anyKeyDown) {
			SceneManager.LoadScene("World");
		}
	}
}
