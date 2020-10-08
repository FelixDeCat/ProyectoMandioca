using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class ElectricSword : MonoBehaviour
{
    [Header ("Fast")]
    [SerializeField] Waves _wave = null;
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;

    [Header("Orb")]
    [SerializeField] GameObject electricOrb;

    CharacterHead myChar;

    public void OnPress()
    {
        //Aca supongo que van cosas de feedback

    }
    public void OnStopUse()
    {
        //Aca tambien

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
        Debug.Log("ExecuteShort");
        var wave = Instantiate(_wave);
        wave.transform.position = myChar.transform.position;
        wave.transform.forward = myChar.GetCharMove().GetRotatorDirection();
        wave = wave.SetSpeed(speed).SetLifeTime(lifeTime);
    }

    void ExecuteLong()
    {
        Debug.Log("ExecuteLong");
        var orb = Instantiate(electricOrb);
        orb.transform.position = myChar.transform.position;
        orb.transform.forward = myChar.transform.forward;
    }

    public void OnEnd()
    {

    }
}
