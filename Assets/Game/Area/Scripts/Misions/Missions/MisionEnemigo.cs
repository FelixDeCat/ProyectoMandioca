using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class MisionEnemigo : Mision
{
    

    public bool CoreIsActive = true;

    public override void Begin()
    {
        CoreIsActive = false;
    }

    public override void ConfigureProgresion()
    {
        progression = new int[2] { 0, 0 };
    }
    public override void SetProgresion(int[] prog)
    {
        progression = prog;
    }

    public override void End()
    {
        
    }

    public override void Failed()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void Refresh()
    {
        if (progression[0] == 0) description = "Entra al basural a buscar el primer trozo de nucleo";
        if (progression[0] == 1) description = "(Mision actualizada) vuelve al pueblo";
        if (progression[1] == 1) description = "(Mision actualizada) Vuelve a hablar con el alcalde";
    }

    public void PrimerItemAgarrado()
    {
        progression[0] = 1;
        MisionManager.instancia.CheckMision();

        subdescription = "Vuelve al pueblo";
        UI_StackMision.instancia.LogearMision(this, "Mision Actualizada", 4f);
    }

    public override void PermanentConfigurations()
    {
        
    }

    public override void CheckProgresion()
    {
        
    }
}
