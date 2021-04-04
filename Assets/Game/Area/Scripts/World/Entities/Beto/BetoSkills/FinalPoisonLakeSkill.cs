using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinalPoisonLakeSkill : BossSkills
{
    [SerializeField] float minLakeYPos = 0;
    [SerializeField] float maxLakeYPos = 5;

    [SerializeField] float timeToUp = 5;
    [SerializeField] float timeToDown = 5;

    [SerializeField] float timeActive = 20;

    [SerializeField] Transform lake = null;

    Vector3 downPos;
    Vector3 upPos;

    float timerToDesactive;
    float movingTimer;
    bool active;

    Action LakeUp;

    public override void Initialize()
    {
        base.Initialize();
        lake.gameObject.SetActive(true);
        lake.GetComponent<PlayObject>()?.Initialize();
        lake.gameObject.SetActive(false);
    }

    protected override void OnUseSkill()
    {
        lake.gameObject.SetActive(true);
        lake.GetComponent<PlayObject>()?.On();
        downPos = new Vector3(lake.position.x, minLakeYPos, lake.position.z);
        upPos = new Vector3(lake.position.x, maxLakeYPos, lake.position.z);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!active)
        {
            movingTimer += Time.deltaTime;

            lake.position = Vector3.Lerp(downPos, upPos, movingTimer / timeToUp);

            if(movingTimer >= timeToUp)
            {
                lake.position = upPos;
                movingTimer = 0;
                active = true;
                LakeUp?.Invoke();
            }  
        }
        else
        {
            timerToDesactive += Time.deltaTime;

            if (timerToDesactive >= timeActive)
            {
                movingTimer += Time.deltaTime;

                lake.position = Vector3.Lerp(upPos, downPos, movingTimer / timeToUp);

                if (movingTimer >= timeToUp)
                {
                    OverSkill();
                }
            }
        }
    }

    protected override void OnInterruptSkill()
    {
        lake.position = downPos;
    }

    protected override void OnOverSkill()
    {
        lake.position = downPos;
        lake.GetComponent<PlayObject>()?.Off();
        lake.gameObject.SetActive(false);
        movingTimer = 0;
        active = false;
        timerToDesactive = 0;
    }
}
