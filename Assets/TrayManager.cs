using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour {

	List<GameObject> trays;
	public GameObject pickedFood1;
	public GameObject pickedFood2;

	void ChangeFoodPosition(GameObject food1, GameObject food2) {
		// 트레이와 위치를 둘 다 바꿔야 함
		Transform parentOfFood1 = food1.transform.parent;
		Vector3 positionOfFood1 = food1.transform.position;
		Transform parentOfFood2 = food2.transform.parent;
		Vector3 positionOfFood2 = food2.transform.position;

		Transform parentTemp = food1.transform.parent;
		Vector3 positionTemp = food1.transform.position;

		food1.transform.parent = parentOfFood2;
		food2.transform.parent = parentTemp;

		food1.transform.position = positionOfFood2;
		food2.transform.position = positionTemp;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
				pickedFood1 = hit.collider.gameObject;
            }
		}	

		if (Input.GetMouseButtonUp(0)) {
			//Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
				pickedFood2 = hit.collider.gameObject;
            }

			if ((pickedFood1 != null) && (pickedFood2 != null)) {
				ChangeFoodPosition(pickedFood1, pickedFood2);
			}
		}
	}
}
