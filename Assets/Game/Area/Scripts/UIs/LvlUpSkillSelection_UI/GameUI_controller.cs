using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] GameObject comboTutoScreen = null;

    [SerializeField] Animator bossNameAnim = null;
    [SerializeField] TextMeshProUGUI bossName = null;
    [SerializeField] TextMeshProUGUI bossDescription = null;


    [SerializeField] Image customImage = null;
    

    public FrontendStatBase lifeHeart;
    

    #region Public methods
    
    //public RectTransform GetRectCanvas() => completeCanvas;
    
    public bool openUI { get; private set; }
    public void UI_Send_NameSkillType(string s) { }
    public void SetSelectedPath(string pathName){}

    public void UI_RefreshMenu(){} 
    public void OpenGameMenu(){}
    public void CloseGameMenu(){}

    public void OpenBossName(string _bossName, string _bossDesc)
    {
        bossName.text = _bossName;
        bossDescription.text = _bossDesc;
        bossNameAnim.Play("Appear");
    }


    public bool OpenCustomImage(Sprite img)
    {
        Debug.Log("Llego aca?");
        if (customImage.gameObject.activeSelf) return false;
        customImage.sprite = img;
        customImage.gameObject.SetActive(true);
        return true;
    }

    public void CloseCustomImage()
    {
        customImage.gameObject.SetActive(false);
    }
    #endregion

    public void OnChangeLife(int current, int max)
    {
        lifeHeart.OnValueChange(current, max);
        lifeHeart.OnValueChangeWithDelay(current,1f,max, true);
    }

    public void ActiveOrDesactiveComboScreen(bool b) => comboTutoScreen.SetActive(b);
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
