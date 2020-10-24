using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolParticle : SingleObjectPool<ParticleSystem>
{

    [SerializeField] private ParticleSystem particle;
    public bool soundPoolPlaying = false;
    public float duration;

    public void Configure(ParticleSystem _particle, float _duration)
    {
        extendible = true;
        particle = _particle;
        duration = _duration;
    } 

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

    public void PauseParticles()
    {
        for (int i = 0; i < currentlyUsingObj.Count; i++)
        {
            if(currentlyUsingObj[i] == null)
            {
                currentlyUsingObj.RemoveAt(i);
                i -= 1;
                continue;
            }

            currentlyUsingObj[i].Pause(true);
        }
    }

    public void ResumeParticles()
    {
        for (int i = 0; i < currentlyUsingObj.Count; i++)
        {
            if (currentlyUsingObj[i] == null)
            {
                currentlyUsingObj.RemoveAt(i);
                i -= 1;
                continue;
            }
            currentlyUsingObj[i].Play(true);
        }
    }
}
