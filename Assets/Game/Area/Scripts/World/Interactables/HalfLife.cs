using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfLife : Interactable
{
    //bool execute;
    //float timer;

    public int mycant;
    public GenericBar genericbar;

    new public Light light;

    void Start()
    {
        genericbar.Configure(0, mycant, 0.01f);
        light = GetComponentInChildren<Light>();
    }

    public override void OnExecute(WalkingEntity entity)
    {
        //execute = true;
    }
    public override void OnExit()
    {
        //execute = false;
    }
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(this, "centro de carga", "contiene en total " + mycant + " Cargas", "Cargar");

    }

    public void MensajeTodoCargado()
    {
        
    }
}
