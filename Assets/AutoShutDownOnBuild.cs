﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoShutDownOnBuild : MonoBehaviour
{

    public UnityEvent OnEnable;
    public UnityEvent OnDisable;
    bool en;

    void Start()
    {
#if UNITY_STANDALONE
        OnDisable.Invoke();
        en = false;
#endif
#if UNITY_EDITOR
        OnEnable.Invoke();
        en = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.K))
        {
            en = !en;
            if (en) OnEnable.Invoke();
            else OnDisable.Invoke();
        }
    }
}
