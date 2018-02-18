using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {

	public Text coinText;

	public int coin;

    public int defaultFontSize;
    public int maxFontSize;
    public float animRate;

    bool isEmphasizing;

	public void AddCoin(int amount) {
		coin += amount;
		coinText.text = coin.ToString();
        if (isEmphasizing)
            ResetText();
        isEmphasizing = true;
	}

	// Use this for initialization
	void Start () {
		coin = 0;
		coinText.text = coin.ToString();
        isEmphasizing = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isEmphasizing && coinText.fontSize < maxFontSize)
        {
            coinText.fontSize = (int)Mathf.Lerp(coinText.fontSize, defaultFontSize * 2, animRate);
            coinText.color = new Color(coinText.color.r, coinText.color.g, coinText.color.b, coinText.color.a * (1 - animRate));
        }
        else
            ResetText();
    }

    void ResetText()
    {
        coinText.fontSize = defaultFontSize;
        coinText.color = new Color(coinText.color.r, coinText.color.g, coinText.color.b, 1);
        isEmphasizing = false;
    }
}
