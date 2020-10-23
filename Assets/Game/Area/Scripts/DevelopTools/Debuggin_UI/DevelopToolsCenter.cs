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
    
    private void Start()
    {
        ToogleDebug(false);
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Dummy Enemy State Machine Debug", false, ToogleDebug);
       // DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Range Enemy State Machine Debug", false, ToogleDebugRange);
        //DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Jabalies State Machine Debug", false, ToogleDebugJabali);

        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Cubitos Render", false, CubitosRender);

        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Armas", false, Armas);
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("NormalAttack", true, GodDamage);

        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Magnificar Ents", false, ToogleMagnoFace);
        
    }

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

}
