﻿using System.Collections;
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

    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;

    Vector3 downPos;
    Vector3 upPos;

    float timerToDesactive;
    float movingTimer;
    bool active;

    public Action LakeUp;

    public override void Initialize()
    {
        base.Initialize();
        lake.gameObject.SetActive(true);
        lake.GetComponent<PlayObject>()?.Initialize();
        lake.localPosition = new Vector3(lake.localPosition.x, minLakeYPos, lake.localPosition.z);
        lake.gameObject.SetActive(false);
        animEvent.Add_Callback("PoisonLake", ActiveLake);
    }

    protected override void OnUseSkill()
    {
        anim.Play("StartOrb");
        anim.SetBool("PoisonLake", true);
    }

    void ActiveLake()
    {
        lake.gameObject.SetActive(true);
        lake.GetComponent<PlayObject>()?.On();
        downPos = new Vector3(lake.localPosition.x, minLakeYPos, lake.localPosition.z);
        upPos = new Vector3(lake.localPosition.x, maxLakeYPos, lake.localPosition.z);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!lake.gameObject.activeSelf) return;

        if (!active)
        {
            movingTimer += Time.deltaTime;

            lake.localPosition = Vector3.Lerp(downPos, upPos, movingTimer / timeToUp);

            if(movingTimer >= timeToUp)
            {
                lake.localPosition = upPos;
                movingTimer = 0;
                active = true;
                LakeUp?.Invoke();
                anim.SetBool("PoisonLake", false);
            }  
        }
        else
        {
            timerToDesactive += Time.deltaTime;

            if (timerToDesactive >= timeActive)
            {
                movingTimer += Time.deltaTime;

                lake.localPosition = Vector3.Lerp(upPos, downPos, movingTimer / timeToUp);

                if (movingTimer >= timeToUp)
                {
                    OverSkill();
                }
            }
        }
    }

    protected override void OnInterruptSkill()
    {
        lake.localPosition = downPos;
    }

    protected override void OnOverSkill()
    {
        lake.localPosition = downPos;
        lake.GetComponent<PlayObject>()?.Off();
        lake.gameObject.SetActive(false);
        movingTimer = 0;
        active = false;
        timerToDesactive = 0;
    }
}
