using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class RabbitInfoPanel : MonoBehaviour {

public Rabbit rabbitInfo;
public Image idImage;
public RabbitInfoScrollView rabbitInfoScrollView;

	public void SetRabbitInfo(int index)
	{
		idImage.enabled = true;
		rabbitInfo = RabbitData.GetRabbitData(index);
		string spriteName = "id " + rabbitInfo.imageName;
		Sprite idSprite = Resources.Load<Sprite>("catalogue/id cards/" + spriteName);
		idImage.sprite = idSprite;
	}

	// Use this for initialization
	void Start () {
		rabbitInfoScrollView = FindObjectOfType<RabbitInfoScrollView>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
