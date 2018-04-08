using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public static class RabbitData {

	public static int numberOfRabbitData = 29;

	public static Rabbit GetRabbitData(int index) {
		Rabbit newRabbit = new Rabbit();

		switch (index) {
			case 1:
				newRabbit.index = 1;
				newRabbit.releaseStageIndex = 1;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "yoonsung";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 2:
				newRabbit.index = 2;
				newRabbit.releaseStageIndex = 1;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "hwangsoon";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 3:
				newRabbit.index = 3;
				newRabbit.releaseStageIndex = 1;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "ahyoung";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 4:
				newRabbit.index = 4;
				newRabbit.releaseStageIndex = 1;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "donghoon";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 5:
				newRabbit.index = 5;
				newRabbit.releaseStageIndex = 3;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "donghak";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 6:
				newRabbit.index = 6;
				newRabbit.releaseStageIndex = 3;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "hyunseok";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 7:
				newRabbit.index = 7;
				newRabbit.releaseStageIndex = 3;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "junghwa";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 8:
				newRabbit.index = 8;
				newRabbit.releaseStageIndex = 2;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "eq";
				newRabbit.waitingTime = 40;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 9:
				newRabbit.index = 9;
				newRabbit.releaseStageIndex = 12;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "minjung";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 10:
				newRabbit.index = 10;
				newRabbit.releaseStageIndex = 12;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "haram";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 11:
				newRabbit.index = 11;
				newRabbit.releaseStageIndex = 7;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "orchid";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 12:
				newRabbit.index = 12;
				newRabbit.releaseStageIndex = 7;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "jaeyoung";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 13:
				newRabbit.index = 13;
				newRabbit.releaseStageIndex = 7;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "youngsang";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 14:
				newRabbit.index = 14;
				newRabbit.releaseStageIndex = 14;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "smz";
				newRabbit.waitingTime = 30;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 15:
				newRabbit.index = 15;
				newRabbit.releaseStageIndex = 5;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "hongbeom";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 16:
				newRabbit.index = 16;
				newRabbit.releaseStageIndex = 5;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "normal";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 17:
				newRabbit.index = 17;
				newRabbit.releaseStageIndex = 5;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "jiho";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 18:
				newRabbit.index = 18;
				newRabbit.releaseStageIndex = 5;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "moon";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 19:
				newRabbit.index = 19;
				newRabbit.releaseStageIndex = 16;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "yj";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 20:
				newRabbit.index = 20;
				newRabbit.releaseStageIndex = 16;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "summer";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {2};
				break;
			case 21:
				newRabbit.index = 21;
				newRabbit.releaseStageIndex = 9;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "hyosang";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 22:
				newRabbit.index = 22;
				newRabbit.releaseStageIndex = 9;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "sungchan";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 23:
				newRabbit.index = 23;
				newRabbit.releaseStageIndex = 9;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "hyunjun";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 24:
				newRabbit.index = 24;
				newRabbit.releaseStageIndex = 9;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "naeri";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = false;
				newRabbit.reduceHeartsByFail = 1;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			case 25:
				newRabbit.index = 25;
				newRabbit.releaseStageIndex = 4;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "king";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = true;
				newRabbit.reduceHeartsByFail = 3;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 26:
				newRabbit.index = 26;
				newRabbit.releaseStageIndex = 4;
				newRabbit.gender = Gender.Male;
				newRabbit.imageName = "muscle";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = true;
				newRabbit.reduceHeartsByFail = 3;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 27:
				newRabbit.index = 27;
				newRabbit.releaseStageIndex = 4;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "vip";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = true;
				newRabbit.reduceHeartsByFail = 3;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 28:
				newRabbit.index = 28;
				newRabbit.releaseStageIndex = 4;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "bitsal";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = true;
				newRabbit.reduceHeartsByFail = 3;
				newRabbit.variablesOfOrderFood = new List<int> {3, 4};
				break;
			case 29:
				newRabbit.index = 29;
				newRabbit.releaseStageIndex = 13;
				newRabbit.gender = Gender.Female;
				newRabbit.imageName = "fullLevel";
				newRabbit.waitingTime = 20;
				newRabbit.isVip = true;
				newRabbit.reduceHeartsByFail = 3;
				newRabbit.variablesOfOrderFood = new List<int> {1};
				break;
			default:
				break;
		}

		return newRabbit;
	}
}
