using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums {

	public class Rabbit {
		// 토끼의 스탯: 인덱스, 해금스테이지, 성별, 이미지, 참을성, vip여부, 라이프감소, 주문음식종류
		public int index = 1;
		public int releaseStageIndex = 1;
		public Gender gender = Gender.Male;
		public string imageName = "yoonsung";
		public int waitingTime = 40;
		public bool isVip = false; // vip는 서빙 실패시 원킬
		public int reduceHeartsByFail = 1;
		public List<int> variablesOfOrderFood = new List<int> {3, 4};
	}

	public class ServedPair {
		public Customer customer;
		public List<FoodOnTray> foods;

		public ServedPair (Customer customer, List<FoodOnTray> foods) {
			this.customer = customer;
			this.foods = foods;
		}
	}
	public enum RabbitGroup {
		LeisurelyMore, 
		LeisurelyDouble, 
		LeisurelySingle, 
		NormalMore, 
		NormalDouble, 
		NormalSingle, 
		HastyMore, 
		HastyDouble, 
		HastySingle, 
		VIP, 
		FullLevel
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
		Paused,
		FeverBonus,
		Result,
		End
	}
}
