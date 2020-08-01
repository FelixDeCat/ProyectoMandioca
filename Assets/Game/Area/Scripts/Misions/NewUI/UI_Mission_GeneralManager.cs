using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Mission_GeneralManager : MonoBehaviour
{
    public UI_MisionCollection currents;
    public UI_MisionCollection finializeds;
    public UI_MissionCompleteInfo completeinfo;
    [SerializeField] CanvasGroup CGroup;

    Dictionary<int, Mision> allmissions = new Dictionary<int, Mision>();

    bool active = false;
    private void Start()
    {
        Enable(false);
    }
    
    public void RefreshCurrentMissions(List<Mision> _currents)
    {
        currents.Refresh(_currents, RefreshCompleteInfo);
        AddCollectionToDicctionary(_currents);
    }
    public void RefreshFinishedMissions(List<Mision> _finisheds)
    {
        finializeds.Refresh(_finisheds, RefreshCompleteInfo);
        AddCollectionToDicctionary(_finisheds);
    }

    public void AddCollectionToDicctionary(List<Mision> col)
    {
        foreach (var m in col)
        {
            if (allmissions.ContainsKey(m.id_mision)) allmissions[m.id_mision] = m;
            else allmissions.Add(m.id_mision, m);
        }
    }

    public void RefreshCompleteInfo(int idMision)
    {
        if (allmissions.ContainsKey(idMision))
            completeinfo.SetInfo(allmissions[idMision]);
    }



    public void Enable()
    {
        active = !active;
        CGroup.alpha = active ? 1 : 0;
        CGroup.blocksRaycasts = CGroup.interactable = active;
    }
    public void Enable(bool _val)
    {
        CGroup.alpha = _val ? 1 : 0;
        CGroup.blocksRaycasts = CGroup.interactable = _val;
    }
}
