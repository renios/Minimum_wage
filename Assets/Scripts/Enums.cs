using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums {

	public class ServedPair {
		public Customer customer;
		public List<FoodOnTray> foods;

		public ServedPair (Customer customer, List<FoodOnTray> foods) {
			this.customer = customer;
			this.foods = foods;
		}
	}

	public enum FoodType {
		A,
		B,
		C,
		D,
		E,
		F
	}
	public enum Gender{
		Female, Male
	}

	public enum GameState {
		Start,
		Idle,
		Picked,
		Dropped,
		Change,
		Matching,
		Combo,
		Refill,
		RenewTray,
		UseItem,
		FeverBonus,
		Result,
		End
	}
}
