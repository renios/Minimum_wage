using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour {

	public Image bar;
	public Image checkPoint1;
	public Image checkPoint2;
	public Image checkPoint3;

	public GameObject makeSuperfoodEffectPrefab;

	public Sprite inactiveBunny;
	public Sprite activeBunny;

	float feverAmount = 0;
	float goalAmount = 0;

	int feverLevel = 0;

	int waitingTime = 30;

	int maxAmount = 50;

	readonly int comboCoef = 5;
	readonly int customerCoef = 2;

	public TrayManager trayManager;

	void ActivePoint(Image point) {
		point.GetComponentInChildren<ParticleSystem>().Play();
		point.sprite = activeBunny;
		point.color = Color.white;
		feverLevel += 1;
	}

	void InactiveAllPoints() {
		checkPoint1.sprite = inactiveBunny;
		checkPoint1.color = Color.gray;
		checkPoint1.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint2.sprite = inactiveBunny;
		checkPoint2.color = Color.gray;
		checkPoint2.GetComponentInChildren<ParticleSystem>().Stop();
		checkPoint3.sprite = inactiveBunny;
		checkPoint3.color = Color.gray;
		checkPoint3.GetComponentInChildren<ParticleSystem>().Stop();

		feverLevel = 0;
	}

	public void AddFeverAmountByCustomer(Customer customer) {
		float amount = 0;
		amount = customer.remainWaitingTime / (float)waitingTime;
		amount *= customerCoef;
		AddFeverAmount(amount);
	}

	public void AddFeverAmountByCombo(int comboCount) {
		float amount = comboCount * comboCoef;
		AddFeverAmount(amount);
	}

	public void AddFeverAmount(float amount) {
		goalAmount += amount;
		Debug.Log("fever : " + feverAmount + " -> " + goalAmount);
	}

	// Use this for initialization
	void Start () {
		feverAmount = 0;
		feverLevel = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();

		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		if (missionDataDict.ContainsKey(MissionDataType.waitingTime)) {
			waitingTime = MissionData.GetMissionDataDict()[MissionDataType.waitingTime];
		}
	}

	public IEnumerator MakeSuperfoodByFever(Vector3 startPos) {
		GameObject newSuperfood;
		if(MissionData.gotSuperfood == true)
		{
			newSuperfood = trayManager.FindSuperfoodTarget();
			yield return StartCoroutine(MakeSuperfoodEffect(startPos, newSuperfood));
			yield return StartCoroutine(newSuperfood.GetComponent<FoodOnTray>().ChangeToSuperfood());
			// newSuperfood = trayManager.MakeSuperfood();
			MissionData.gotSuperfood = true;
		}
		else
		{
			MissionData.gotSuperfood = true;
			newSuperfood = trayManager.FindSuperfoodTarget();
			yield return StartCoroutine(MakeSuperfoodEffect(startPos, newSuperfood));
			yield return StartCoroutine(newSuperfood.GetComponent<FoodOnTray>().ChangeToSuperfood());
			// newSuperfood = trayManager.MakeSuperfood();
		}
	}

	IEnumerator MakeSuperfoodEffect(Vector3 startPos, GameObject target) {
		if (target != null) {
			Vector3 endPos = target.transform.position;
			GameObject makeSuperfoodEffect = Instantiate(makeSuperfoodEffectPrefab, startPos, Quaternion.identity);
			yield return StartCoroutine(makeSuperfoodEffect.GetComponent<MakeSuperfoodAnim>().StartAnim(startPos, endPos));
		}
	}

	float amount = 0.5f;

	public IEnumerator CheckFeverPoint() {
		List<IEnumerator> coroutines = new List<IEnumerator>();

		if (feverAmount > maxAmount/3f && feverLevel < 1) {
			ActivePoint(checkPoint1);
			coroutines.Add(MakeSuperfoodByFever(checkPoint1.transform.position));
		}

		if (feverAmount > (maxAmount*2)/3f && feverLevel < 2) {
			ActivePoint(checkPoint2);
			coroutines.Add(MakeSuperfoodByFever(checkPoint2.transform.position));
		}

		if (feverAmount >= maxAmount && feverLevel < 3) {
			ActivePoint(checkPoint3);
			coroutines.Add(MakeSuperfoodByFever(checkPoint3.transform.position));
		}

		if (coroutines.Count == 1) {
			yield return StartCoroutine(coroutines[0]);
		}
		else if (coroutines.Count > 1) {
			for (int i = 0; i < coroutines.Count - 1; i++) {
				StartCoroutine(coroutines[i]);
			}
			yield return StartCoroutine(coroutines[coroutines.Count - 1]);
		}

		if (feverLevel >= 3) {
			Reset();
		}
	}

	// Update is called once per frame
	void Update () {
		if (feverAmount < goalAmount) {
			feverAmount += amount;
			bar.fillAmount = feverAmount / maxAmount;
		}

		if (Input.GetKeyDown(KeyCode.V)) {
			AddFeverAmount(15);
		}
	}

	void Reset () {
		goalAmount = 0;
		feverAmount = 0;
		feverLevel = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();
	}
}
