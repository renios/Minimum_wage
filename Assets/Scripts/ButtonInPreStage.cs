using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInPreStage : MonoBehaviour {
    public MissionPanel missionPanel;

    public void GoToStage() {

        if (missionPanel.resetTimeItem.isOn == true) MissionData.gotTimeItem = true;
        if (missionPanel.superfoodItem.isOn == true) MissionData.gotSuperfood = true;
        if (missionPanel.renewTrayItem.isOn == true) MissionData.gotTrayItem = true;
        int progress = PlayerPrefs.GetInt("Progress", -1);
        if (progress < 2)
        {
            SceneManager.LoadScene("Tutorial");
            return;
        }
        SceneManager.LoadScene("Ingame");
	}

	public void GoToTitle() {
		SceneManager.LoadScene("Title");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
