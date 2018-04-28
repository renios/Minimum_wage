using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonMethods : MonoBehaviour {
    public void GoToWorld()
    {
        if (MissionData.gotTimeItem)
        {
            PlayerPrefs.SetInt("TimerReset", PlayerPrefs.GetInt("TimerReset", 0) + 1);
            MissionData.gotTimeItem = false;
        }
        if (MissionData.gotSuperfood)
        {
            PlayerPrefs.SetInt("Superfood", PlayerPrefs.GetInt("Superfood", 0) + 1);
            MissionData.gotSuperfood = false;
        }
        if (MissionData.gotTrayItem)
        {
            PlayerPrefs.SetInt("TrayReset", PlayerPrefs.GetInt("TrayReset", 0) + 1);
            MissionData.gotTrayItem = false;
        }
        SoundManager.Play(SoundType.Button);
       
        if (PlayerPrefs.GetInt("TutorialFinished", 0) == 0) {
            SceneManager.LoadScene("World_tutorial"); 
        }
        else {
            SceneManager.LoadScene("World");
        }
    }

    public void GoToTutorial()
    {
        SoundManager.Play(SoundType.Button);
        SceneManager.LoadScene("Tutorial_new");
    }

    public void GoIngame()
    {
        SoundManager.Play(SoundType.Button);
        SceneManager.LoadScene("Ingame");
    }

    public MissionPanel missionPanel;

    public void GoToStage()
    {
        if (missionPanel.resetTimeItem.isOn == true)
        {
            MissionData.gotTimeItem = true;
            PlayerPrefs.SetInt("TimerReset", PlayerPrefs.GetInt("TimerReset", 1) - 1);
        }
        if (missionPanel.superfoodItem.isOn == true)
        {
            MissionData.gotSuperfood = true;
            PlayerPrefs.SetInt("Superfood", PlayerPrefs.GetInt("Superfood", 1) - 1);
        }
        if (missionPanel.renewTrayItem.isOn == true)
        {
            MissionData.gotTrayItem = true;
            PlayerPrefs.SetInt("TrayReset", PlayerPrefs.GetInt("TrayReset", 1) - 1);
        }

        if (PlayerPrefs.GetInt("TutorialFinished", 0) == 0)
        {
            MissionData.fromStage = true;
            GoToTutorial();
        }
        else {
            MissionData.fromStage = true;
            GoIngame();
        }
    }

    public GameObject[] Tutorials;

    int index = 0;

    public void NextTutorialPanel()
    {
        if (index == Tutorials.Length - 1)
        {
            if (MissionData.fromStage == true)
            {
                MissionData.fromStage = false;
                GoIngame();
            }
            else
            {
                if (MissionData.gotTimeItem)
                {
                    PlayerPrefs.SetInt("TimerReset", PlayerPrefs.GetInt("TimerReset", 0) + 1);
                    MissionData.gotTimeItem = false;
                }
                if (MissionData.gotSuperfood)
                {
                    PlayerPrefs.SetInt("Superfood", PlayerPrefs.GetInt("Superfood", 0) + 1);
                    MissionData.gotSuperfood = false;
                }
                if (MissionData.gotTrayItem)
                {
                    PlayerPrefs.SetInt("TrayReset", PlayerPrefs.GetInt("TrayReset", 0) + 1);
                    MissionData.gotTrayItem = false;
                }
                GoToWorld();
            }
        }

        else {
            index++;
            SoundManager.Play(SoundType.Button);
            foreach (var tutorial in Tutorials)
                tutorial.SetActive(false);
            Tutorials[index].SetActive(true);
        }
    }

    public void PrevTutorialPanel()
    {
        if (index == 0)
        {
            GoToWorld();
            return;
        }
        index--;
        SoundManager.Play(SoundType.Button);
        foreach (var tutorial in Tutorials)
            tutorial.SetActive(false);
        Tutorials[index].SetActive(true);
    }
}
