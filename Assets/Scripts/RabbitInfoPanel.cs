using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class RabbitInfoPanel : MonoBehaviour {

public Rabbit rabbitInfo;
public Image rabbitImage;
public Text rabbitNameText;
public Text rabbitIndexText;

	public void SetRabbitInfo(int index)
	{
		if(index < 1 || index > 29)
		{
			rabbitImage.enabled = false;
			rabbitNameText.enabled = false;
			rabbitIndexText.enabled = false;
		}
		else
		{
		rabbitInfo = RabbitData.GetRabbitData(index);
		string spriteName = rabbitInfo.gender.ToString() + "/" + rabbitInfo.imageName;
		Sprite rabbitSprite = Resources.Load<Sprite>("customers/" + spriteName);
		rabbitImage.sprite = rabbitSprite;
		rabbitNameText.text = rabbitInfo.imageName;
		rabbitIndexText.text = rabbitInfo.index.ToString("N0");
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
