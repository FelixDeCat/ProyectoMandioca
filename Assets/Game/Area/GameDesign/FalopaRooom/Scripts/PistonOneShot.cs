using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonOneShot : Piston
{
    [SerializeField] AnimPalanca animPalanca;

    public override void Start()
    {
        if (Anim)
        {
            pingponglerp = new PingPongLerp();
            pingponglerp.Configure(AnimationResult, true, true, stay_position_time);

            pingponglerp.ConfigureSpeedsMovements(speed_go_multiplier, speed_back_multiplier);
            pingponglerp.ConfigueTimeStopsSides(staypositiontime_go, staypositiontime_back);
        }
    }
    protected override void Update() { }

    public bool isStop = false;

    public void StopPiston()
    {
        //if (isStop) { pingponglerp.Play(1); }
        //else pingponglerp.Stop();
        //isStop = !isStop;

        StartCoroutine(pingponglerp.stopAfter(1, animPalanca.AnimOff));
    }
}
