using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionManager : MonoBehaviour
{
    public static MisionManager instancia;
    private void Awake() => instancia = this;

    public List<Mision> active_misions;
    public List<Mision> allmisions;
    public UI_MisionManager ui;

    public Mision first;

    public bool MisionIsActive(Mision m) => active_misions.Contains(m);

    public void FirstTime() => AddMision(first);
    public void RemoveMision(Mision m)
    {
        m.isactive = false;
        active_misions.Remove(m);
        UI_StackMision.instancia.LogearMision(m, "MisionFinalizada", 8f);
    }

    public void AddMissionFromDisk(int id, int[] progression, bool _isactive, bool _completed)
    {
        var findedmisions = FindObjectsOfType<Mision>();

        for (int i = 0; i < findedmisions.Length; i++)
        {
            if (findedmisions[i].id_mision == id)
            {
                findedmisions[i].alreadyconfigured = true;
                findedmisions[i].SetProgresion(progression);
                findedmisions[i].isactive = _isactive;
                findedmisions[i].completed = _completed;

                ///si nunca la tuve... no importa si esta completada o no...
                ///lo que yo necsito es saber cuales estan en progreso
                if (findedmisions[i].isactive)
                {
                    AddMision(findedmisions[i]);
                }
                
                return;
            }
            else
            {
                continue;
            }
        }

        Debug.LogError("NO se encuentra el indice de esta mision, fijate si esta mision que buscas tiene asignado el indice");
    }

    public void AddMision(Mision m)
    {
        m.isactive = true;
        UI_StackMision.instancia.LogearMision(m, "Mision Nueva", 4f);
        m.Begin();
        active_misions.Add(m);
        CheckMision();
    }

    public void CheckMision()
    {
        List<Mision> nextMisions = new List<Mision>();
        List<Mision> marktoremove = new List<Mision>();
        foreach (var m in active_misions) m.CheckProgresion();
        foreach (var m in active_misions)
        {
            if (m.completed)
            {
                m.End();
                if (m.next) nextMisions.Add(m.next);
                marktoremove.Add(m);
            }
        }
        foreach (var m in marktoremove) RemoveMision(m);
        foreach (var nm in nextMisions) AddMision(nm);
        foreach (var m in active_misions) m.Refresh();
        ui.RefreshUIMisions(active_misions);
    }

    private void Update()
    {
        if (active_misions.Count >= 1)
        {
            for (int i = 0; i < active_misions.Count; i++)
                active_misions[i].OnUpdate();
        }
    }
}
