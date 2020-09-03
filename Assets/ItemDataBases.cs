using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBases : MonoBehaviour
{
    [SerializeField] Item[] database;

    public Dictionary<int, Item> items_bautizados = new Dictionary<int, Item>();

    public static ItemDataBases instance;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < database.Length; i++)
        {
            if (!items_bautizados.ContainsKey(database[i].id))
            {
                items_bautizados.Add(database[i].id, database[i]);
            }
            else
            {
                items_bautizados[database[i].id] = database[i];
            }
        }
    }

    public Item GetItemByID(int ID)
    {
        return items_bautizados[ID];
    }
}
