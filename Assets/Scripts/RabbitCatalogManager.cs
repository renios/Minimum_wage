using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RabbitCatalogManager : MonoBehaviour {

	public GameObject catalogPanel;

	public void ShowCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
	}

	public void HideCatalog()
	{
		SoundManager.Play(SoundType.Button);
		Vector3 endPos = new Vector3(Screen.width*3/2, Screen.height/2, 0);
		float delay = 0.5f;
		catalogPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
