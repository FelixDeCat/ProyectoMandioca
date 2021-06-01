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
    bool objetive_plants = false;

    [SerializeField] CameraCinematic animacion_camara_tranquera = null;

    public void TRIGGER_StartTutorial()
    {
        ChangeFase(FASE_LIBERA_CAMINO_JACINTA);

        jacinta_ents_Objetive.BeginObjetive(FinishObjetive_All_Jacinta_Ents_Killed);
        farm_armored_ent_Objetive.BeginObjetive(FinishObjetive_Armored_Ent_Killed);
        doctorway_ents_Objetive.BeginObjetive(FinishObjetive_Way_To_Doctor_Cleared);
    }

    //externo por UnityEvent
    public void BeginPlantsMision()
    {
        MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(Plants_Recollection_ID), FinishObjetive_Curative_Plants_Recollected,TryToLogMisionAlreadyCompleted );
    }
    //externo por UnityEvent
    public void ComboWomboLearned() => ChangeFase(FASE_ATENEA_DEJA_PASAR);

    void TryToLogMisionAlreadyCompleted(bool val)
    {
        if (!val)
        {
            animacion_camara_tranquera.StartCinematic();
        }
    }

    void FinishObjetive_All_Jacinta_Ents_Killed() => ChangeFase(FASE_ACTIVO_DOCTOR);
    void FinishObjetive_Way_To_Doctor_Cleared() => ChangeFase(FASE_CAMINO_A_DOCTOR_DESPEJADO);
    void FinishObjetive_Curative_Plants_Recollected(int ID)
    {
        Debug.Log("Objetivo de plantas");
        objetive_plants = true;
        DoctorRequirements();
    }
    void FinishObjetive_Armored_Ent_Killed()
    {
        Debug.Log("Objetivo de Ent Acorazado");
        ChangeFase(FASE_ARMORED_ENT_ASESINATADO);
        DoctorRequirements();
    }
    void DoctorRequirements()
    {
        if (objetive_plants)
        {
            Debug.LogWarning("TERMINO LA MISION");
            ChangeFase(FASE_PLANTAS_ENTREGADAS);
        }
    }

    #region auxs
    void ChangeFase(int val) { ManagerGlobalFases.instance.ModifyFase(ID_TUTORIAL, val); }
    #endregion
}
