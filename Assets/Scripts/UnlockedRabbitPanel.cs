using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Enums;
using System.Linq;

public class UnlockedRabbitPanel : MonoBehaviour {

	public List<Image> unlockedRabbitImages; 
	public Image unlockPanelbg;
	public GameObject unlockPanel;
	public Text unlockText;

	public void ShowUnlockPanel(List<Rabbit> newUnlockedRabbits) {
		unlockPanelbg.raycastTarget = true;
		unlockedRabbitImages.ForEach(image => image.enabled = true);

		for (int index = 0; index < newUnlockedRabbits.Count; index++) {
			Rabbit newUnlockedRabbit = newUnlockedRabbits[index];
			string spriteName = newUnlockedRabbit.gender.ToString() + "/" + newUnlockedRabbit.imageName;
			Sprite rabbitSprite = Resources.Load<Sprite>("customers/" + spriteName);
			unlockedRabbitImages[index].sprite = rabbitSprite;
		}
		for (int index = newUnlockedRabbits.Count; index < 4; index++) {
			unlockedRabbitImages[index].enabled = false;
		}

		if (newUnlockedRabbits.Any(rabbit => rabbit.index >= 25)) {
			unlockText.text = "이제 이 손님이 가게를 방문합니다" + '\n' +
							  "<color=red>주의! 이 손님은 VIP입니다." + '\n' +
							  "VIP 서빙은 절대 실패하면 안 됩니다.</color>";
		}
		else {
			unlockText.text = "이제 이 손님이 가게를 방문합니다";
		}

		Vector3 endPos = new Vector3(Screen.width/2, Screen.height/2, 0);
		float delay = 0.5f;
		unlockPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		unlockPanelbg.DOFade(0.4f, delay);
	}

	public void HideUnlockPanel() {
		Vector3 endPos = new Vector3(Screen.width/2, -Screen.height/2, 0);
		float delay = 0.5f;
		unlockPanel.GetComponent<RectTransform>().DOMove(endPos, delay);
		unlockPanelbg.DOFade(0, delay);
		unlockPanelbg.raycastTarget = false;
	}

	// Use this for initialization
	void Start () {
		int progress = PlayerPrefs.GetInt("Progress", 1);
		int unlockProgress = PlayerPrefs.GetInt("UnlockProgress", 1);

		if (progress != unlockProgress) {
			List<Rabbit> newUnlockedRabbits = new List<Rabbit>();
			for (int index = 1; index <= RabbitData.numberOfRabbitData; index++) {
				Rabbit newRabbitData = RabbitData.GetRabbitData(index);
				if (newRabbitData.releaseStageIndex == unlockProgress + 1)
					newUnlockedRabbits.Add(newRabbitData);
			}

			if (newUnlockedRabbits.Count > 0) {
				ShowUnlockPanel(newUnlockedRabbits);			
			}

			int newUnlockProgress = unlockProgress + 1;
			PlayerPrefs.SetInt("UnlockProgress", newUnlockProgress);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
