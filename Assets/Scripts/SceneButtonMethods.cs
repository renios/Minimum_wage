using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonMethods : MonoBehaviour
{
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
        SceneManager.LoadScene("World");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoIngame()
    {
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
        int progress = PlayerPrefs.GetInt("Progress", -1);
        if (progress < 2)
        {
            MissionData.fromStage = true;
            GoToTutorial();
            return;
        }
        GoIngame();
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
        foreach (var tutorial in Tutorials)
            tutorial.SetActive(false);
        Tutorials[index].SetActive(true);
    }
}
