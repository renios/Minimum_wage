using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {

	public Text coinText;

	public int coin;

	public void AddCoin(int amount) {
		coin += amount;
		coinText.text = coin.ToString();
	}

	// Use this for initialization
	void Start () {
		coin = 0;
		coinText.text = coin.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
