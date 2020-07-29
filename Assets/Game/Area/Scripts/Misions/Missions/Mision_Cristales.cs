using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_Cristales : Mision
{
   // public Barricada barricada;
   // public ImplosionCore core;

    public override void Begin()
    {

    }
    public override void End()
    {

    }
    public override void PermanentConfigurations()
    {
        Debug.Log("Permanent Configurations");
      //  var barr = FindObjectOfType<Barricada>();
      //  var core = FindObjectOfType<ImplosionCore>();
        //if (barr != null)
        //{
        //    barr.gameObject.SetActive(!completed);
        //    core.gameObject.SetActive(!completed);
        //}
    }

    public override void ConfigureProgresion()
    {
        progression = new int[3];
    }
    public override void SetProgresion(int[] prog)
    {
        Debug.Log("SETPROGRESIONNNNN");
        progression = prog;
    }
    public void AlcaldeActivo()
    {
        if (!completed)
        {
            if (progression[0] == 0)
            {
                progression[0] = 1;
                //UI_Messages.instancia.ShowMessage("Alcalde: -Hola... Oh! tienes pocos cristales de reconstruccion. " +
                //    "Si sucede algo malo, con ellos podras reconstruirte. acércate al nucleo y recarga tus cristales", 10f);
                //FindObjectOfType<ReloadCristals>().Activate();
            }
            else if (progression[0] == 1)
            {
                if (progression[1] == 0)
                {
                    //UI_Messages.instancia.ShowMessage("Alcalde: -¿que esperas? acércate al nucleo y recarga tus cristales", 5f);
                    //FindObjectOfType<ReloadCristals>().Activate();
                }
                else
                {
                   // UI_Messages.instancia.ShowMessage("Alcalde: -Eso es todo, vuelve a tus actividades", 3f);
                    progression[2] = 1;
                }
            }
            if (progression[2] == 1)
            {
                ListoEntroACasa();

                description = "(has cargado tus cristales al maximo, ahora podras reconstruirte si llega a suceder algo malo)";
                subdescription = "";

            }

            MisionManager.instancia.CheckMision();
        }
    }

    public override void CheckProgresion()
    {
        foreach (var v in progression)
        {
            if (v == 0)
            {
                Debug.Log("El incompleto es el inidce..." + v);
                return;
            }
        }
        Debug.Log("Completed");
        completed = true;
    }

    public void ActivarBehaviourPortalCasa()
    {
        //var b = FindObjectOfType<Behaviour_EntroACasa>();
        //var portalcasa = b.GetComponent<InteractableTeleport>();
        //portalcasa.onlyBehaviour = true;
        //portalcasa.behaviours.Add(b);
    }

    public void ListoEntroACasa()
    {
        Invoke("ActivarCinematica", 1.5f);

    }

    void ActivarCinematica()
    {
       // FindObjectOfType<MaloMaloCinematica>().Iniciar();
    }

    public override void OnUpdate()
    {

    }

    public void CristalesRecargados()
    {
        if (progression[1] == 0)
        {
            progression[1] = 1;
            MisionManager.instancia.CheckMision();
            UI_StackMision.instancia.LogearMision(this, "Mision Actualizada", 4f);
        }
    }

    public override void Refresh()
    {
        if (progression[0] == 0) { /*no hago nada*/ }
        if (progression[0] == 1)
        {
            description = "acercate al nucleo y recarga tus cristales";
            subdescription = "El alcalde del pueblo te pidio que recargues tus cristales";
        }
        if (progression[1] == 1)
        {
            description = "Vuelve a hablar con el alcalde";
            subdescription = "El alcalde del pueblo te pidio que recargues tus cristales";
        }
        if (progression[2] == 1)
        {
            description = "";
            subdescription = "";
        }
    }

    public override void Failed()
    {

    }


}
