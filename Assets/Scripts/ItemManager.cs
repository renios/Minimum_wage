using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

    public GameObject TimeItem;
    public GameObject SuperFood;
    public GameObject TrayItem;
    public Button TimeItemButton;
    public Button MakeSuperfoodButton;
    public Button ResetTrayButton; 
    public GameObject OpenBin;
    public GameObject ClosedBin;

    TrayManager trayManager;
    FeverManager feverManager;
    CustomerManager customerManager;

    void Start () {
        trayManager = FindObjectOfType<TrayManager>();
        feverManager = FindObjectOfType<FeverManager>();
        customerManager = FindObjectOfType<CustomerManager>();
    }

	// Update is called once per frame
	void Update () {
        if (MissionData.gotTimeItem && !TimeItemButton.interactable)
            TimeItemButton.interactable = true;
        else if (!MissionData.gotTimeItem && TimeItemButton.interactable)
            TimeItemButton.interactable = false;
        if (MissionData.gotSuperfood && !MakeSuperfoodButton.interactable)
            MakeSuperfoodButton.interactable = true;
        else if (!MissionData.gotSuperfood && MakeSuperfoodButton.interactable)
            MakeSuperfoodButton.interactable = false;
        if (MissionData.gotTrayItem && !ResetTrayButton.interactable)
            ResetTrayButton.interactable = true;
        else if (!MissionData.gotTrayItem && ResetTrayButton.interactable)
            ResetTrayButton.interactable = false;
    }

    public void UseMakeSuperfoodItem() {
        feverManager.MakeSuperfoodByFever(Camera.main.ScreenToWorldPoint(MakeSuperfoodButton.transform.position));
        trayManager.MakeSuperfood();
    }

    public void UseTimeResetItem() {
        customerManager.ResetWaitingTime();
    }

    public void UseResetTrayItem() {
        trayManager.StartRenewTray();
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
