using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PreTutorialManager : MonoBehaviour {

	public GameObject bgPanel;
	public Image managerImage;
	public GameObject balloonObject;
	public Button button;
	
	public void GoToTutorial() {
		SceneManager.LoadScene("Tutorial_new");
	}

	// Use this for initialization
	IEnumerator Start () {
		Tween tw = bgPanel.GetComponent<Image>().DOFade(0.6f, 1);
		yield return tw.WaitForCompletion();
		tw = managerImage.transform.DOLocalMove(new Vector3(-250, -600, 0), 1);
		yield return tw.WaitForCompletion();
		balloonObject.SetActive(true);
		
		yield return new WaitForSeconds(0.5f);

		button.interactable = true;
	}
}
