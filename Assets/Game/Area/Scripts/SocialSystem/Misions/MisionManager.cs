using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MisionManager : MonoBehaviour
{
    public static MisionManager instancia;
    private void Awake() => instancia = this;

    public Dictionary<int, Mision> registry = new Dictionary<int, Mision>();

    public List<Mision> active_misions = new List<Mision>();
    public List<Mision> finalizedMisions = new List<Mision>();

    public UI_MisionManager ui;

    public UI_Mission_GeneralManager ui_panel;


    public bool MisionIsActive(Mision m) => active_misions.Contains(m);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ui_panel.Enable();
        }
    }

    public void AddMisionItem(int Id, int Index)
    {
        bool notfount = false;
        for (int i = 0; i < active_misions.Count; i++)
        {
            if (active_misions[i].id_mision == Id)
            {
                var aux = active_misions[i].data.MisionItems[Index];
                aux.Execute();
                notfount = true;
            }
        }

        if (notfount)
        {
            MissionMemory.instance.AddToMemory(Id, Index);
        }

        CheckMision();
    }

    public void RefreshInPlace(string place)
    {
        foreach (var m in active_misions)
        {
            m.data.CanPrint(place);
        }
    }

    public bool AddMision(Mision m, Action<Mision> callbackToEnd)
    {
        if (!registry.ContainsKey(m.id_mision))
        {
            registry.Add(m.id_mision, m);
            if(!m.data.IsHided) UI_StackMision.instancia.LogearMision(m, false, 4f);
            if (LocalMisionManager.instance) LocalMisionManager.instance.OnMissionsChange();
            m.Begin(CheckMision);
            m.AddCallbackToEnd(MissionIsEnded);
            m.AddCallbackToEnd(callbackToEnd);
            active_misions.Add(m);
            CheckMision();
            m.OnRefresh();
            return true;
        }
        else
        {
            m.OnRefresh();
            CheckMision();
            return false;
        }
    }
    public bool AddMision(Mision m)
    {
        if (!registry.ContainsKey(m.id_mision))
        {
            registry.Add(m.id_mision, m);
            if(!m.data.IsHided) UI_StackMision.instancia.LogearMision(m, false, 4f);
            if (LocalMisionManager.instance) LocalMisionManager.instance.OnMissionsChange();
            m.Begin(CheckMision);
            m.AddCallbackToEnd(MissionIsEnded);
            active_misions.Add(m);
            m.OnRefresh();
            CheckMision();
            return true;
        }
        else
        {
            m.OnRefresh();
            CheckMision();
            return false;
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

    public void CheckMision()
    {
        ui.RefreshUIMisions(active_misions);
        ui_panel.RefreshCurrentMissions(active_misions);
        ui_panel.RefreshFinishedMissions(finalizedMisions);

        Canvas.ForceUpdateCanvases();
    }

    public void MissionIsEnded(Mision m)
    {
        m.End();

        if (m.rewarding.items_rewarding.Length > 0)
        {
            for (int i = 0; i < m.rewarding.items_rewarding.Length; i++)
            {
                var itm = m.rewarding.items_rewarding[i].item;

                if (itm.equipable)
                {
                    EquipedManager.instance.EquipItem(itm);
                }
                else
                {
                    FastInventory.instance.Add(m.rewarding.items_rewarding[i].item, m.rewarding.items_rewarding[i].cant);
                }

                //for con todos las entregas

            }
        }

        active_misions.Remove(m);
        finalizedMisions.Add(m);
        if (m.data.FasesToChange.Length > 0)
        {
            Debug.Log("Tengo fases por cambiar");
            for (int i = 0; i < m.data.FasesToChange.Length; i++)
            {
                ManagerGlobalFases.instance.ModifyFase(m.data.FasesToChange[i].IDFase, m.data.FasesToChange[i].IndexToChange);
            }
        }
        UI_StackMision.instancia.LogearMision(m, true, 8f); 
    }
}
