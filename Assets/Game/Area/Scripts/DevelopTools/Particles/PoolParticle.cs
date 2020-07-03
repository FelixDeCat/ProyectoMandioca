using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolParticle : SingleObjectPool<ParticleSystem>
{

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private bool _loop = false;
    public bool soundPoolPlaying = false;
    private Transform trackingTransform;

    public void Configure(ParticleSystem audioClip, bool loop = false)
    {
        particle = audioClip;
        _loop = loop;
    }
    protected override void AddObject(int prewarm = 3)
    {
        var newParticle = Instantiate(particle);
        newParticle.gameObject.SetActive(false);
        newParticle.transform.SetParent(transform);
        objects.Enqueue(newParticle);
    }

    public void StopAllSounds()
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

}
