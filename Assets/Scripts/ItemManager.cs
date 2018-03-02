using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public GameObject TimeItem;
    public GameObject SuperFood;
    public GameObject TrayItem;
    public GameObject OpenBin;
    public GameObject ClosedBin;
	
	// Update is called once per frame
	void Update () {
        if (MissionData.gotTimeItem == true) TimeItem.SetActive(true);
        else TimeItem.SetActive(false);
        if (MissionData.gotSuperfood == true) SuperFood.SetActive(true);
        else SuperFood.SetActive(false);
        if (MissionData.gotTrayItem == true) TrayItem.SetActive(true);
        else TrayItem.SetActive(false);
    }

    public void BinOpen()
    {
        OpenBin.SetActive(true);
        ClosedBin.SetActive(false);
    }

    public void BinClose()
    {
        OpenBin.SetActive(false);
        ClosedBin.SetActive(true);
    }
}
