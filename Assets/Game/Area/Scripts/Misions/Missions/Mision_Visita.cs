using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_Visita : Mision
{
    void InternalRefresh()
    {
        
    }

    protected override void OnBegin()
    {
        
    }

    public override void ConfigureProgresion()
    {
       // progression = new int[3] { 0, 0, 0 };
    }
    public override void SetProgresion(int[] prog)
    {
       // progression = prog;
    }

    protected override void OnEnd()
    {
        //feedback de mision terminada
    }

    public override void OnFailed()
    {
        //si fallo la podemos dar como terminada
        //o la podemos borrar
        //o repetir
        //o resetear la progresion
        //o hacer que suceda algo en el mundo
    }

    protected override void OnUpdate()
    {
        //en el caso que la mision lo necesite
        //ejemplo: una mision que tiene un contratiempo
        //ejemplo: una mision que se mueve algun mecanismo
    }


    public override void Refresh()
    {
        
        //subdescription =
        //"Herrero visitado " + (progression[0] == 1 ? "<color=#00FF00>listo</color>" : "_") +
        //"\nPescador visitado " + (progression[1] == 1 ? "<color=#00FF00>listo</color>" : "_") +
        //"\nArmero visitado " + (progression[2] == 1 ? "<color=#00FF00>listo</color>" : "_");
    }

    public override void PermanentConfigurations()
    {
        
    }

    public override void CheckProgresion()
    {
       // if (progression[0] == 1 && progression[1] == 1 && progression[2] == 1) completed = true;
    }
}
