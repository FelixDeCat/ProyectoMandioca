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
    [SerializeField] private GameObject stats3D_UI_pf;
    [SerializeField] private UI3D_Shields_controller _shieldsController_pf;

    public RectTransform CompleteParentInstancer { get => completeCanvas; }
    
    
    [SerializeField] private GameMenu_UI gameMenu_UI = null;
    [HideInInspector] public GameObject stats3D_UI;
    private CharStats_UI _charStats_Ui;
    //private UI3D_Shields_controller _shieldsController;
    
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
    
    
    
    
    //Seguroi esto se deje de usar
    Dictionary<UI_templates, GameObject> UiTemplateRegistry = new Dictionary<UI_templates, GameObject>();
    

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

        //_shieldsController = Instantiate(_shieldsController_pf, transform);
        //_shieldsController.Init(3,3);
        
        Main.instance.GetChar().Life.frontendLife = stats3D_UI.GetComponent<Stats3D_UI_helper>().littleHeart;
        
    }

    private void RegistrarUIPrefabs()
    {
        UiTemplateRegistry.Add(UI_templates.skillSelection, skillSelection_template_pf);
        UiTemplateRegistry.Add(UI_templates.charStats, charStats_template_pf);
    }

    #endregion

    #region Public methods
    
    //public void HideShields() => _shieldsController.HideShields();
    //public void ShowShields() => _shieldsController.ShowShields();
    //public void RefreshShields_UI(int currentShields, int maxShields) =>  _shieldsController.RefreshUI(currentShields, maxShields);
    public RectTransform GetRectCanvas() => completeCanvas;
    
    /// <summary>
    /// Para que esto funcione aca hay que cambiar el shader. El que tenemos ahora no me permite hacer la transparencia
    /// </summary>
    /// //Harcodeada la velocidad. Hay que unificar todos los configs de la UI
    public void FadeOutPlayerStats(){stats3D_UI.GetComponent<Stats3D_UI_helper>().FadeOut(15);}
    //Harcodeada la velocidad. Hay que unificar todos los configs de la UI
    public void FadeInPlayerStats(){stats3D_UI.GetComponent<Stats3D_UI_helper>().FadeIn(15);}
    public void UI_Send_NameSkillType(string s) { }
    public void UI_SendLevelUpNotification() {/* CanvasPopUpInWorld_Manager.instance.MakePopUpAnimated(Main.instance.GetChar().transform, lvlUp_pf);*/ }
    public void UI_SendActivePlusNotification(bool val) { if (val) _charStats_Ui.ToggleLvlUpSignON(); }
    public void RefreshPassiveSkills_UI(List<SkillInfo> skillsNuevas) => _charStats_Ui.UpdatePasiveSkills(skillsNuevas);
    public void SetSelectedPath(string pathName) => _charStats_Ui.SetPathChoosen(pathName);
    public void UI_RefreshMenu() => gameMenu_UI.Refresh();
    public void OpenGameMenu() => gameMenu_UI.Open();
    public void CloseGameMenu() => gameMenu_UI.Close();
    #endregion

    public void OnChangeLife(int current, int max)
    {
        stats3D_UI.GetComponent<Stats3D_UI_helper>().littleHeart.OnValueChange(current, max);
        stats3D_UI.GetComponent<Stats3D_UI_helper>().yellowHeart.OnValueChangeWithDelay(current, 1,max); //el 0 esta mal, dsp lo coambio
    }

    public void ResetYellowHeart()
    {
        stats3D_UI.GetComponent<Stats3D_UI_helper>().yellowHeart.OnValueChange(1,1);
    }
    
    
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
