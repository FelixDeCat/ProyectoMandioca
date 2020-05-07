using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;
using ProyectTools;
using System.Linq;

public class DevelopToolsCenter : MonoBehaviour
{
    public static DevelopToolsCenter instance; private void Awake() => instance = this;

    bool open = false;
    
    private void Start()
    {
        ToogleDebug(false);
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Dummy Enemy State Machine Debug", false, ToogleDebug);
    }
    public void UIBUTTON_WrenchDebug()
    {
        Debug.Log("se abreeeeee");
        open = !open;
        Debug_UI_Tools.instance.Toggle(open);
    }    

    void Configurations()
    {
        
    }
    bool enemydebug;
    public bool EnemyDebuggingIsActive() { return enemydebug; }
    string ToogleDebug(bool active) { enemydebug = active; FindObjectsOfType<TrueDummyEnemy>().ToList().ForEach(x => x.ToogleDebug(active)); return active ? "debug activado" : "debug desactivado"; }

}
