using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_Movement : Mision
{

    enum fases { fase1, fase2 }
    fases _fases;

   // public MiCasa micasa;

    protected override void OnBegin()
    {
        _fases = fases.fase1;

       // UI_Messages.instancia.ShowMessage("sal afuera", 2f);
    }

    protected override void OnEnd()
    {

    }

    public override void Refresh()
    {
       //// micasa = FindObjectOfType<MiCasa>();

       // if (_fases == fases.fase1)
       // {
       //     subdescription =
       //     "arriba " + (progression[0] == 1 ? "<color=#00FF00>listo</color>" : "_") +
       //     "\nabajo " + (progression[1] == 1 ? "<color=#00FF00>listo</color>" : "_") +
       //     "\nizquierda " + (progression[2] == 1 ? "<color=#00FF00>listo</color>" : "_") +
       //     "\nderecha " + (progression[3] == 1 ? "<color=#00FF00>listo</color>" : "_");

       //    // if (micasa) micasa.TutorialHouse();
       // }
       // else if (_fases == fases.fase2)
       // {
            
       //     //if (micasa) micasa.Fase2terminada();
       //     description = "comprueba que tu sistema de recoleccion sigue funcionando";
       //     subdescription = "agarra el palo y equipalo";
       //     UI_StackMision.instancia.LogearMision(this, "Mision Actualizada", 4f);
       // }
    }

    public override void ConfigureProgresion()
    {
        //progression = new int[4];
    }
    public override void SetProgresion(int[] prog)
    {
        //progression = prog;
        //if (progression[0] == 1 &&
        //    progression[1] == 1 &&
        //    progression[2] == 1 &&
        //    progression[3] == 1)
        //{
        //    _fases = fases.fase2;
        //}
    }

    bool oneshot;

    public void PaloEquipado()
    {
        //if (!completed)
        //{
        //    completed = true;

        //  //  micasa = FindObjectOfType<MiCasa>();
        //  //  if (micasa) micasa.TutorialTerminado();

        //    MisionManager.instancia.CheckMision();
        //}
    }

    protected override void OnUpdate()
    {
        //if (_fases == fases.fase1)
        //{
        //    if (Input.GetAxis("MoveVertical") > 0.5f) progression[0] = 1;
        //    if (Input.GetAxis("MoveVertical") < -0.5f) progression[1] = 1;
        //    if (Input.GetAxis("MoveHorizontal") < -0.5f) progression[2] = 1;
        //    if (Input.GetAxis("MoveHorizontal") > 0.5f) progression[3] = 1;


        //    oneshot = true;

        //    if (oneshot)
        //    {
        //        oneshot = false;
        //        MisionManager.instancia.CheckMision();
        //    }
        //    if (
        //        progression[0] == 1 &&
        //        progression[1] == 1 &&
        //        progression[2] == 1 &&
        //        progression[3] == 1)
        //    {
        //        _fases = fases.fase2;
        //        MisionManager.instancia.CheckMision();
        //    }
        //}
        //else if (_fases == fases.fase2)
        //{

        //}

    }

    public override void OnFailed()
    {

    }

    public override void PermanentConfigurations()
    {
        
    }

    public override void CheckProgresion()
    {
        
    }
}
