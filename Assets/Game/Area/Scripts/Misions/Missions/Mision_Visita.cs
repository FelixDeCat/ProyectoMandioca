using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_Visita : Mision
{
    void InternalRefresh()
    {
        
    }

    public override void Begin()
    {
        
    }

    public override void ConfigureProgresion()
    {
        progression = new int[3] { 0, 0, 0 };
    }
    public override void SetProgresion(int[] prog)
    {
        progression = prog;
    }

    public override void End()
    {
        //feedback de mision terminada
    }

    public override void Failed()
    {
        //si fallo la podemos dar como terminada
        //o la podemos borrar
        //o repetir
        //o resetear la progresion
        //o hacer que suceda algo en el mundo
    }

    public override void OnUpdate()
    {
        //en el caso que la mision lo necesite
        //ejemplo: una mision que tiene un contratiempo
        //ejemplo: una mision que se mueve algun mecanismo
    }


    public override void Refresh()
    {
        
        subdescription =
        "Herrero visitado " + (progression[0] == 1 ? "<color=#00FF00>listo</color>" : "_") +
        "\nPescador visitado " + (progression[1] == 1 ? "<color=#00FF00>listo</color>" : "_") +
        "\nArmero visitado " + (progression[2] == 1 ? "<color=#00FF00>listo</color>" : "_");
    }

    public override void PermanentConfigurations()
    {
        
    }

    public override void CheckProgresion()
    {
        if (progression[0] == 1 && progression[1] == 1 && progression[2] == 1) completed = true;
    }
}
