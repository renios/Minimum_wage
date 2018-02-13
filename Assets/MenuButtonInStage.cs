using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonInStage : MonoBehaviour {
    public bool  isHeld;
    public float feedbackTime;
    public float maxFeedbackTime;
    public float feedbackRate;
    public float settleRate;
    Vector2 originalScale;
    Image buttonImage;

    public void StartFeedback()
    {
        if(isHeld==false)
        {
            isHeld = true;
            originalScale = buttonImage.transform.localScale;
        }
    }

    public void EndFeedback()
    {
        isHeld = false;
        feedbackTime = 0;
        buttonImage.transform.localScale = originalScale;
        SceneManager.LoadScene("World");
    }

    // Use this for initialization
    void Start () {
        isHeld = false;
        buttonImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isHeld==true)
        {
            print("is held");
            feedbackTime += Time.deltaTime;
            if(feedbackTime<maxFeedbackTime)
            {
                buttonImage.transform.localScale = originalScale * (1f + feedbackRate * (feedbackTime / maxFeedbackTime));
            }
            else
            {
                buttonImage.transform.localScale = originalScale * (1f + settleRate);
            }
        }
	}
}
