using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour {

	public Image bar;
	public Image checkPoint1;
	public Image checkPoint2;
	public Image checkPoint3;

	public Sprite inactiveBunny;
	public Sprite activeBunny;

	float feverAmount = 0;
	int goalAmount = 0;

	int feverLevel = 0;

	void ActivePoint(Image point) {
		point.sprite = activeBunny;
		point.color = Color.white;
		feverLevel += 1;
	}

	void InactiveAllPoints() {
		checkPoint1.sprite = inactiveBunny;
		checkPoint1.color = Color.gray;
		checkPoint2.sprite = inactiveBunny;
		checkPoint2.color = Color.gray;
		checkPoint3.sprite = inactiveBunny;
		checkPoint3.color = Color.gray;

		feverLevel = 0;
	}

	public void AddFeverAmount(int amount) {
		goalAmount += amount;
	}

	// Use this for initialization
	void Start () {
		feverAmount = 0;
		feverLevel = 0;
		bar.fillAmount = 0;
		InactiveAllPoints();
	}

	float amount = 0.5f;

	// Update is called once per frame
	void Update () {
		if (feverAmount < goalAmount) {
			feverAmount += amount;
			bar.fillAmount = feverAmount / 100f;
		}

		if (feverAmount > 33 && feverLevel < 1) {
			ActivePoint(checkPoint1);
		}

		if (feverAmount > 66 && feverLevel < 2) {
			ActivePoint(checkPoint2);
		}

		if (feverAmount >= 100 && feverLevel < 3) {
			ActivePoint(checkPoint3);
		}

		if (Input.GetKeyDown(KeyCode.V)) {
			AddFeverAmount(15);
		}
	}
}
