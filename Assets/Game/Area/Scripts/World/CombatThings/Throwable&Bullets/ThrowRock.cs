using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : Throwable
{
    [SerializeField] AudioClip _crushBoulder = null;

    public float time_to_disapear = 3f;
    float timerDisapear;
    bool canDisapear;

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.GetSoundPool("boulder Crush", AudioGroups.GAME_FX, _crushBoulder);
    }

    protected override void InternalThrow()
    {
        myrig.AddForce(savethrowdata.Direction * savethrowdata.Force, ForceMode.VelocityChange);
    }

    protected override void InternalParry()
    {
        AudioManager.instance.PlaySound("boulder Crush");
        timerDisapear = 0;
        myrig.AddForce(transform.forward * savethrowdata.Force * 2, ForceMode.VelocityChange);
    }

    protected override void Update()
    {
        base.Update();

        if (canDisapear)
        {
            if (timerDisapear < time_to_disapear)
            {
                timerDisapear = timerDisapear + 1 * Time.deltaTime;
            }
            else
            {
                timerDisapear = 0;
                canDisapear = false;
                Dissappear();
            }
        }
    }

    protected override void OnFloorCollision()
    {
        canDisapear = true;
    }

    protected override void NonParry()
    {
        base.NonParry();
        canDisapear = true;
    }
}
