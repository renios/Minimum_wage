using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTutorial : MonoBehaviour
{
	public List<Sprite> WorldTutorialImages;
	public List<Sprite> CatalogTutorialImages;
	public List<Sprite> MissionPanelTutorialImages;

	public Image currentImage;
	public Image bgImage;

	public Image catalogBlockPanel;
	public Image catalogArrow;

	public bool catalogOpenedInTutorial;

	IEnumerator ShowWorldTutorial()
	{
		currentImage.enabled = true;
		bgImage.enabled = true;
		
		foreach (var image in WorldTutorialImages)
		{
			currentImage.sprite = image;
			yield return new WaitForSeconds(3);
		}

		PlayerPrefs.SetInt("WorldTutorialFinished", 1);
		bgImage.enabled = false;
		currentImage.enabled = false;
	}

	public IEnumerator ShowMissionPanelTutorial()
	{
		currentImage.enabled = true;
		bgImage.enabled = true;

		foreach (var image in MissionPanelTutorialImages)
		{
			currentImage.sprite = image;
			yield return new WaitForSeconds(3);
		}

		PlayerPrefs.SetInt("MissionPanelTutorialFinished", 1);
		bgImage.enabled = false;
		currentImage.enabled = false;
	}
	
	IEnumerator ShowCatalogTutorial()
	{
		catalogOpenedInTutorial = false;
		currentImage.enabled = true;
		bgImage.enabled = true;

		var images = CatalogTutorialImages.Count;
		for (int index = 0; index < images; index++)
		{
			if (index == 1)
			{
				currentImage.enabled = false;
				bgImage.enabled = false;
				catalogBlockPanel.enabled = true;
				catalogArrow.enabled = true;
				while (!catalogOpenedInTutorial)
				{
					yield return null;
				}
				catalogArrow.enabled = false;
				catalogBlockPanel.enabled = false;
				bgImage.enabled = true;
				currentImage.enabled = true;
			}

			currentImage.sprite = CatalogTutorialImages[index];
			yield return new WaitForSeconds(3);
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
		
		if (PlayerPrefs.GetInt("PlayProgress", 0) > 0 && 
		    PlayerPrefs.GetInt("CatalogTutorialFinished", 0) == 0)
		{
			StartCoroutine(ShowCatalogTutorial());
			return;	
		}
	}
}
