using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteItemMision : MonoBehaviour
{
    public Int_IntDictionary IDs = new Int_IntDictionary();
    public int IdToExecute;
    public int indexItem;
    public void Execute()
    {
        MisionManager.instancia.AddMisionItem(IdToExecute, indexItem);
    }

    public void Execute(int Id)
    {
        if (IDs.ContainsKey(Id)) MisionManager.instancia.AddMisionItem(Id, IDs[Id]);
    }

    public void AddID(int ID, int indexLocal) => IDs.Add(ID, indexLocal);
    public void ResetID() => IDs.Clear();
}
