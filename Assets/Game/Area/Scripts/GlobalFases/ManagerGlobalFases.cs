using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGlobalFases : MonoBehaviour
{
    public static ManagerGlobalFases instance;
    private void Awake() => instance = this;

    public Dictionary<int, GlobalFaseData> fases = new Dictionary<int, GlobalFaseData>();

    public void AddNewFase(int ID, int max, Action refresh)
    {
        if (!fases.ContainsKey(ID))
        {
            fases.Add(ID, new GlobalFaseData(ID, max, refresh));
        }
    }
    public void LevelUPFase(int ID)
    {
        var newlevel = GetCurrentFase(ID) + 1;
        ModifyFase(ID, newlevel);
    }

    public void ModifyFase(int ID, int newLevelFase)
    {
        if (fases.ContainsKey(ID))
        {
            fases[ID].SetLevelFase(newLevelFase);
        }
    }

    public int GetCurrentFase(int ID)
    {
        int aux = 0;
        if (fases.ContainsKey(ID))
        {
            aux = fases[ID].CurrentIndex;
        }
        return aux;
    }
}

[System.Serializable]
public class GlobalFaseData
{
    int idfase; public int ID_fase { get => idfase; }
    int currentindex; public int CurrentIndex { get => currentindex; }
    int max; public int Max { get => Max; }
    Action refresh = delegate { };

    public GlobalFaseData(int ID, int _max, Action _refresh) { idfase = ID; max = _max; refresh = _refresh; }

    public void SetLevelFase(int fase) { currentindex = fase;  refresh.Invoke(); }
    public void AddLevelFase() { 

        currentindex++;
        Debug.Log("Current fase: " + currentindex);
        refresh.Invoke(); 
    }
}
