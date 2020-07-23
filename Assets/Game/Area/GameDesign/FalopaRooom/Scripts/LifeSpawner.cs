using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpawner : MonoBehaviour
{
    public HealthPickUp healthPF;
    HealthPickUp health;

    private void Update()
    {
        if(health == null)
        {
            health = Instantiate<HealthPickUp>(healthPF, transform);
        }
    }
}
