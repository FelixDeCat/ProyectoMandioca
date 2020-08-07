using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticlesManager : MonoBehaviour
{
    public ParticlesManager Instance { get; private set; }

    private Dictionary<string, PoolParticle> particleRegistry = new Dictionary<string, PoolParticle>();

    Action OnEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #region SlowMO
    public void GoToSlowMo()
    {

    }
    public void BackToSlowMo()
    {

    }
    #endregion

    public void PlayParticle(string particleName, Vector3 spawnPos, Transform trackingTransform = null)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            var particlePool = particleRegistry[particleName];
            particlePool.soundPoolPlaying = true;
            ParticleSystem aS = particlePool.Get();
            aS.transform.position = spawnPos;
            if (trackingTransform != null) aS.transform.SetParent(trackingTransform);
            aS.Play();

            if (!aS.main.loop)
                StartCoroutine(ReturnSoundToPool(aS, particleName));
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }

    public void PlayParticle(string particleName, Vector3 spawnPos, Action callbackEnd, Transform trackingTransform = null)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            OnEnd = callbackEnd;
            var particlePool = particleRegistry[particleName];
            particlePool.soundPoolPlaying = true;
            ParticleSystem aS = particlePool.Get();
            aS.transform.position = spawnPos;
            if (trackingTransform != null) aS.transform.SetParent(trackingTransform);

            aS.Play();

            if (!aS.main.loop)
                StartCoroutine(ReturnSoundToPool(aS, particleName));
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }

    public void StopAllParticles(string particleName)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            particleRegistry[particleName].StopAllParticles();
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }

    public PoolParticle GetParticlePool(string particleName, ParticleSystem particle = null, int prewarmAmount = 2)
    {
        if (particleRegistry.ContainsKey(particleName)) return particleRegistry[particleName];
        else if (particle != null) return CreateNewParticlePool(particle, particleName, prewarmAmount);
        else return null;
    }

    public void DeleteParticlePool(string particleName)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            Destroy(particleRegistry[particleName].gameObject);
            particleRegistry.Remove(particleName);
        }
    }

    #region Internal
    private PoolParticle CreateNewParticlePool(ParticleSystem particle, string particleName, int prewarmAmount = 2)
    {
        var particlePool = new GameObject($"{particleName} soundPool").AddComponent<PoolParticle>();
        particlePool.transform.SetParent(transform);
        particlePool.Configure(particle);
        particlePool.Initialize(prewarmAmount);
        particleRegistry.Add(particleName, particlePool);
        return particlePool;
    }

    private IEnumerator ReturnSoundToPool(ParticleSystem aS, string sT)
    {
        yield return new WaitForSeconds(aS.main.duration);

        particleRegistry[sT].ReturnParticle(aS);
    }
    #endregion
}
