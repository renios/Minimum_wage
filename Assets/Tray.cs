using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tray : MonoBehaviour {

	public List<FoodOnTray> foods = new List<FoodOnTray>();

	public void UpdateFoodsOnTray() {
		foods = GetComponentsInChildren<FoodOnTray>().ToList();
	}

	public void Refresh() {
		foods.ForEach(food => food.Initialize());
	}

	// Use this for initialization
	void Start () {
		foods = GetComponentsInChildren<FoodOnTray>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
