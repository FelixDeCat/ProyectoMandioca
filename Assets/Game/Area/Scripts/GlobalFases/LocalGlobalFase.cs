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
        Debug.LogWarning("REFRESH");
        var aux = ManagerGlobalFases.instance.GetCurrentFase(ID);

        Debug.LogWarning("REAL CURRENT:" + aux);

        if (currentfase != aux)
        {

            Debug.LogWarning("MI LOCAL CURRENT:" + currentfase);
            item_fase[currentfase].Exit();
            currentfase = aux;

            Debug.LogWarning("MI LOCAL CURRENT CAMBIADO:" + currentfase);
        }

        //Debug.LogWarning("ESTOYO EJECUTANDO:" + GetComponentsInChildren<MonoSigleFase>()[currentfase].gameObject.name);
        item_fase[currentfase].Begin();
    }
}
