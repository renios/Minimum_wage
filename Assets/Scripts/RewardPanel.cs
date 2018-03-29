using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour {

    bool isChoosing;
    [SerializeField]
    Sprite[] itemSprites;
    [SerializeField]
    Button randomItem;
    [SerializeField]
    GameObject checkButton;

    private void Start()
    {
        checkButton.SetActive(false);
        isChoosing = true;
        StartCoroutine(ChoosingItem());
    }

    IEnumerator ChoosingItem()
    {
        int spriteIndex = 0;
        while (isChoosing)
        {
            spriteIndex = Random.Range(0, itemSprites.Length);
            randomItem.GetComponent<Image>().sprite = itemSprites[spriteIndex];
            yield return null;
        }

        string itemString = "";

        switch (spriteIndex)
        {
            // 0은 타이머 리셋, 1은 만능음식, 2는 판갈기
            case 0:
                {
                    PlayerPrefs.SetInt("TimerReset", PlayerPrefs.GetInt("TimerReset", 0) + 1);
                    itemString = "타이머";
                }
                break;
            case 1:
                {
                    PlayerPrefs.SetInt("Superfood", PlayerPrefs.GetInt("Superfood", 0) + 1);
                    itemString = "만능음식";
                }
                break;
            case 2:
                {
                    PlayerPrefs.SetInt("TrayReset", PlayerPrefs.GetInt("TrayReset", 0) + 1);
                    itemString = "트레이";
                }
                break;
        }

        checkButton.SetActive(true);
        checkButton.GetComponentInChildren<Text>().text = itemString + " 아이템을 얻었어요!";
    }

    public void SelectItem()
    {
        isChoosing = false;
    }
}
