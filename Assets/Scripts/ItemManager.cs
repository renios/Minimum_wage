using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

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

    public Material grayScale;

    TrayManager trayManager;
    FeverManager feverManager;
    CustomerManager customerManager;

    GameStateManager gameStateManager;

    void Start () {
        trayManager = FindObjectOfType<TrayManager>();
        feverManager = FindObjectOfType<FeverManager>();
        customerManager = FindObjectOfType<CustomerManager>();
        gameStateManager = FindObjectOfType<GameStateManager>();

        Initialize();
    }

    void Initialize() {
        if (!MissionData.gotTimeItem) {
            TimeItemButton.GetComponent<Image>().material = grayScale;
        }
        if (!MissionData.gotSuperfood) {
            MakeSuperfoodItemButton.GetComponent<Image>().material = grayScale;
        }
        if (!MissionData.gotTrayItem) {
            ResetTrayItemButton.GetComponent<Image>().material = grayScale;
        }
    }

    bool raycastTargetEnabled = true;

	// Update is called once per frame
	void Update () {
        if (raycastTargetEnabled && gameStateManager.gameState != GameState.Idle) {
            TimeItemButton.GetComponent<Image>().raycastTarget = false;
            MakeSuperfoodItemButton.GetComponent<Image>().raycastTarget = false;
            ResetTrayItemButton.GetComponent<Image>().raycastTarget = false;
            raycastTargetEnabled = false;
        }
        else if (!raycastTargetEnabled && gameStateManager.gameState == GameState.Idle) {
            TimeItemButton.GetComponent<Image>().raycastTarget = true;
            MakeSuperfoodItemButton.GetComponent<Image>().raycastTarget = true;
            ResetTrayItemButton.GetComponent<Image>().raycastTarget = true;
            raycastTargetEnabled = true;
        }


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
            MakeSuperfoodItemEffect.Stop();
            MakeSuperfoodItemButton.interactable = false;
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
        if (FindObjectOfType<GameStateManager>().gameState == GameState.Idle
            && MissionData.gotSuperfood == true) 
        {
            StartCoroutine(UseMakeSuperfoodItemCoroutine());
            MissionData.gotSuperfood = false;
        }
    }

    IEnumerator UseMakeSuperfoodItemCoroutine() {
        FindObjectOfType<GameStateManager>().gameState = GameState.UseItem;

        GameObject newSuperfood;
        newSuperfood = trayManager.FindSuperfoodTarget();

        if (newSuperfood != null)
        {
            Vector3 endPos = newSuperfood.transform.position;
            GameObject makeSuperfoodEffect = 
                Instantiate(feverManager.makeSuperfoodEffectPrefab, MakeSuperfoodItemButton.transform.position, Quaternion.identity);
            yield return StartCoroutine(makeSuperfoodEffect.GetComponent<MakeSuperfoodAnim>().StartAnim(MakeSuperfoodItemButton.transform.position, endPos));
        }

        yield return StartCoroutine(newSuperfood.GetComponent<FoodOnTray>().ChangeToSuperfood());

        MakeSuperfoodItemButton.GetComponent<Image>().material = grayScale;

        FindObjectOfType<GameStateManager>().gameState = GameState.Idle;
    }

    public void UseTimeResetItem() {
        if (FindObjectOfType<GameStateManager>().gameState == GameState.Idle) {
            FindObjectOfType<GameStateManager>().gameState = GameState.UseItem;
            customerManager.ResetWaitingTime();
            TimeItemButton.GetComponent<Image>().material = grayScale;
            FindObjectOfType<GameStateManager>().gameState = GameState.Idle;
        }
    }

    public void UseResetTrayItem() {
        if (FindObjectOfType<GameStateManager>().gameState == GameState.Idle && MissionData.gotTrayItem == true) {
            StartCoroutine(UseResetTrayItemCoroutine());
        }
    }

    IEnumerator UseResetTrayItemCoroutine() {
        FindObjectOfType<GameStateManager>().gameState = GameState.UseItem;    
        yield return StartCoroutine(trayManager.RenewTray());
        MissionData.gotTrayItem = false;
        ResetTrayItemButton.GetComponent<Image>().material = grayScale;
        FindObjectOfType<GameStateManager>().gameState = GameState.Idle;
    }

    public void BinOpen()
    {
        OpenBin.SetActive(true);
        ClosedBin.SetActive(false);
        trayManager.isOnBin = true;
    }

    public void BinClose()
    {
        OpenBin.SetActive(false);
        ClosedBin.SetActive(true);
        trayManager.isOnBin = false;
    }
}
