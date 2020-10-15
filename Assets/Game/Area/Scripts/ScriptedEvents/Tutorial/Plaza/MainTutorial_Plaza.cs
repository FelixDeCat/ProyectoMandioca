using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTutorial_Plaza : MonoBehaviour
{
    public const int ID_TUTORIAL = 5;
    public const int FASE_INICIO = 0;
    public const int FASE_CAMINO_LIBERADO_PARA_ENTUSIASTA = 1;

    public ObjetiveSubscriber cuervo_asesinado_Objetive;

    public void TRIGGER_StartTutorial()
    {
        ChangeFase(FASE_INICIO);
        cuervo_asesinado_Objetive.BeginObjetive(FinishObjetive_CuervoAsesinado);
    }

    void FinishObjetive_CuervoAsesinado() 
    {
        ChangeFase(FASE_CAMINO_LIBERADO_PARA_ENTUSIASTA);
    }

    #region auxs
    void ChangeFase(int val) =>  ManagerGlobalFases.instance.ModifyFase(ID_TUTORIAL, val);
    #endregion
}
