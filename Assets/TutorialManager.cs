using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    public GameObject[] Tutorials;

    int index = 0;

    public void NextTutorialPanel()
    {
        index++;
        foreach (var tutorial in Tutorials)
            tutorial.SetActive(false);
        Tutorials[index].SetActive(true);
        if (index > 4)
            GoIngame();
    }

    public void PrevTutorialPanel()
    {
        index--;
        foreach (var tutorial in Tutorials)
            tutorial.SetActive(false);
        if (index < 1)
            GoWorld();
        else
            Tutorials[index].SetActive(true);
    }

    public void GoIngame()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void GoWorld()
    {
        SceneManager.LoadScene("World");
    }
}
