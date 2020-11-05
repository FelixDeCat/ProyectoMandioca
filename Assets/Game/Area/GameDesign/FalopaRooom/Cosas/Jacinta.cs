using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jacinta : Villager
{
    [Header("Jacinta Variables")]
    public PointToGo pos_techo_hermana;
    public PointToGo pos_habla_melemaco;
    public PointToGo pos_esquinita;
    public PointToGo pos_doctor;
    public UnityEvent Jacinta_Arrive_Techo;
    public UnityEvent Jacinta_Arrive_AbajoEscalera;
    public UnityEvent Jacinta_Arrive_Esquinita;
    public UnityEvent Jacinta_Arrive_Doctor;

    protected override void OnInitialize() { }

    public void GoToPos_TechoHermana() => GoToCustomAnim(pos_techo_hermana.transform.position, "RunNormal", Jacinta_Arrive_Techo.Invoke);
    public void GoToPos_HablaMelemaco() => GoTo(pos_habla_melemaco.transform.position, Jacinta_Arrive_AbajoEscalera.Invoke);
    public void GoToPos_Esquinita() => GoToCustomAnim(pos_esquinita.transform.position, "RunNormal", Jacinta_Arrive_Esquinita.Invoke);
    public void GoToPos_Doctor() => GoToCustomAnim(pos_doctor.transform.position, "RunNormal", Jacinta_Arrive_Doctor.Invoke);

    public void ExecuteAnimationCustom(string name)
    {
        anim.PlayAnimation(name);
    }

    public void ChangeSpeed(float newSpeed)
    {
        rig_path_finder.movement_speed = newSpeed;
    }

    #region En desuso
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity()
    {
        
    }
    #endregion
}
