using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTutorial : MonoBehaviour
{
    public const int ID_TUTORIAL = 4;
    public const int FASE_LIBERA_CAMINO_JACINTA = 0;
    public const int FASE_CAMINO_A_DOCTOR_DESPEJADO = 1;
    public const int FASE_ACTIVO_DOCTOR = 2;
    public const int FASE_PLANTAS_ENTREGADAS = 3;
    public const int FASE_ATENEA_DEJA_PASAR = 4;
    public const int FASE_ARMORED_ENT_ASESINATADO = 5;

    public ObjetiveSubscriber jacinta_ents_Objetive;
    public ObjetiveSubscriber farm_armored_ent_Objetive;
    public ObjetiveSubscriber doctorway_ents_Objetive;
    public int Plants_Recollection_ID;
    bool objetive_armored_ent = false;
    bool objetive_plants = false;

    public void TRIGGER_StartTutorial()
    {
        ChangeFase(FASE_LIBERA_CAMINO_JACINTA);

        jacinta_ents_Objetive.BeginObjetive(FinishObjetive_All_Jacinta_Ents_Killed);
        farm_armored_ent_Objetive.BeginObjetive(FinishObjetive_Armored_Ent_Killed);
        doctorway_ents_Objetive.BeginObjetive(FinishObjetive_Way_To_Doctor_Cleared);
    }

    //externo por UnityEvent
    public void BeginPlantsMision() => MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(Plants_Recollection_ID), FinishObjetive_Curative_Plants_Recollected);
    //externo por UnityEvent
    public void ComboWomboLearned() => ChangeFase(FASE_ATENEA_DEJA_PASAR);

    void FinishObjetive_All_Jacinta_Ents_Killed() => ChangeFase(FASE_ACTIVO_DOCTOR);
    void FinishObjetive_Way_To_Doctor_Cleared() => ChangeFase(FASE_CAMINO_A_DOCTOR_DESPEJADO);
    void FinishObjetive_Curative_Plants_Recollected(int ID) { objetive_plants = true; DoctorRequirements(); }
    void FinishObjetive_Armored_Ent_Killed() { ChangeFase(FASE_ARMORED_ENT_ASESINATADO); objetive_armored_ent = true; DoctorRequirements(); }
    void DoctorRequirements()
    {
        if (objetive_armored_ent && objetive_plants)
        {
            ChangeFase(FASE_PLANTAS_ENTREGADAS);
        }
    }

    #region auxs
    void ChangeFase(int val) =>  ManagerGlobalFases.instance.ModifyFase(ID_TUTORIAL, val);
    #endregion
}
