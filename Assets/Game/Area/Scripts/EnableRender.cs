using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DevelopTools.UI;

[ExecuteInEditMode]
public class EnableRender : MonoBehaviour
{
    [Header("EXECUTE IN EDIT MODE")]
    public bool execute;
    bool enable;


    private void InitializeMeshToogle()
    {
        Debug_UI_Tools.instance.CreateToogle(this.gameObject.name, false, Toogle);
    }

    public string Toogle(bool val)
    {
        string s = val ? "prendido" : "apagado";
        enable = val;
        GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.enabled = enable);
        return s;
    }
    

    void Update()
    {
        if (execute)
        {
            execute = false;
            enable = !enable;
            GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.enabled = enable);
        }
    }
}
