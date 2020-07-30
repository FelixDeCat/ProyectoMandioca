using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System.Linq;

public class FallingArena : MonoBehaviour
{
    [Range(0,1)]
    public float percentFalling;

    void Start()
    {
        var playObjects = GetComponentsInChildren<ColapsingFloor>().ToList();
        int a =  (int)(playObjects.Count * percentFalling);

        for (int i = 0; i < a; i++)
        {
            var rng = Random.Range(0, playObjects.Count);
            playObjects[rng].On();
        }
    }



}
