using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums {
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
		End
	}
}
