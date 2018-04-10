using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class TestManager : MonoBehaviour {
	public bool randomizeCustomer;
	public Toggle randomToggle;
	public List<int> nextCustomers;
	public Image[] nextCustomerImages;
	public Toggle[] nextCustomerChoices;
	public Toggle[] nextOrderChoices;
	
	public void RandomizeCustomer()
	{
		randomizeCustomer = true;
	}

	public void AddChosenCustomer()
	{
		int nextCustomerIndex = 1;
		for(int i = 0; i < nextCustomerChoices.Length; i++)
		{
			if(nextCustomerChoices[i].isOn)
			{
				nextCustomerIndex = i + 1;
				break;
			}
			else
			nextCustomerIndex = Random.Range(1, 30);
		}

		nextCustomers.Add(nextCustomerIndex);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		randomizeCustomer = (randomToggle.isOn) ? true : false;
		int nextCustomerCount = nextCustomers.Count;
		for(int i = 0; i < nextCustomerImages.Length; i++){
			if(i < nextCustomerCount){
				Rabbit rabbitData = RabbitData.GetRabbitData(nextCustomers[i]); 
				string spriteName = rabbitData.gender.ToString() + "/" + rabbitData.imageName;
				Sprite customerSprite = Resources.Load<Sprite>("customers/" + spriteName);
				nextCustomerImages[i].sprite = customerSprite;
				nextCustomerImages[i].enabled = true;
			}
			else{
				nextCustomerImages[i].enabled = false;
			}
		}
	}
}
