using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinematic : MonoBehaviour
{
    [SerializeField] Transform targetPos = null;
    [SerializeField] Transform lookAt = null;

    [SerializeField] float goTime = 5;
    [SerializeField] float cinematicTime = 7;
    [SerializeField] float returnTime = 5;

    bool alreadyActivate;
    public void StartCinematic()
    {
        if (alreadyActivate) return;

        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, targetPos, lookAt);
        alreadyActivate = true;
    }

    public void StartCinematic(Action OnFinishCinematic_Callback)
    {
        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, targetPos, lookAt, OnFinishCinematic_Callback);
    }

}
