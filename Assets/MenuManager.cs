using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public GameObject panel;
	public Image panelBg;
	public Image fadeoutPanel;

	float delay = 0.5f;
	Vector3 originPos;

	public void ActiveMenuPanel() {
		StartCoroutine(ActiveMenuPanelCoroutine());
	}

	IEnumerator ActiveMenuPanelCoroutine() {
		Time.timeScale = 0;
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
		Time.timeScale = 1;
	}

	public void GoToWorld() {
		StartCoroutine(GoToWorldCoroutine());
	}

	IEnumerator GoToWorldCoroutine() {
		Tween tw = fadeoutPanel.DOFade(1, delay).SetUpdate(UpdateType.Normal, true);
		MissionData.gotSuperfood = false;
		MissionData.gotTimeItem = false;
		MissionData.gotTrayItem = false;
		yield return tw.WaitForCompletion();
		tw = fadeoutPanel.DOFade(1, 1).SetUpdate(UpdateType.Normal, true);
		yield return tw.WaitForCompletion();
		Time.timeScale = 1;
		SceneManager.LoadScene("World");
	}

	// Use this for initialization
	void Start () {
		originPos = panel.GetComponent<RectTransform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
