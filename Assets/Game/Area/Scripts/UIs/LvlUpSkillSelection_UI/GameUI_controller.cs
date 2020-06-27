using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.UI;

public class GameUI_controller : MonoBehaviour
{
    [SerializeField] Canvas myCanvas = null; public Canvas MyCanvas { get => myCanvas; }

    public UI2D_Shields_controller shieldsController;
    
    [Header("--XX--Canvas containers--XX--")]
    [SerializeField] private RectTransform leftCanvas = null;
    [SerializeField] private RectTransform rightCanvas = null;
    [SerializeField] private RectTransform completeCanvas = null;

    //No me mates, es pa probar rapido si funciona
    public Image skillImage;
    public Text skillInfoTxt;
    public Text skillName;
    public GameObject skillInfoContainer;

    private SkillManager_Pasivas _skillManagerPasivas;

    public FrontendStatBase lifeHeart;
    

    #region Config

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }
    void Initialize(){shieldsController = GetComponent<UI2D_Shields_controller>();}

    #endregion

    #region Public methods
    
    public void RefreshShields_UI(int currentShields, int maxShields) =>  shieldsController.RefreshUI(currentShields, maxShields);
    //public RectTransform GetRectCanvas() => completeCanvas;
    
    public bool openUI { get; private set; }
    public void UI_Send_NameSkillType(string s) { }
    public void SetSelectedPath(string pathName){}

    public void UI_RefreshMenu(){} 
    public void OpenGameMenu(){}
    public void CloseGameMenu(){}
    #endregion

    public void OnChangeLife(int current, int max)
    {
        lifeHeart.OnValueChange(current, max);
        lifeHeart.OnValueChangeWithDelay(current,1f,max, true);
    }
    public void ResetYellowHeart(){}
    public void Set_Opened_UI() { openUI = true; Main.instance.Pause(); }
    public void Set_Closed_UI() { openUI = false; Main.instance.Play(); }
    public void BTN_Back_OpenMenu()
    {
        if (!openUI)
        {
            OpenGameMenu();
            Set_Opened_UI();
        }
        else
        {
            CloseGameMenu();
            Set_Closed_UI();
        }
    }
}
