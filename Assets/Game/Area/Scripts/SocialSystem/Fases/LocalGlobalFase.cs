using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGlobalFase : MonoBehaviour
{
    [SerializeField] int ID;
    public int CurrentID { get => ID; }

    int currentfase = 0;

    public MonoSigleFase[] item_fase = new MonoSigleFase[0];

    private void Awake()
    {
        item_fase = GetComponentsInChildren<MonoSigleFase>();
    }

    public void AutoRefresh()
    {
        ManagerGlobalFases.instance.AddNewFase(ID, item_fase.Length, Refresh);
        Refresh();
    }

    void Refresh()
    {
        var aux = ManagerGlobalFases.instance.GetCurrentFase(ID);
        if (currentfase != aux)
        {
            item_fase[currentfase].Exit();
            currentfase = aux;
        }
        item_fase[currentfase].Begin();
    }
}
