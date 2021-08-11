using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct MisionItemKey
{
    public int id;
    public int index;
    public MisionItemKey(int _id, int _index)
    {
        id = _id;
        index = _index;
    }
    public override bool Equals(object obj)
    {
        var key = (MisionItemKey)obj;
        return key.id == id && key.index == index;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class MisionManager : MonoBehaviour
{
    public static MisionManager instancia;
    private void Awake() => instancia = this;
    public Dictionary<int, Mision> registry = new Dictionary<int, Mision>();
    public List<Mision> active_misions = new List<Mision>();
    public List<Mision> finalizedMisions = new List<Mision>();
    public UI_MisionManager ui;
    public UI_Mission_GeneralManager ui_panel;
    public MisionSoundDataBase feedbackSound;

    Dictionary<MisionItemKey, int> stores = new Dictionary<MisionItemKey, int>();

    public bool MisionIsActive(Mision m) => active_misions.Contains(m);
    public void RefreshInPlace(string place)
    {
        foreach (var m in active_misions)
        {
            m.data.CanPrint(place);
        }
    }
    public void LinkEndToCallback(int Id_Mision, Action callbackToEnd)
    {
        for (int i = 0; i < active_misions.Count; i++)
        {
            if (active_misions[i].id_mision == Id_Mision)
            {
                active_misions[i].AddCallbackToEnd(callbackToEnd);
            }
        }
    }

    #region FrontEnd
    public void Refresh_UI_CheckMision()
    {
        ui.RefreshUIMisions(active_misions);
        ui_panel.RefreshCurrentMissions(active_misions);
        ui_panel.RefreshFinishedMissions(finalizedMisions);

        Canvas.ForceUpdateCanvases();
    }
    #endregion

    #region Add Mision functions
    /////////////////////////////////////////////////////
    //// AGREGADO DE MISIONS
    /////////////////////////////////////////////////////
    public bool AddMision(Mision _m, Action<int> callbackToEnd, Action<bool> mision_already_Completed = null)
    {
        if (!registry.ContainsKey(_m.id_mision))
        {
            var m = Instantiate(_m);

            for (int i = 0; i < m.data.MisionItems.Length; i++)
            {
                MisionItemKey key = new MisionItemKey(m.id_mision, i);

                if (stores.ContainsKey(key))
                {
                    m.data.MisionItems[i].SetCurrentValue(stores[key]);
                }

                m.data.MisionItems[i].InitCheck(ItemMisionFeedbackCompleted);
            }

            registry.Add(m.id_mision, m);
            active_misions.Add(m);

            if (CheckIfMissionIsCompleted(m.id_mision))
            {
                UI_StackMision.instancia.LogearMision(m, true, 2f);

                m.Begin(Refresh_UI_CheckMision);
                m.AddCallbackToEnd(CompleteMision);
                m.AddCallbackToEnd(callbackToEnd);

                if (!m.AutoEnd)
                {
                    EndMision(m.id_mision);
                }
                mision_already_Completed(true);

                UI_StackMision.instancia.LogearMision(m, true, 2f);
            }
            else
            {
                if (!m.IsHided)
                {
                    UI_StackMision.instancia.LogearMision(m, false, 2f);
                    feedbackSound.Play_NewMissionAdded();
                }
                if (LocalMisionManager.instance) LocalMisionManager.instance.OnMissionsChange();
                m.Begin(Refresh_UI_CheckMision);
                m.AddCallbackToEnd(CompleteMision);
                m.AddCallbackToEnd(callbackToEnd);
            }

            Refresh_UI_CheckMision();

            return true;
        }
        else
        {
            Refresh_UI_CheckMision();
            return false;
        }
    }
    #endregion 

    public Mision GetMisionInRegistryByID(int id)
    {
        if (registry.ContainsKey(id))
        {
            for (int i = 0; i < active_misions.Count; i++)
            {
                if (active_misions[i].id_mision == id)
                {
                    return active_misions[i];
                }
            }
        }
        return null;
    }

    #region End Mision functions
    /////////////////////////////////////////////////////
    //// ENTREGA DE MISION
    /////////////////////////////////////////////////////

    //Automatico
    public void CompleteMision(int id)
    {
        var m = GetMisionInRegistryByID(id);
        if (m != null)
        {
            m.data.SetCompletedMision();
            if (m.AutoEnd)
            {
                EndMision(id);
            }
            else
            {
                feedbackSound.Play_MissionCompleted();
            }
        }
        Refresh_UI_CheckMision();
    }
    //Manual
    public bool DeliveMision(int id)
    {
        var m = GetMisionInRegistryByID(id);
        bool ended = false;
        if (m != null)
        {
            Debug.Log("Terminando la mision: " + m.mision_name);

            if (m.CanFinishMision())
            {
                m.data.SetCompletedMision();
                EndMision(id);
                ended = true;
            }
        }

        return ended;
    }

    public bool CheckIfMissionIsCompleted(int id)
    {
        var m = GetMisionInRegistryByID(id);
        if (m != null)
        {
            bool ended = true;
            for (int i = 0; i < m.data.MisionItems.Length; i++)
            {
                if (!m.data.MisionItems[i].IsCompleted)
                {
                    
                    ended = false;
                    break;
                }
            }
            return ended;
        }
        else return false;
    }

    //completado de items
    public void AddMisionItem(int Id, int Index)
    {
        var m = GetMisionInRegistryByID(Id);

        // si me da null es xq no esta en el registro, por lo tanto todavia
        // no pasó por el AddMision, quiere decir que estoy agregando un 
        // item de mision sin antes tener la mision activa
        if (m != null)
        {
            //Debug.Log("NO ES NULO");

            if (Index < m.data.MisionItems.Length)
            {
                var aux = m.data.MisionItems[Index];
                aux.Execute(ItemMisionFeedbackCompleted);
            }
            else Debug.LogError("El Index que me pasaron es Mayor al la cantidad que tengo");
            Refresh_UI_CheckMision();
        }
        else
        {
            var fragile = MisionsDataBase.instance.GetMision(Id).data.MisionItems[Index];
            if (fragile.Store_This_Item)
            {
                MisionItemKey key = new MisionItemKey(Id, Index);

                if (!stores.ContainsKey(key))
                {
                    stores.Add(key, 1);
                }
                else
                {
                    stores[key]++;
                }
                // Debug.Log("Estoy agregando un item: " + fragile.Description + " val: " + stores[key]);
            }
        }
    }

    void ItemMisionFeedbackCompleted() => feedbackSound.Play_OneItemMisionCompleted();

    //Funcional
    void EndMision(int Id)
    {

        var m = GetMisionInRegistryByID(Id);

        if (!m.data.Delivered)
        {

            Debug.Log("Terminé: " + m.rewarding.items_rewarding.Length);
            m.data.SetDeliveredMision();
            m.End();

            feedbackSound.Play_MissionFinished();

            if (m.rewarding.items_rewarding.Length > 0)
            {
                for (int j = 0; j < m.rewarding.items_rewarding.Length; j++)
                {
                    var itm = m.rewarding.items_rewarding[j].item;

                    FastInventory.instance.Add(m.rewarding.items_rewarding[j].item, m.rewarding.items_rewarding[j].cant);
                }
            }
            active_misions.Remove(m);
            finalizedMisions.Add(m);
            UI_StackMision.instancia.LogearMision(m, true, 2f);
        }
    }

    #endregion
}
