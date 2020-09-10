using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonOneShot : Piston
{
    [SerializeField] AnimPalanca animPalanca;
    [SerializeField] bool notCanComeBack;

    Palanca palanca;

    bool status = true;
    
    public override void Start()
    {
        palanca = animPalanca.GetComponent<Palanca>();
        palanca.SetPredicate(currStatus);
        if (Anim)
        {
            pingponglerp = new PingPongLerp();
            pingponglerp.Configure(AnimationResult, true, true, stay_position_time);

            pingponglerp.ConfigureSpeedsMovements(speed_go_multiplier, speed_back_multiplier);
            pingponglerp.ConfigueTimeStopsSides(staypositiontime_go, staypositiontime_back);
        }
    }
    protected override void Update() {
        pingponglerp.Updatear();
    }

    public bool isStop = false;

    public void StopPiston()
    {        
        StartCoroutine(pingponglerp.stopAfter(1, animPalanca.AnimOff, changeInteractableStatus, notCanComeBack));
    }

    bool changeInteractableStatus(bool stat)
    {
        status = stat;
        return status;
    } 
    bool currStatus()
    {
        return status;
    }

    public void StartPistonInf()
    {
        pingponglerp.Play(1);
    }
}
