using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class EnableRender : MonoBehaviour
{
    [Header("EXECUTE IN EDIT MODE")]
    public bool execute;
    bool enable;
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
