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

    [SerializeField] AnimationCurve moveSmooth = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [SerializeField] AnimationCurve lookAtSmooth = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });


    bool alreadyActivate;

    public void SetLookAtPos(Vector3 pos, Vector3 forward) 
    {
        lookAt.transform.position = pos;
        lookAt.transform.forward = forward;
    }

    public void StartCinematic()
    {
        if (alreadyActivate) return;

        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, moveSmooth, lookAtSmooth, targetPos, lookAt);
        alreadyActivate = true;
    }

    public void StartCinematic(Action OnFinishCinematic_Callback)
    {
        if (alreadyActivate) return;
        Main.instance.GetMyCamera().StartCinematic(goTime, cinematicTime, returnTime, moveSmooth, lookAtSmooth, targetPos, lookAt, OnFinishCinematic_Callback);
        alreadyActivate = true;
    }

    public void CinematicOver()
    {
        Main.instance.GetMyCamera().CinematicOver();
    }
    public void CinematicInstantOver()
    {
        Main.instance.GetMyCamera().CinematicInstantOver();
    }

    public void CinematicInstant(Action OnFinishCinematic_Callback)
    {
        if (alreadyActivate) return;
        Main.instance.GetMyCamera().InstantCinematic(goTime, cinematicTime, returnTime, moveSmooth, lookAtSmooth, targetPos, lookAt, OnFinishCinematic_Callback);
        alreadyActivate = true;
    }
}
