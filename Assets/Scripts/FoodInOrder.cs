﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class FoodInOrder : MonoBehaviour {

	public FoodType foodType;

	public Sprite EnumToSprite (FoodType foodTypeEnum) {
		switch (foodTypeEnum)
		{
			case FoodType.A:
				return Resources.Load("Foods/food01", typeof(Sprite)) as Sprite;
			case FoodType.B:
				return Resources.Load("Foods/food02", typeof(Sprite)) as Sprite;
			case FoodType.C:
				return Resources.Load("Foods/food03", typeof(Sprite)) as Sprite;
			case FoodType.D:
				return Resources.Load("Foods/food04", typeof(Sprite)) as Sprite;
			case FoodType.E:
				return Resources.Load("Foods/food05", typeof(Sprite)) as Sprite;
			case FoodType.F:
				return Resources.Load("Foods/food06", typeof(Sprite)) as Sprite;
			default:
				return Resources.Load("Foods/food01", typeof(Sprite)) as Sprite;
		}
	}

	public void Initialize () {
		// 랜덤 음식으로 변환
		int foodTypeIndex = Random.Range(0, 6);
		foodType = (FoodType)foodTypeIndex;

		// 임시 음식 이미지로 변환
		GetComponent<SpriteRenderer>().sprite = EnumToSprite(foodType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}