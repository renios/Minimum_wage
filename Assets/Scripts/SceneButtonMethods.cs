using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonMethods : MonoBehaviour
{
    public void GoToWorld()
    {
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
        if (missionPanel.resetTimeItem.isOn == true) MissionData.gotTimeItem = true;
        if (missionPanel.superfoodItem.isOn == true) MissionData.gotSuperfood = true;
        if (missionPanel.renewTrayItem.isOn == true) MissionData.gotTrayItem = true;
        int progress = PlayerPrefs.GetInt("Progress", -1);
        if (progress < 2)
        {
            GoToTutorial();
            return;
        }
        GoIngame();
    }

    public GameObject[] Tutorials;

    int index = 0;

    public void NextTutorialPanel()
    {
        if (index == 4)
            GoIngame();

        else {
            index++;
            foreach (var tutorial in Tutorials)
                tutorial.SetActive(false);
            Tutorials[index].SetActive(true);
        }
    }

    public void PrevTutorialPanel()
    {
        if (index == 0) return;
        index--;
        foreach (var tutorial in Tutorials)
            tutorial.SetActive(false);
        if (index < 1)
            GoToWorld();
        else
            Tutorials[index].SetActive(true);
    }
}
