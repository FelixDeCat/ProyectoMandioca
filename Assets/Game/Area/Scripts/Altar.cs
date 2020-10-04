using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public Transform spawnposition;
    public InteractCollect item;
    public Item itemspawn;
    private void Start()
    {
        if (item != null) Main.instance.SpawnItem(item, spawnposition);
        if (itemspawn != null) Main.instance.SpawnItem(itemspawn, spawnposition);
    }
    public Vector3 GetPosition() => spawnposition.position;
}
