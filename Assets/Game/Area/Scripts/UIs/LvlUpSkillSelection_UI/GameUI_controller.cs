﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.UI;

public class GameUI_controller : MonoBehaviour
{
    [SerializeField] Canvas myCanvas = null; public Canvas MyCanvas { get => myCanvas; }
    
    [Header("--XX--Canvas containers--XX--")]
    //[SerializeField] private RectTransform leftCanvas = null;
    //[SerializeField] private RectTransform rightCanvas = null;
    //[SerializeField] private RectTransform completeCanvas = null;

    //No me mates, es pa probar rapido si funciona
    public Image skillImage;
    public Text skillInfoTxt;
    public Text skillName;
    public GameObject skillInfoContainer;
    [SerializeField] Image combatStateImage = null;
    [SerializeField] Sprite[] combatStatesSprites = new Sprite[0];


    private SkillManager_Pasivas _skillManagerPasivas;

    public FrontendStatBase lifeHeart;
    

    #region Public methods
    
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

    public void ChangeCombat(int stateIndex)
    {
        combatStateImage.sprite = combatStatesSprites[stateIndex];
    }
}
