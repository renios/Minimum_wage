using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class FoodOnTray : MonoBehaviour {

	public FoodType foodType;

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

	public void Initialize() {
		// 랜덤 음식으로 변환
		int foodTypeIndex = Random.Range(0, 6);
		foodType = (FoodType)foodTypeIndex;
		
		// 음식에 따른 색으로 변환(임시)
		GetComponent<SpriteRenderer>().color = EnumToColor(foodType);
	}

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}