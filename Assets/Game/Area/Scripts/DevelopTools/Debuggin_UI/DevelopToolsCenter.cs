using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;
using Tools;
using System.Linq;

public class DevelopToolsCenter : MonoBehaviour
{
    public static DevelopToolsCenter instance; private void Awake() => instance = this;

    bool open_wrench = false;
    bool open_debugView = false;

    [SerializeField] GameObject[] childs = new GameObject[0];

    CanvasGroup canvasGroup;


    #region God Mode
    bool godMode = false;
    public bool GodMode { get { return godMode; } }
    public void SetGodMode(bool val) 
    { 
        godMode = val;
        Main.instance.GetChar().SetGodMode(true);
    }
    #endregion

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        ToogleDebug(false);
        

        Debug_UI_Tools.instance.CreateToogle("GODMODE", false, ToogleGodMOdeDebug);
        Debug_UI_Tools.instance.CreateToogle("Armas", false, Armas);

       // Debug_UI_Tools.instance.CreateToogle("Dummy Enemy State Machine Debug", false, ToogleDebug);

      // Debug_UI_Tools.instance.CreateToogle("Cubitos Render", false, CubitosRender);

        
       // Debug_UI_Tools.instance.CreateToogle("NormalAttack", true, GodDamage);

        Debug_UI_Tools.instance.CreateToogle("Magnificar Ents", false, ToogleMagnoFace);
        

    }

    public void On_Debug() { canvasGroup.alpha = 1; canvasGroup.interactable = true; }
    public void Off_Debug() { canvasGroup.alpha = 0; canvasGroup.interactable = false; }

    #region Toggles [GODMODE]
    string ToogleGodMOdeDebug(bool active)
    {

        SetGodMode(active);
        return active ? "debug activado" : "debug desactivado";
    }
    #endregion

    public string ToogleMagnoFace(bool val)
    {
        FindObjectsOfType<TrueDummyEnemy>().ToList().ForEach(x => x.GetComponentInChildren<MagnoSuperCheat>().ToogleMagnoFace(val));

        if (val)
        {
            return "Ents Magnificados";
        }
        else
        {
            return "Ents Normales";
        }
    }

    public string Armas(bool val)
    {
        Main.instance.GetChar().ToggleShield(val);
        Main.instance.GetChar().ToggleSword(val);
        return "C:=> " + (val ? "ON" : "OFF");
    }
    string GodDamage(bool val)
    {
        var c = Main.instance.GetChar();

        if (!val)
        {
            c.GetCharacterAttack().ChangeDamageBase(999);
        }
        else
        {
            c.GetCharacterAttack().ChangeDamageBase(20);
        }

        return "C:=> " + (val ? "normal" : "Divine Force");
    }

    string CubitosRender(bool val)
    {
        var cubits = FindObjectsOfType<EnableCubitos>();
        foreach(var c in cubits) c.EnableCubitosBoool(val);
        return "C:=> " + (val ? "ON" : "OFF");
    }
    public void UIBUTTON_WrenchDebug()
    {
        Debug.Log("se abreeeeee");
        open_wrench = !open_wrench;
        Debug_UI_Tools.instance.Toggle(open_wrench);
    }
    public void UIBUTTON_DebugViewer()
    {
        open_debugView = !open_debugView;
        if(open_debugView) DebugVisual.instance.BUTTON_Show();
        else DebugVisual.instance.BUTTON_Hide();

    }

    void Configurations()
    {
        
    }
    bool enemydebug;
    public bool EnemyDebuggingIsActive() { return enemydebug; }
    string ToogleDebug(bool active) { enemydebug = active; FindObjectsOfType<TrueDummyEnemy>().ToList().ForEach(x => x.ToogleDebug(active)); return active ? "debug activado" : "debug desactivado"; }
   // string ToogleDebugRange(bool active) { enemydebug = active; FindObjectsOfType<RangeDummy>().ToList().ForEach(x => x.debug_options.ToogleDebug(active)); return active ? "debug activado" : "debug desactivado"; }
    //string ToogleDebugJabali(bool active) { enemydebug = active; FindObjectsOfType<JabaliEnemy>().ToList().ForEach(x => x.ToogleDebug(active)); return active ? "debug activado" : "debug desactivado"; }

    public void ShowChilds(bool b)
    {
        for (int i = 0; i < childs.Length; i++)
            childs[i].SetActive(b);
    }

}
