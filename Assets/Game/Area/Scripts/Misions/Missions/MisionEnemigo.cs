using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class MisionEnemigo : Mision
{
    
    public bool CoreIsActive = true;

    protected override void OnBegin()
    {
        CoreIsActive = false;
    }

    public override void ConfigureProgresion()
    {
       // progression = new int[2] { 0, 0 };
    }
    public override void SetProgresion(int[] prog)
    {
       // progression = prog;
    }

    protected override void OnEnd()
    {
        
    }

    public override void OnFailed()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    public override void Refresh()
    {
        //if (progression[0] == 0) description = "Entra al basural a buscar el primer trozo de nucleo";
        //if (progression[0] == 1) description = "(Mision actualizada) vuelve al pueblo";
        //if (progression[1] == 1) description = "(Mision actualizada) Vuelve a hablar con el alcalde";
    }

    public void PrimerItemAgarrado()
    {
        //progression[0] = 1;
        //MisionManager.instancia.CheckMision();

        //subdescription = "Vuelve al pueblo";
        //UI_StackMision.instancia.LogearMision(this, "Mision Actualizada", 4f);
    }

    public override void PermanentConfigurations()
    {
        
    }

    public override void CheckProgresion()
    {
        
    }
}
