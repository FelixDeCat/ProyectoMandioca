using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class ElectricSword : MonoBehaviour
{
    [Header ("Fast")]
    [SerializeField] Waves electricBullet = null;
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;

    [Header("Orb")]
    [SerializeField] ElectricOrb electricOrb = null;
    [SerializeField] int orbSpeed = 5;
    [SerializeField] float orbLifeTime = 2;

    [Header("Other")]
    [SerializeField] float charSpeed = 0;
    CharacterHead myChar;

    public void OnPress()
    {
        //Aca supongo que van cosas de feedback
        Main.instance.GetChar().SwordAbiltyCharge(charSpeed);

    }
    public void OnStopUse()
    {
        //Aca tambien
        Main.instance.GetChar().SwordAbilityRelease();

    }
    public void OnUpdate()
    {

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
        var wave = Instantiate(electricBullet);
        wave.transform.position = myChar.transform.position + Vector3.up; ;
        wave.transform.forward = myChar.GetCharMove().GetRotatorDirection();
        wave = wave.SetSpeed(speed).SetLifeTime(lifeTime);
    }

    void ExecuteLong()
    {
        ElectricOrb orb = Instantiate(electricOrb);
        orb.transform.position = myChar.transform.position + Vector3.up;
        orb.transform.forward = myChar.GetCharMove().GetRotatorDirection();
        orb.SetSpeed(orbSpeed).SetLifeTime(orbLifeTime);
    }

    public void OnEnd()
    {

    }
}
