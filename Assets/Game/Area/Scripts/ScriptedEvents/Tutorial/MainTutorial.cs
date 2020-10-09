using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTutorial : MonoBehaviour
{
    public const int ID_TUTORIAL = 4;
    public const int FASE_LIBERA_CAMINO_JACINTA = 0;
    public const int FASE_ACTIVO_DOCTOR = 1;
    public const int FASE_PLANTAS_ENTREGADAS = 2;
    public const int FASE_ATENEA_DEJA_PASAR = 3;

    public ObjetiveSubscriber jacinta_ents_Objetive;
    public ObjetiveSubscriber farm_armored_ent_Objetive;
    public int Plants_Recollection_ID;
    bool doc_req_armored_ent_killed = false;
    bool doc_req_plants_mision_ended = false;

    public void TRIGGER_StartTutorial()
    {
        ChangeFase(FASE_LIBERA_CAMINO_JACINTA);
        jacinta_ents_Objetive.BeginObjetive(All_Jacinta_Ents_Killed);
    }

    void All_Jacinta_Ents_Killed()
    {
        ChangeFase(FASE_ACTIVO_DOCTOR);
        farm_armored_ent_Objetive.BeginObjetive(Armored_Ent_Killed);
        MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(Plants_Recollection_ID), All_Curative_Plants_Recollected);
    }

    void All_Curative_Plants_Recollected(int ID)
    {
        doc_req_plants_mision_ended = true;
        DoctorRequirements();
    }
    void Armored_Ent_Killed()
    {
        doc_req_armored_ent_killed = true;
        DoctorRequirements();
    }

    void DoctorRequirements()
    {
        if (doc_req_armored_ent_killed && doc_req_plants_mision_ended)
        {
            ChangeFase(FASE_PLANTAS_ENTREGADAS);
        }
    }


    #region auxs
    void ChangeFase(int val) =>  ManagerGlobalFases.instance.ModifyFase(ID_TUTORIAL, val);
    #endregion
}
