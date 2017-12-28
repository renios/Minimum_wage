using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class FoodOnTray : MonoBehaviour {

	public FoodType foodType;

	TrayManager trayManager;

	// 음식과 색을 매치시키는 임시 코드
	public Color EnumToColor (FoodType foodTypeEnum) {
		switch (foodTypeEnum)
		{
			case FoodType.A:
				return Color.black;
			case FoodType.B:
				return Color.blue;
			case FoodType.C:
				return Color.cyan;
			case FoodType.D:
				return Color.gray;
			case FoodType.E:
				return Color.magenta;
			case FoodType.F:
				return Color.yellow;
			default:
				return Color.white;
		}
	}

	// Use this for initialization
	void Start () {
		trayManager = FindObjectOfType<TrayManager>();

		// 랜덤 음식으로 변환
		int foodTypeIndex = Random.Range(0, 6);
		foodType = (FoodType)foodTypeIndex;
		
		// 음식에 따른 색으로 변환(임시)
		GetComponent<SpriteRenderer>().color = EnumToColor(foodType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}