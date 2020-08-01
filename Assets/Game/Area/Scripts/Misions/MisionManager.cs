using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionManager : LoadComponent
{
    public static MisionManager instancia;
    private void Awake() => instancia = this;

    public List<Mision> active_misions;
    public List<Mision> finished_misions;

    public UI_MisionManager ui;

    public UI_Mission_GeneralManager ui_panel;


    #region para carga de datos
    protected override IEnumerator LoadMe()
    {
        /// AddMissionFromDisk
        /// bla bla bla
        yield return null;
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
    #endregion

    public bool MisionIsActive(Mision m) => active_misions.Contains(m);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ui_panel.Enable();
        }
    }


    public void RefreshInPlace(string place)
    {
        foreach (var m in active_misions)
        {
            m.data.CanPrint(place);
        }
    }

    public void RemoveMision(Mision m)
    {
        m.End();
        active_misions.Remove(m);
        UI_StackMision.instancia.LogearMision(m, "MisionFinalizada", 8f);
    }

    public void AddMision(Mision m)
    {
        UI_StackMision.instancia.LogearMision(m, "Mision Nueva", 4f);
        m.Begin(EndMision);
        active_misions.Add(m);
        CheckMision();
    }


    public void CheckMision()
    {
        ui.RefreshUIMisions(active_misions);
    }

    public void EndMision(Mision m)
    {

    }
}
