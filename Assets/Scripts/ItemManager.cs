using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

    public GameObject TimeItem;
    public GameObject SuperFoodItem;
    public GameObject ResetTrayItem;
    public Button TimeItemButton;
    public Button MakeSuperfoodItemButton;
    public Button ResetTrayItemButton; 
    public ParticleSystem TimeItemEffect;
    public ParticleSystem MakeSuperfoodItemEffect;
    public ParticleSystem ResetTrayItemEffect;
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
        if (MissionData.gotTimeItem && !TimeItemButton.interactable) {
            TimeItemButton.interactable = true;
            TimeItemEffect.Play();
        }
        else if (!MissionData.gotTimeItem && TimeItemButton.interactable) {
            TimeItemButton.interactable = false;
            TimeItemEffect.Stop();
        }
        if (MissionData.gotSuperfood && !MakeSuperfoodItemButton.interactable) {
            MakeSuperfoodItemButton.interactable = true;
            MakeSuperfoodItemEffect.Play();
        }
        else if (!MissionData.gotSuperfood && MakeSuperfoodItemButton.interactable) {
            MakeSuperfoodItemButton.interactable = false;
            MakeSuperfoodItemEffect.Stop();
        }
        if (MissionData.gotTrayItem && !ResetTrayItemButton.interactable) {
            ResetTrayItemButton.interactable = true;
            ResetTrayItemEffect.Play();
        }
        else if (!MissionData.gotTrayItem && ResetTrayItemButton.interactable) {
            ResetTrayItemButton.interactable = false;
            ResetTrayItemEffect.Stop();
        }
    }

    public void UseMakeSuperfoodItem() {
        feverManager.MakeSuperfoodByFever(MakeSuperfoodItemButton.transform.position);
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
