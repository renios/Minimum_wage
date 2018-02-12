using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

	public int indexInArray;

	public float waitingTime;
	public float remainWaitingTime;
    public float toleranceRate;
	public Image timerImage;
	public Image customerImage;

	public List<FoodInOrder> orderedFoods = new List<FoodInOrder>();

	bool initialized = false;

	CustomerManager customerManager;

	void InitializeTimer(float inputTime) {
		waitingTime = inputTime;
		remainWaitingTime = waitingTime;
        timerImage.color = new Color(131f / 255f, 193f / 255f, 193f / 255f, 1f);
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void UpdateTimer() {
		remainWaitingTime -= Time.deltaTime;
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void MakeOrder() {
		orderedFoods = GetComponentsInChildren<FoodInOrder>().ToList();
	}

	void SetRandomImage() {
		Object[] spriteObjects = Resources.LoadAll("Bunnies", typeof(Sprite));
		int pickedIndex = Random.Range(0, spriteObjects.Length);
		Sprite pickedSprite = spriteObjects[pickedIndex] as Sprite;
		customerImage.sprite = pickedSprite;
	}

	// Use this for initialization
	public void Initialize (int indexInArray, float inputTime) {
		this.indexInArray = indexInArray;

		SetRandomImage();

		MakeOrder();
		InitializeTimer(inputTime);

		initialized = true;
	}

	// Use this for initialization
	void Start () {
		customerManager = FindObjectOfType<CustomerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialized) return;
	
		UpdateTimer();

        if(remainWaitingTime<=waitingTime/toleranceRate)
        {
            timerImage.color = new Color(255f / 255f, 131f / 255f, 131f / 255f, 1f);
        }

		if (remainWaitingTime <= 0) {
			customerManager.RemoveCustomerByTimeout(indexInArray);
		}
	}
}
