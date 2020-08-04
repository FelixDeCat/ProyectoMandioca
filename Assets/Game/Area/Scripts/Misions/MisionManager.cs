using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for (int i = 0; i < active_misions.Count; i++)
        {
            if (active_misions[i].id_mision == Id)
            {
                var aux = active_misions[i].data.MisionItems[Index];
                aux.Execute();
            }
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

    public void RemoveMision(Mision m)
    {
        
    }

    public void AddMision(Mision m)
    {
        if (!registry.ContainsKey(m.id_mision))
        {
            registry.Add(m.id_mision, m);
            UI_StackMision.instancia.LogearMision(m, false, 4f);
            if (LocalMisionManager.instance) LocalMisionManager.instance.OnMissionsChange();
            m.Begin(EndMision, CheckMision);
            active_misions.Add(m);
        }
        
        CheckMision();
    }

    public void CheckMision()
    {
        ui.RefreshUIMisions(active_misions);
        ui_panel.RefreshCurrentMissions(active_misions);
        ui_panel.RefreshFinishedMissions(finalizedMisions);

        Canvas.ForceUpdateCanvases();
    }

    public void EndMision(Mision m)
    {
        m.End();
        active_misions.Remove(m);
        finalizedMisions.Add(m);
        UI_StackMision.instancia.LogearMision(m, true, 8f); 
    }
}
