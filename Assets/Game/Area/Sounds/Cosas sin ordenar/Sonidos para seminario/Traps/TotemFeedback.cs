using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TotemFeedback
{
    [SerializeField] ParticleSystem goToPos = null;
    [SerializeField] float timeToGoPos = 1f;

    [SerializeField] ParticleSystem chargeParticle = null;
    ParticleSystem chargeParticleTemp;

    [SerializeField] public Transform startPos = null;

    Func<IEnumerator, Coroutine> StartCoroutine;

    public bool pause;

    List<ParticleSystem> particlesGoTo = new List<ParticleSystem>();

    public void Initialize(Func<IEnumerator, Coroutine> _StartCoroutine)
    {
        StartCoroutine = _StartCoroutine;

        ParticlesManager.Instance.GetParticlePool(chargeParticle.name, chargeParticle, 3);
        ParticlesManager.Instance.GetParticlePool(goToPos.name, goToPos, 6);
    }

    public void StartChargeFeedback(Action EndCast)
    {
        chargeParticleTemp = ParticlesManager.Instance.PlayParticle(chargeParticle.name, startPos.position, EndCast, startPos);
    }

    public void InterruptCharge()
    {
        if(chargeParticleTemp != null && chargeParticleTemp.gameObject.activeSelf) ParticlesManager.Instance.StopParticle(chargeParticle.name, chargeParticleTemp);
    }

    public void StartGoToFeedback(Vector3 finalPos, Action<Vector3> OnEndGo) => StartCoroutine(GoToFeedback(startPos.position, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Vector3 finalPos, Action<Vector3> OnEndGo)
    {
        var go = ParticlesManager.Instance.PlayParticle(goToPos.name, initPos);

        particlesGoTo.Add(go);

        float timer = 0;

        while (timer < timeToGoPos)
        {
            if (!pause) timer += Time.deltaTime;

            go.transform.position = Vector3.Lerp(initPos, finalPos, timer / timeToGoPos);

            yield return new WaitForEndOfFrame();
        }

        OnEndGo?.Invoke(finalPos);

        particlesGoTo.Remove(go);
        ParticlesManager.Instance.StopParticle(goToPos.name, go);
    }

    public void StartGoToFeedback(Transform finalPos, Action<Vector3> OnEndGo) => StartCoroutine(GoToFeedback(startPos.position, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Transform finalPos, Action<Vector3> OnEndGo)
    {
        var go = ParticlesManager.Instance.PlayParticle(goToPos.name, initPos);

        particlesGoTo.Add(go);

        float timer = 0;

        while (timer < timeToGoPos)
        {
            if(!pause) timer += Time.deltaTime;

            go.transform.position = Vector3.Lerp(initPos, finalPos.position, timer / timeToGoPos);

            yield return new WaitForEndOfFrame();
        }

        OnEndGo?.Invoke(finalPos.position);

        particlesGoTo.Remove(go);
        ParticlesManager.Instance.StopParticle(goToPos.name, go);
    }

   
    public void StopAll()
    {
        for (int i = 0; i < particlesGoTo.Count; i++)
            ParticlesManager.Instance.StopParticle(goToPos.name, particlesGoTo[i]);

        if (chargeParticleTemp != null && chargeParticleTemp.gameObject.activeSelf) ParticlesManager.Instance.StopParticle(chargeParticle.name, chargeParticleTemp);
    }
}
