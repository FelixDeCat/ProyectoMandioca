using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadObject : MonoBehaviour
{
    Action endload;
    [SerializeField] string prefabname;

    GameObject go;

    public void LoadObjects(Action callbackendload)
    {
        endload = callbackendload;
        go = Instantiate(Resources.Load<GameObject>(prefabname), this.transform);
        endload.Invoke();
    }

    public void Clean()
    {
        Destroy(go);
    }
}
