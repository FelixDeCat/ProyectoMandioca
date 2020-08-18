using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public Transform spawnposition;
    public ItemWorld item;
    private void Start()
    {
        Main.instance.SpawnItem(item, spawnposition);
    }
    public Vector3 GetPosition() => spawnposition.position;
}
