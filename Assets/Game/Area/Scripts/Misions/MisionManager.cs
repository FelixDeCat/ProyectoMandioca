using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionManager : LoadComponent
{
    public static MisionManager instancia;
    private void Awake() => instancia = this;

    public List<Mision> active_misions;
    public UI_MisionManager ui;


    protected override IEnumerator LoadMe()
    {
        /// AddMissionFromDisk
        /// bla bla bla
        yield return null;
    }

    public bool MisionIsActive(Mision m) => active_misions.Contains(m);

    public void RemoveMision(Mision m)
    {
        m.End();
        active_misions.Remove(m);
        UI_StackMision.instancia.LogearMision(m, "MisionFinalizada", 8f);
    }

    public void AddMissionFromDisk(int index, Misions.Core.Serializable_MisionData data)
    {
        var findedmisions = FindObjectsOfType<Mision>();

        for (int i = 0; i < findedmisions.Length; i++)
        {
            if (findedmisions[i].id_mision == index)
            {
                findedmisions[i].data = data;

                ///si nunca la tuve... no importa si esta completada o no...
                ///lo que yo necsito es saber cuales estan en progreso
                if (findedmisions[i].data.IsActive)
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
            if (m.Completed)
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

    
}
