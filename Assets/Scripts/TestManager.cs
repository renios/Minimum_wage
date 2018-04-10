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
	public int[] nextOrderCount;
	public Text[] nextOrderCountText;
	int totalChosenOrder;
	public List<List<FoodType>> nextOrders;
	public Image[] nextTrayImages;
	public List<FoodType> nextTrayFood;
	
	public void RandomizeCustomer()
	{
		randomizeCustomer = true;
	}

#region ButtonRegion
	public void ChooseFood01()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[0]++;
			totalChosenOrder++;
		}
	}
	public void ChooseFood02()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[1]++;
			totalChosenOrder++;
		}
	}
		public void ChooseFood03()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[2]++;
			totalChosenOrder++;
		}
	}
		public void ChooseFood04()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[3]++;
			totalChosenOrder++;
		}
	}
		public void ChooseFood05()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[4]++;
			totalChosenOrder++;
		}
	}
		public void ChooseFood06()
	{
		if(totalChosenOrder < 4)
		{
			nextOrderCount[5]++;
			totalChosenOrder++;
		}
	}

	public void NextTrayFood01()
	{
		nextTrayFood.Add(FoodType.A);
	}
		public void NextTrayFood02()
	{
		nextTrayFood.Add(FoodType.B);
	}
		public void NextTrayFood03()
	{
		nextTrayFood.Add(FoodType.C);
	}
		public void NextTrayFood04()
	{
		nextTrayFood.Add(FoodType.D);
	}
		public void NextTrayFood05()
	{
		nextTrayFood.Add(FoodType.E);
	}
		public void NextTrayFood06()
	{
		nextTrayFood.Add(FoodType.F);
	}
#endregion

	public void AddChosenCustomer()
	{
		int nextCustomerIndex = 1;
		for(int i = 0; i < nextCustomerChoices.Length; i++)
		{
			if(nextCustomerChoices[i].isOn)
			{
				nextCustomerIndex = i + 1;
				nextCustomerChoices[i].isOn = false;
				break;
			}
			else
			nextCustomerIndex = Random.Range(1, 30);
		}

		nextCustomers.Add(nextCustomerIndex);

		// 입력된만큼 넣고, 입력 안 된만큼은 랜덤하게 넣기
		List<FoodType> nextOrder = new List<FoodType>();
		for(int i = 0; i < 6; i++)
		{
			for(int j = 0; j < nextOrderCount[i]; j++)
			{
				nextOrder.Add((FoodType)i);
			}
		}

		while(nextOrder.Count < 4)
		{
			nextOrder.Add((FoodType)Random.Range(0, 6));
		}

		nextOrders.Add(nextOrder);
		totalChosenOrder = 0;
		for(int i = 0; i < 6; i++)
		{
			nextOrderCount[i] = 0;
		}
	}

	// Use this for initialization
	void Start () {
		MissionData.foodTypeCount = 6;

		nextOrderCount = new int[6];
		for(int i = 0; i < 6; i++)
		{
			nextOrderCount[i] = 0;
		}
		totalChosenOrder = 0;
		nextOrders = new List<List<FoodType>>();
	}
	
	// Update is called once per frame
	void Update () {
		randomizeCustomer = (randomToggle.isOn) ? true : false;
		int nextCustomerCount = nextCustomers.Count;
		for(int i = 0; i < nextCustomerImages.Length; i++){
			if(i < nextCustomerCount)
			{
				Rabbit rabbitData = RabbitData.GetRabbitData(nextCustomers[i]); 
				string spriteName = rabbitData.gender.ToString() + "/" + rabbitData.imageName;
				Sprite customerSprite = Resources.Load<Sprite>("customers/" + spriteName);
				nextCustomerImages[i].sprite = customerSprite;
				nextCustomerImages[i].enabled = true;
			}
			else
			{
				nextCustomerImages[i].enabled = false;
			}
		}

		int nextTrayCount = nextTrayFood.Count;
		for(int i = 0; i < nextTrayImages.Length; i++)
		{
			if(i < nextTrayCount)
			{
				string spriteName = "Foods/World1/food0" + ((int)nextTrayFood[i] + 1).ToString("N0");
				Sprite foodSprite = Resources.Load<Sprite>(spriteName);
				nextTrayImages[i].sprite = foodSprite;
				nextTrayImages[i].enabled = true;
			}
			else
			{
				nextTrayImages[i].enabled = false;
			}
		}

		for(int i = 0; i < nextOrderCountText.Length; i++)
		{
			nextOrderCountText[i].text = nextOrderCount[i].ToString("N0");
		}
	}
}
