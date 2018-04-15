using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class RabbitTableScrollView : MonoBehaviour {
	public RectTransform scrollContent;
	public RabbitInfoScrollView rabbitInfoScrollView;

	public void Initialize()
	{
		int unlockProgress = PlayerPrefs.GetInt("UnlockProgress", 1);
		scrollContent.sizeDelta = new Vector2(2160*unlockProgress, 0);
		scrollContent.anchoredPosition = new Vector2();
	}

	// Use this for initialization
	void Start () {
		// rabbitInfoScrollView = FindObjectOfType<RabbitInfoScrollView>();
		scrollContent = GetComponent<ScrollRect>().content;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
