using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class WorldTutorial : MonoBehaviour
{
	public List<Sprite> WorldTutorialImages;
	public List<Sprite> CatalogTutorialImages;
	public List<Sprite> MissionPanelTutorialImages;

	public Image currentImage;
	public Image bgImage;

	IEnumerator ShowWorldTutorial()
	{
		currentImage.enabled = true;
		bgImage.enabled = true;
		
		foreach (var image in WorldTutorialImages)
		{
			currentImage.sprite = image;
			yield return new WaitForSeconds(2);
		}

		PlayerPrefs.SetInt("WorldTutorialFinished", 1);
		bgImage.enabled = false;
		currentImage.enabled = false;
	}

	IEnumerator ShowMissionPanelTutorial()
	{
		currentImage.enabled = true;
		bgImage.enabled = true;
		
		foreach (var image in MissionPanelTutorialImages)
		{
			currentImage.sprite = image;
			yield return new WaitForSeconds(2);
		}

		PlayerPrefs.SetInt("MissionPanelTutorialFinished", 1);
		bgImage.enabled = false;
		currentImage.enabled = false;
	}
	
	IEnumerator ShowCatalogTutorial()
	{
		currentImage.enabled = true;
		bgImage.enabled = true;
		
		foreach (var image in CatalogTutorialImages)
		{
			currentImage.sprite = image;
			yield return new WaitForSeconds(2);
		}

		PlayerPrefs.SetInt("CatalogTutorialFinished", 1);
		bgImage.enabled = false;
		currentImage.enabled = false;
	}
	
	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.GetInt("WorldTutorialFinished", 0) == 0)
		{
			StartCoroutine(ShowWorldTutorial());
			return;
		}

//		if (PlayerPrefs.GetInt("MissionPanelTutorialFinished", 0) == 0)
//		{
//			StartCoroutine(ShowMissionPanelTutorial());
//			return;	
//		}
		
		if (PlayerPrefs.GetInt("CatalogTutorialFinished", 0) == 0)
		{
			StartCoroutine(ShowCatalogTutorial());
			return;	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
