using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonInTitle : MonoBehaviour {

	public void GoToWorldScene() {
        SceneManager.LoadScene("World");
	}
}
