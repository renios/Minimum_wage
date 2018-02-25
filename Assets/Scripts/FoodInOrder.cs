using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class FoodInOrder : MonoBehaviour {

	public FoodType foodType;
    // 매칭에 대응되는 트레이 음식을 찾았는지 여부. 매칭 애니메이션 때 같은 타입이 한 주문에 여럿 있을 때 따로 처리하기 위해 도입.
    public bool foundCorrespondent;  

	public Sprite EnumToSprite (FoodType foodTypeEnum) {
		string pathString;
		if (MissionData.stageIndex < 11) {
			pathString = "Foods/World1/";
		}
		else if (MissionData.stageIndex < 21) {
			pathString = "Foods/World2/";
		}
		else {
			pathString = "Foods/World1/";
		}
		switch (foodTypeEnum)
		{
			case FoodType.A:
				return Resources.Load(pathString + "food01", typeof(Sprite)) as Sprite;
			case FoodType.B:
				return Resources.Load(pathString + "food02", typeof(Sprite)) as Sprite;
			case FoodType.C:
				return Resources.Load(pathString + "food03", typeof(Sprite)) as Sprite;
			case FoodType.D:
				return Resources.Load(pathString + "food04", typeof(Sprite)) as Sprite;
			case FoodType.E:
				return Resources.Load(pathString + "food05", typeof(Sprite)) as Sprite;
			case FoodType.F:
				return Resources.Load(pathString + "food06", typeof(Sprite)) as Sprite;
			default:
				return Resources.Load(pathString + "food01", typeof(Sprite)) as Sprite;
		}
	}

	public void Initialize () {
		// 랜덤 음식으로 변환
		int foodTypeIndex = Random.Range(0, MissionData.foodTypeCount);
		foodType = (FoodType)foodTypeIndex;

		// 임시 음식 이미지로 변환
		GetComponent<SpriteRenderer>().sprite = EnumToSprite(foodType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}