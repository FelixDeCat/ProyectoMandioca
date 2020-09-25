using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PistonOneShot : Piston
{
    [SerializeField] AnimPalanca animPalanca = null;
    [SerializeField] bool notCanComeBack = false;
    [SerializeField] AudioClip timerSound = null;
    [SerializeField] UnityEvent onEndReach = null;

    Palanca palanca;

    bool status = true;
    
    public override void Start()
    {
        AudioManager.instance.GetSoundPool(timerSound.name, AudioGroups.GAME_FX, timerSound);
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

    protected override void Update()
    {
        pingponglerp.Updatear();
    }

    public bool isStop = false;

    public void StopPiston()
    {        
        StartCoroutine(pingponglerp.stopAfter(1, animPalanca.AnimOff, changeInteractableStatus, onEndReach, notCanComeBack));
        if(!notCanComeBack) AudioManager.instance.PlaySound(timerSound.name);
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
