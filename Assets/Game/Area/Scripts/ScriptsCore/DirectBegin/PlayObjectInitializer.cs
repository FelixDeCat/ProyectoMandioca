using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayObjectInitializer : MonoBehaviour
{
    void Start()
    {
        //esto es lo que lo hace... pero estaria copado que lo haga la grilla
        //total Initialize tiene adentro un AlreadyInitialize para que no repita
        FindObjectsOfType<PlayObject>().ToList().ForEach(x => { x.Initialize(); x.On(); });
    }
}
