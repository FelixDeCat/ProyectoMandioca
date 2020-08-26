using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteItemMision : MonoBehaviour
{
    public int ID;
    public int IndexLocalItem;

    public void Execute()
    {
        
        MisionManager.instancia.AddMisionItem(ID, IndexLocalItem);
    }
}
