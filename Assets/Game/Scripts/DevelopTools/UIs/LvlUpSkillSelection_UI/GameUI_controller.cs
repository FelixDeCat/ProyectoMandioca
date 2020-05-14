using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsMandioca.Extensions;
using UnityEngine.UI;

public class GameUI_controller : MonoBehaviour
{
    [SerializeField] Canvas myCanvas = null; public Canvas MyCanvas { get => myCanvas; }

    [SerializeField] private GameObject skillSelection_template_pf = null;
    [SerializeField] private GameObject charStats_template_pf = null;
    
    [SerializeField] private GameMenu_UI gameMenu_UI = null;

    [SerializeField] public RectTransform shieldPLaces;

    private CharStats_UI _charStats_Ui;
    [SerializeField] private GameObject stats3D_UI_pf;
    private GameObject stats3D_UI;
    Dictionary<UI_templates, GameObject> UiTemplateRegistry = new Dictionary<UI_templates, GameObject>();

    
    
    [Header("--XX--Canvas containers--XX--")]
    [SerializeField] private RectTransform leftCanvas = null;
    [SerializeField] private RectTransform rightCanvas = null;

    private SkillManager_Pasivas _skillManagerPasivas;

    public bool openUI { get; private set; }


    #region Config

    void Awake()
    {
        RegistrarUIPrefabs();
    }

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    //Esto est amuy feo, tengo que ponerlo mas lindo
    void Initialize()
    {
        _charStats_Ui = Instantiate<GameObject>(UiTemplateRegistry[UI_templates.charStats], leftCanvas).GetComponent<CharStats_UI>();
        gameMenu_UI.Configure(_charStats_Ui.ToggleLvlUpSignOFF);
        stats3D_UI = Instantiate(stats3D_UI_pf, transform);
        
        
        Main.instance.GetChar().Life.frontendLife = stats3D_UI.GetComponent<Stats3D_UI_helper>().lifeBar;
        
    }

    private void RegistrarUIPrefabs()
    {
        UiTemplateRegistry.Add(UI_templates.skillSelection, skillSelection_template_pf);
        UiTemplateRegistry.Add(UI_templates.charStats, charStats_template_pf);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Para que esto funcione aca hay que cambiar el shader. El que tenemos ahora no me permite hacer la transparencia
    /// </summary>
    public void FadeOutPlayerStats()
    {
        //Harcodeada la velocidad. Hay que unificar todos los configs de la UI
        stats3D_UI.GetComponent<Stats3D_UI_helper>().FadeOut(15);
    }
    
    public void FadeInPlayerStats()
    {
        //Harcodeada la velocidad. Hay que unificar todos los configs de la UI
        stats3D_UI.GetComponent<Stats3D_UI_helper>().FadeIn(15);
    }
    public void UI_Send_NameSkillType(string s) { }
    public void UI_SendLevelUpNotification() {/* CanvasPopUpInWorld_Manager.instance.MakePopUpAnimated(Main.instance.GetChar().transform, lvlUp_pf);*/ }
    public void UI_SendActivePlusNotification(bool val) { if (val) _charStats_Ui.ToggleLvlUpSignON(); }

    public void UI_RefreshExpBar(int currentExp, int maxExp, int currentLevel) =>
        stats3D_UI.GetComponent<Stats3D_UI_helper>().expBar.OnValueChange(currentExp, maxExp); //_charStats_Ui.UpdateXP_UI(currentExp, maxExp, currentLevel);
    public void UI_MaxExpBar(int currentLevel) => _charStats_Ui.MaxLevel(currentLevel);
    public void RefreshPassiveSkills_UI(List<SkillInfo> skillsNuevas) => _charStats_Ui.UpdatePasiveSkills(skillsNuevas);
    public void SetSelectedPath(string pathName) => _charStats_Ui.SetPathChoosen(pathName);
    public void UI_RefreshMenu() => gameMenu_UI.Refresh();
    public void OpenGameMenu() => gameMenu_UI.Open();
    public void CloseGameMenu() => gameMenu_UI.Close();
    #endregion
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
