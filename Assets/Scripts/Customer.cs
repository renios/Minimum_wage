using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Enums;

public class Customer : MonoBehaviour {

	public int indexInArray;

	// newRabbit.index = 1;
	// newRabbit.releaseStageIndex = 1;
	// newRabbit.gender = Gender.Male;
	// newRabbit.imageName = "yoonsung";
	// newRabbit.waitingTime = 40;
	// newRabbit.reduceHeartsByFail = 1;
	// newRabbit.variableOfOrderFood = new List<int> {3, 4};
	// 토끼 기본 스탯
	public Rabbit rabbitData;

	public float waitingTime;
	public float remainWaitingTime;
	public float toleranceRate;
	public Image timerImage;
	public Image customerImage;
	public GameObject[] orderToBeDestroyed;   // 매칭 애니메이션 때 손님 이미지와 주문판 이미지들을 분리하기 위해 미리 주문판 이미지들을 받아놓음
	public Vector3 customerImageOriginPos;
	public bool isServeCompleted;            // 서빙 완료돼서 나갈때 true
	public bool isServed = false;            // 동시체크를 위한 변수
	public Gender gender;				// 효과음 성별 구분용
	public int rabbitIndex;					// 이미지 중복 체크용
	public float furyRate;
	public float maxFuryRate;
	public float furyCount = 0;

	public List<FoodInOrder> orderedFoods = new List<FoodInOrder>();

	public bool startedFury = false;
	bool initialized = false;

	CustomerManager customerManager;
	GameManager gameManager;
	GameStateManager gameStateManager;

	void InitializeTimer() {
		waitingTime = rabbitData.waitingTime;
		remainWaitingTime = waitingTime;
		timerImage.color = new Color(131f / 255f, 193f / 255f, 193f / 255f, 1f);
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void UpdateTimer() {
		if(!isServeCompleted && (gameStateManager.gameState == GameState.Idle || gameStateManager.gameState == GameState.Picked))
		{
			remainWaitingTime -= Time.deltaTime;
			timerImage.fillAmount = remainWaitingTime / waitingTime;
		}
	}

	void MakeOrder() {
		orderedFoods = GetComponentsInChildren<FoodInOrder>().ToList();
		orderedFoods.ForEach(food => food.Initialize());
	}
	public void SetOrder(List<FoodType> foodList){
		//Debug.Log("Customer.SetOrder : "+Time.time);
		orderedFoods = GetComponentsInChildren<FoodInOrder>().ToList();
		for (int i = 0; i < orderedFoods.Count; i++){
			int randomIndex = Random.Range(0,foodList.Count);
			orderedFoods[i].Initialize((int)foodList[randomIndex]);
			foodList.RemoveAt(randomIndex);
		}
	}

	void SetImage() {
		string spriteName = rabbitData.gender.ToString() + "/" + rabbitData.imageName;
		Sprite customerSprite = Resources.Load<Sprite>("customers/" + spriteName);
		customerImage.sprite = customerSprite;
	}

	// Use this for initialization
	public void Initialize (int indexInArray, Rabbit rabbitData) {
		this.indexInArray = indexInArray;
		this.rabbitData = rabbitData;

		SetImage();
		InitializeTimer();
		MakeOrder();

		initialized = true;
	}

	// Use this for initialization
	void Start () {
		isServeCompleted = false;
		gameManager = FindObjectOfType<GameManager>();
		gameStateManager = FindObjectOfType<GameStateManager>();
		customerManager = FindObjectOfType<CustomerManager>();
		customerImageOriginPos = customerImage.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.isPlaying) return;
		if (!initialized) return;
		if (isServeCompleted) return;
	
		UpdateTimer();

		if (remainWaitingTime <= waitingTime / toleranceRate && 
		startedFury == false)
		{
			timerImage.color = new Color(255f / 255f, 131f / 255f, 131f / 255f, 1f);
			customerImageOriginPos = customerImage.transform.localPosition;
			startedFury = true;
			furyRate = 0.1f;
		}

		if (startedFury == true)
		{
			furyCount++;
			if(furyCount % 2 == 1)
			{
				furyRate = Mathf.Lerp(furyRate, maxFuryRate, 0.001f);
				customerImage.transform.localPosition = customerImageOriginPos + new Vector3(Random.Range(-1f, 1f) * furyRate, 0, 0);
			}
		}

		if (remainWaitingTime <= 0) {
			SoundManager.PlayCustomerReaction(rabbitData.gender, false);
			startedFury = false;
			furyCount = 0;
			customerManager.RemoveCustomerByTimeout(indexInArray);
		}
	}
}
