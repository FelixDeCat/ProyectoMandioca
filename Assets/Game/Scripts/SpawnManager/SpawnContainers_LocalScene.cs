using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainers_LocalScene : MonoBehaviour
{
    public Transform parent_items;
    public Transform parent_entities;
    public Transform parent_others;
    public Transform parent_desctructibles;

    private void Start()
    {
        Main.instance.GetSpawner()
            .SetLocal_items_Transform(parent_items)
            .SetLocal_Entities_Transform(parent_entities)
            .SetLocal_Others_Transform(parent_others)
            .SetLocal_Destructibles_Transform(parent_desctructibles);

        Debug.Log("Spawers Seteados");
    }
}
