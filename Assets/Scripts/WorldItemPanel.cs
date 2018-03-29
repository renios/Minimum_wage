using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldItemPanel : MonoBehaviour {
    [SerializeField]
    Text[] ItemAmountTexts;
    int maxItemAmount = 5;

    void SetAmountText()
    {
        for (int i = 0; i < ItemAmountTexts.Length; i++)
        {
            int itemAmount = 0;

            switch (i)
            {
                case 0:
                    {
                        itemAmount = PlayerPrefs.GetInt("TimerReset", 0);
                    }
                    break;
                case 1:
                    {
                        itemAmount = PlayerPrefs.GetInt("Superfood", 0);
                    }
                    break;
                case 2:
                    {
                        itemAmount = PlayerPrefs.GetInt("TrayReset", 0);
                    }
                    break;
            }

            if (itemAmount == 0)
            {
                ItemAmountTexts[i].text = "-";
            }
            else if (itemAmount == maxItemAmount)
            {
                ItemAmountTexts[i].text = "MAX";
            }
            else
            {
                ItemAmountTexts[i].text = itemAmount.ToString("N0");
            }
        }
    }

    // Use this for initialization
    void Start () {
        // SetAmountText();
    }

    private void Update()
    {
        SetAmountText();
    }
}
