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
    public bool isServeCompleted; // 서빙 완료돼서 나갈때 true
	public bool isServed = false; // 동시체크를 위한 변수
    Vector3 backupBunnyPosition;
    float furyRate;
    public float maxFuryRate;

    public List<FoodInOrder> orderedFoods = new List<FoodInOrder>();

    bool startedFury = false;
	bool initialized = false;

	CustomerManager customerManager;

	void InitializeTimer(float inputTime) {
		waitingTime = inputTime;
		remainWaitingTime = waitingTime;
        timerImage.color = new Color(131f / 255f, 193f / 255f, 193f / 255f, 1f);
		timerImage.fillAmount = remainWaitingTime / waitingTime;
	}

	void UpdateTimer() {
        if(isServeCompleted==false)
        {
            remainWaitingTime -= Time.deltaTime;
            timerImage.fillAmount = remainWaitingTime / waitingTime;
        }
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
        isServeCompleted = false;
        customerManager = FindObjectOfType<CustomerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialized) return;
	
		UpdateTimer();

        if(remainWaitingTime<=waitingTime/toleranceRate&&startedFury==false&&isServeCompleted==false)
        {
            timerImage.color = new Color(255f / 255f, 131f / 255f, 131f / 255f, 1f);
            backupBunnyPosition = gameObject.transform.position;
            startedFury = true;
            furyRate = 0.1f;
        }

        if(startedFury==true)
        {
            furyRate = Mathf.Lerp(furyRate, maxFuryRate, 0.001f);
            transform.position = new Vector3(backupBunnyPosition.x + Random.Range(-1f, 1f) * furyRate, backupBunnyPosition.y, backupBunnyPosition.z);
        }

		if (remainWaitingTime <= 0) {
            startedFury = false;
            customerManager.RemoveCustomerByTimeout(indexInArray);
		}
	}
}
