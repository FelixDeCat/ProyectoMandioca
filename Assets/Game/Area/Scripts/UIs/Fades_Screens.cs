using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Fades_Screens : MonoBehaviour
{
    public static Fades_Screens instance;
    [SerializeField] float speed;
    [SerializeField] CanvasGroup canvas_group;
    float timer;
    public bool on;
    Action EndOff = delegate { };
    Action EndOn = delegate { };
    public bool Anim=true;
    [SerializeField] bool startOn;
    private void Awake() { instance = this; if(startOn) canvas_group.alpha = 1; }
    public void Black() { canvas_group.alpha = 1; }
    public void Transparent() { canvas_group.alpha = 0; }
    public void FadeOn(Action FadeOnEndCallback)
    {
        EndOn = FadeOnEndCallback;
        timer = 0;
        on = true;
        Anim = true;
    }
    public void FadeOff(Action FadeOffEndCallback)
    {
        EndOff = FadeOffEndCallback;
        timer = 1;
        on = false;
        Anim = true;
    }
    void Update()
    {
        if (Anim)
        {
            if (on)
            {
                if (timer < 1)
                {
                    timer = timer + 1 * Time.deltaTime * speed;
                    canvas_group.alpha = timer;
                }
                else
                {
                    timer = 1;
                    Anim = false;
                    EndOn();
                }
            }
            else
            {
                if (timer > 0)
                {
                    timer = timer - 1 * Time.deltaTime * speed;
                    canvas_group.alpha = timer;
                }
                else
                {
                    timer = 0;
                    Anim = false;
                    EndOff();
                }
            }
        }
    }
}