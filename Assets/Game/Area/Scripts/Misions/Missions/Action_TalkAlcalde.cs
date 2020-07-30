using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_TalkAlcalde : ActionRealize
{
    Mision_Cristales misioncristales;
    MisionEnemigo misionPrincipal;
    private void Awake()
    {
        misioncristales = FindObjectOfType<Mision_Cristales>();
        misionPrincipal = FindObjectOfType<MisionEnemigo>();
    }

    public void AgregarMisionSiNoLaTenia()
    {
        MisionManager.instancia.AddMision(misioncristales);
    }

    public override void Excecute()
    {
        //if (misioncristales)
        //{
        //    if (MisionManager.instancia.MisionIsActive(misioncristales))
        //    {
        //        Debug.Log("Ya la tengo");
        //        if (!misioncristales.completed) misioncristales.AlcaldeActivo();
        //    }
        //    else
        //    {
        //        if (MisionManager.instancia.MisionIsActive(misionPrincipal))
        //        {
        //            Debug.Log("NO la tengo");
        //            //UI_Messages.instancia.ShowMessage("Investiga el basural");
        //        }
        //        else
        //        {
        //            Debug.Log("NO la tengo");

        //            if (!misioncristales.completed)
        //            {
        //                Debug.Log("NO la tengo");
        //                AgregarMisionSiNoLaTenia();
        //                misioncristales.AlcaldeActivo();
        //            }
        //        }

        //    }
        //}
        //else
        //{
        //    Debug.LogError("No se encotro la mision, algo se rompio");
        //}
    }
}
