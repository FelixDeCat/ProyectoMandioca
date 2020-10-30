using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jacinta : Villager
{
    public PointToGo pos_techo_hermana;
    public PointToGo pos_habla_melemaco;
    public PointToGo pos_esquinita;
    public PointToGo pos_doctor;

    protected override void OnInitialize() {  }

    public void GoToPos_TechoHermana() => GoTo(pos_techo_hermana.transform.position);
    public void GoToPos_HablaMelemaco() => GoTo(pos_habla_melemaco.transform.position);
    public void GoToPos_Esquinita() => GoTo(pos_esquinita.transform.position);
    public void GoToPos_Doctor() => GoTo(pos_doctor.transform.position);

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
