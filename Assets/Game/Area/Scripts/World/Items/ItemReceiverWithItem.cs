using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiverWithItem : MonoBehaviour
{
    public Item item;

    public void OnCollectItem()
    {
        FastInventory.instance.Add(item);
    }
}
