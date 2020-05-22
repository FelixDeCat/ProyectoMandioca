using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData", order = 2)]
public class SpawnData : ScriptableObject
{
    [System.Serializable]
    public struct ItemList
    {
        public GameObject item;
        public int probability;
    }

    public ItemList[] ListaItems;

   
}

