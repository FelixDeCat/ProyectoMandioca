using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolParticle : SingleObjectPool<ParticleSystem>
{

    [SerializeField] private ParticleSystem particle;
    public bool soundPoolPlaying = false;

    public void Configure(ParticleSystem _particle) => particle = _particle;

    protected override void AddObject(int prewarm = 3)
    {
        var newParticle = Instantiate(particle);
        newParticle.gameObject.SetActive(false);
        newParticle.transform.SetParent(transform);
        objects.Enqueue(newParticle);
    }

    public void StopAllParticles()
    {
        for (int i = currentlyUsingObj.Count - 1; i >= 0; i--)
        {
            if (currentlyUsingObj[i].isPlaying)
            {
                currentlyUsingObj[i].Stop();
                ReturnToPool(currentlyUsingObj[i]);
            }
        }

        soundPoolPlaying = false;
    }

    public void ReturnParticle(ParticleSystem particle)
    {
        if (particle == null) return;

        particle.transform.SetParent(transform);
        particle.Stop();
        ReturnToPool(particle);
    }
}
