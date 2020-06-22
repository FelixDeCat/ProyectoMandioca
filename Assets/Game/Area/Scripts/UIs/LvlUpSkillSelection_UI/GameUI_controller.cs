using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsMandioca.Extensions;
using UnityEngine.UI;

public class GameUI_controller : MonoBehaviour
{
    [SerializeField] Canvas myCanvas = null; public Canvas MyCanvas { get => myCanvas; }

    //public RectTransform CompleteParentInstancer { get => completeCanvas; }
    
    //private CharStats_UI _charStats_Ui;
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
    
    
    //Seguroi esto se deje de usar
    //Dictionary<UI_templates, GameObject> UiTemplateRegistry = new Dictionary<UI_templates, GameObject>();
    

    public bool openUI { get; private set; }


    #region Config

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    //Esto est amuy feo, tengo que ponerlo mas lindo
    void Initialize(){shieldsController = GetComponent<UI2D_Shields_controller>();}

    #endregion

    #region Public methods
    
    public void RefreshShields_UI(int currentShields, int maxShields) =>  shieldsController.RefreshUI(currentShields, maxShields);
    public RectTransform GetRectCanvas() => completeCanvas;
    
    public void UI_Send_NameSkillType(string s) { }
    //public void UI_SendLevelUpNotification() {/* CanvasPopUpInWorld_Manager.instance.MakePopUpAnimated(Main.instance.GetChar().transform, lvlUp_pf);*/ }
    //public void UI_SendActivePlusNotification(bool val) { if (val) _charStats_Ui.ToggleLvlUpSignON(); }
    //public void RefreshPassiveSkills_UI(List<SkillInfo> skillsNuevas) => _charStats_Ui.UpdatePasiveSkills(skillsNuevas);
    public void SetSelectedPath(string pathName){}

    public void UI_RefreshMenu(){} //=> gameMenu_UI.Refresh();
    public void OpenGameMenu(){} // => gameMenu_UI.Open();
    public void CloseGameMenu(){}// => gameMenu_UI.Close();
    #endregion

    public void OnChangeLife(int current, int max)
    {
        lifeHeart.OnValueChange(current, max);
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
