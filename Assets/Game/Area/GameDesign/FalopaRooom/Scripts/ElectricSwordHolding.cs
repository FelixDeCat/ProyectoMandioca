﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwordHolding : MonoBehaviour
{
    [Header("Fast")]
    [SerializeField] Waves _wave = null;
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;
    [SerializeField] float cooldown = 0.2f;

    [Header("Orb")]
    [SerializeField] ElectricOrb electricOrb;
    [SerializeField] int orbSpeed = 2;
    [SerializeField] float orbLifeTime = 5;

    CharacterHead myChar;
    [Header("Other")]
    [SerializeField] float charSpeed = 0;

    float timer = 0;
    bool canUpdate = false;

    public void OnPress()
    {
        //Aca supongo que van cosas de feedback
        timer = 0;
        canUpdate = true;
        Main.instance.GetChar().SwordAbiltyCharge(charSpeed);
    }
    public void OnStopUse()
    {
        //Aca tambien
        canUpdate = false;
        Main.instance.GetChar().SwordAbilityRelease();

    }
    public void OnUpdate()
    {
        if (!canUpdate) return;
        timer += Time.deltaTime;
        if(timer >= cooldown)
        {
            Spawn();
            timer = 0;
        }       
    }

    public void OnEquip()
    {
        //Sonidos?
        myChar = Main.instance.GetChar();

    }
    public void UnEquip()
    {
        //Sonidos? quiza
    }

    public void OnExecute(int charges)
    {
        if (charges == 0) ExecuteShort();
        else ExecuteLong();
    }

    void ExecuteShort()
    {
        var orb = Instantiate(electricOrb);
        orb.SetSpeed(orbSpeed).SetLifeTime(orbLifeTime);
        orb.transform.position = myChar.transform.position + Vector3.up;
        orb.transform.forward = myChar.GetCharMove().GetRotatorDirection();
    }

    void ExecuteLong()
    {
        canUpdate = true;
    }

    void Spawn()
    {
        var wave = Instantiate(_wave);
        wave = wave.SetSpeed(speed).SetLifeTime(lifeTime);
        wave.transform.position = myChar.transform.position + Vector3.up;
        wave.transform.forward = myChar.GetCharMove().GetRotatorDirection();
    }

    public void OnEnd()
    {

    }
}
