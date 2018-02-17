using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour {

	public Text todoText;
	public Text dayText;
	public Text customerText;
	public Text timeText;

	public void LoadMissonInfo(string stageName) {
		if (stageName == "1-1") {
			dayText.text = "DAY 1";
			todoText.text = "1) 3명 이상의 손님을" + '\n' + "돌려보내지 않기" + '\n'  + '\n' +  
							"2) 90초 이내에" + '\n' + "15명 이상 서빙하기";
			customerText.text = ": 15+";
			timeText.text = ": 1:30";
		}
		else if (stageName == "1-2") {
			dayText.text = "DAY 2";
			todoText.text = "1) 3명 이상의 손님을" + '\n' + "돌려보내지 않기" + '\n'  + '\n' +  
							"2) 120초 이내에" + '\n' + "30명 이상 서빙하기";
			customerText.text = ": 30+";
			timeText.text = ": 2:00";
		}
	}
}
