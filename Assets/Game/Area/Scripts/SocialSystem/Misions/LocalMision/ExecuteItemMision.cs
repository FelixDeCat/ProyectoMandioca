using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteItemMision : MonoBehaviour
{
    public int ID;
    public int IndexLocalItem;

    public void Execute()
    {
        Debug.Log("EJECUTO ESTO... tantas veces");
        MisionManager.instancia.AddMisionItem(ID, IndexLocalItem);
    }
}
