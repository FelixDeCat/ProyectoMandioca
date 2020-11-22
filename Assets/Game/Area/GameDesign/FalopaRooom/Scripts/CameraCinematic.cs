using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinematic : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] Transform lookAt;

    [SerializeField] float goTime;
    [SerializeField] float cinematicTime;
    [SerializeField] float returnTime;

    public void StartCinematic()
    {
        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, targetPos, lookAt);
    }

    public void StartCinematic(Action OnFinishCinematic_Callback)
    {
        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, targetPos, lookAt, OnFinishCinematic_Callback);
    }
}
