using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using DG.Tweening;

public class FoodOnTray : MonoBehaviour {

	public FoodType foodType;
	public Vector2 foodCoord;
    public bool isEnlarging;
    public bool isFoodMoving;
	public bool isServed = false;
    public Vector3 correspondentPos;
    public FoodInOrder correspondent;

	public bool isSuperfood = false;
	public GameObject superfoodEffect;

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

	public void Initialize() {
		// 랜덤 음식으로 변환
		int foodTypeIndex = Random.Range(0, MissionData.foodTypeCount);
		foodType = (FoodType)foodTypeIndex;
		
		// 임시 음식 이미지로 변환
		GetComponent<SpriteRenderer>().sprite = EnumToSprite(foodType);
	}

	public void Initialize(FoodType foodType) {
		// 지정된 음식으로 변환
		this.foodType = foodType;
		
		// 해당되는 음식 이미지로 변환
		GetComponent<SpriteRenderer>().sprite = EnumToSprite(foodType);
	}

	public IEnumerator ChangeToSuperfood() {
		isSuperfood = true;

        string superfoodPath = "Foods/World" + ( ((MissionData.stageIndex - 1) / 10) + 1 ).ToString("N0") + "/food09";

        Sprite superfoodSprite = Resources.Load(superfoodPath, typeof(Sprite)) as Sprite;
        GetComponent<SpriteRenderer>().sprite = superfoodSprite;

		superfoodEffect.SetActive(true);

        float delay = 0.2f;
		float originScale = transform.localScale.x;
		Tween tw = transform.DOScale(originScale*1.5f, delay);
		yield return tw.WaitForCompletion();
		transform.DOScale(originScale, delay);

		yield return StartCoroutine(FindObjectOfType<TrayManager>().TryMatch());
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}