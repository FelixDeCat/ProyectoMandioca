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

    Dictionary<MisionItemKey, int> stores = new Dictionary<MisionItemKey, int>();

    public bool MisionIsActive(Mision m) => active_misions.Contains(m);
    private void Update() { if (Input.GetKeyDown(KeyCode.J)) ui_panel.Enable(); }
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
    public void CheckMision()
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
    public bool AddMision(Mision _m, Action<int> callbackToEnd)
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
            }

            registry.Add(m.id_mision, m);
            if (!m.IsHided) UI_StackMision.instancia.LogearMision(m, false, 4f);
            if (LocalMisionManager.instance) LocalMisionManager.instance.OnMissionsChange();
            m.Begin(CheckMision);
            m.AddCallbackToEnd(CompleteMision);
            m.AddCallbackToEnd(callbackToEnd);
            active_misions.Add(m);
            CheckMision();
            m.OnRefresh();
            return true;
        }
        else
        {
            var m = registry[_m.id_mision];
            m.OnRefresh();
            CheckMision();
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
            if (m.AutoEnd) EndMision(id);
        }
    }
    //Manual
    public void DeliveMision(int id)
    {
        var m = GetMisionInRegistryByID(id);
        if (m != null)
        {
            if (m.CanFinishMision())
            {
                m.data.SetCompletedMision();
                EndMision(id);
            }
        }
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
                aux.Execute();
            }
            else Debug.LogError("El Index que me pasaron es Mayor al la cantidad que tengo");
            CheckMision();
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

    //Funcional
    void EndMision(int Id)
    {
        var m = GetMisionInRegistryByID(Id);

        if (!m.data.Delivered)
        {
            m.data.SetDeliveredMision();
            m.End();
            if (m.rewarding.items_rewarding.Length > 0)
            {
                for (int j = 0; j < m.rewarding.items_rewarding.Length; j++)
                {
                    var itm = m.rewarding.items_rewarding[j].item;

                    if (itm.equipable)
                    {
                        EquipedManager.instance.EquipItem(itm);
                    }
                    else
                    {
                        FastInventory.instance.Add(m.rewarding.items_rewarding[j].item, m.rewarding.items_rewarding[j].cant);
                    }
                }
            }
            active_misions.Remove(m);
            finalizedMisions.Add(m);
            UI_StackMision.instancia.LogearMision(m, true, 8f);
        }
    }

    #endregion
}
