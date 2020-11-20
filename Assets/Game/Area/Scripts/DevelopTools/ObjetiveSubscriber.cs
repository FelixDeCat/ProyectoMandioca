using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjetiveSubscriber : MonoBehaviour
{
    ObjectiveCatcher MyCatcher;

    Action OnFinishObjetive;

    //override mission Logic
    public bool OverrideMission;
    public int Mision_ID_To_Override;
    public string Item_Name_To_Override;

    //ObjetiveSubscriber Own Logic
    int objetive_cant;
    int current_cant;

    public void BeginObjetive(Action _callback_finish_objetive)
    {
        OnFinishObjetive = _callback_finish_objetive;

        MyCatcher = GetComponent<ObjectiveCatcher>();
        if (MyCatcher == null) return;

        MyCatcher.BeginCatch(CatchedObjetives);
    }

    void CatchedObjetives(Collider[] colls)
    {
        objetive_cant = colls.Length;

        // Logica de Overrideo de Misiones
        if (OverrideMission)
        {
            var mission_to_override = MisionManager.instancia.GetMisionInRegistryByID(Mision_ID_To_Override);
            mission_to_override.data.ModifyItemMision(0, colls.Length, Item_Name_To_Override);
            MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(Mision_ID_To_Override), EndMision);

            //recorro todos los colls
            for (int i = 0; i < colls.Length; i++)
            {
                Debug.Log("Col catched: > " + colls[i].gameObject.name);

                //obtengo los execute item mision
                var exes_item_mision = colls[i].gameObject.GetComponentsInChildren<ExecuteItemMision>();
                bool hasAExecute = false;
                //si algun ExecuteItemMision corresponde con la mision que quiero overridear, activo flag para que no haga nada
                for (int j = 0; j < exes_item_mision.Length; j++)
                {
                    if (exes_item_mision[j].IDs.ContainsKey(Mision_ID_To_Override))
                    {
                        hasAExecute = true;
                    }
                }

                //si ningun ExecuteItemMision coincide.. 
                if (!hasAExecute)
                {
                    //obtengo el ExecuteObjetive o me creo uno
                    var exeObjetive = colls[i].gameObject.GetComponent<ExecuteObjetive>();
                    if (!exeObjetive)
                    {
                        exeObjetive = colls[i].gameObject.AddComponent<ExecuteObjetive>();
                    }
                    //me subscribo
                    exeObjetive.SubscribeToExecuteObjetive(ObjetiveFinish);
                }
            }
        }
        else // Logica propia del Objetive Subscriber
        {
            //recorro todos los colls
            for (int i = 0; i < colls.Length; i++)
            {
                //obtengo el ExecuteObjetive o me creo uno
                var exeObjetive = colls[i].gameObject.GetComponent<ExecuteObjetive>();
                if (!exeObjetive)
                {
                    exeObjetive = colls[i].gameObject.AddComponent<ExecuteObjetive>();
                }
                Debug.Log("OBJETIVE CATCHER |||| SUBS. [" + colls[i].gameObject.name + "] => [" + this.gameObject.name+"]");
                //me subscribo
                exeObjetive.SubscribeToExecuteObjetive(ObjetiveFinish);
            }
        }
    }

    void ObjetiveFinish()
    {
        current_cant++;
        if (current_cant >= objetive_cant)
        {
            OnFinishObjetive.Invoke();
        }
    }


    void EndMision(int ID)
    {
        OnFinishObjetive.Invoke();
    }
}
