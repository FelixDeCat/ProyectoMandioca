using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObject : MonoBehaviour
{
    public ParticleSystem particle;
    public ParticleSystem particleTwo;

    public GameObject obj;

    private void Update()
    {
        if (particle.isPlaying)
            obj.SetActive(true);
        else if(particleTwo.isStopped)
            obj.SetActive(false);
    }
}
