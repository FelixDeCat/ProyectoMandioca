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
    [SerializeField, Range(0, 1)] float percentToAppear = 0.5f;

    [SerializeField] Transform startPos = null;

    Func<IEnumerator, Coroutine> StartCoroutine;
    Action<IEnumerator> StopCoroutine;

    List<ParticleSystem> particlesGoTo = new List<ParticleSystem>();

    public void Initialize(Func<IEnumerator, Coroutine> _StartCoroutine, Action<IEnumerator> _StopCoroutine)
    {
        StartCoroutine = _StartCoroutine;
        StopCoroutine = _StopCoroutine;

        ParticlesManager.Instance.GetParticlePool(chargeParticle.name, chargeParticle, 3);
        ParticlesManager.Instance.GetParticlePool(goToPos.name, goToPos, 6);
    }

    public void StartChargeFeedback(float timeToCast)
    {
        float percent = timeToCast * percentToAppear;

        StartCoroutine(StartToCharge(percent));
    }

    IEnumerator StartToCharge(float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast);

        chargeParticleTemp = ParticlesManager.Instance.PlayParticle(chargeParticle.name, startPos.position, startPos);
    }

    public void InterruptCharge()
    {
        if(chargeParticleTemp != null && chargeParticleTemp.gameObject.activeSelf) ParticlesManager.Instance.StopParticle(chargeParticle.name, chargeParticleTemp);
        StopCoroutine(StartToCharge(0));
    }

    public void StartGoToFeedback(Vector3 finalPos, Action<Vector3> OnEndGo) => StartCoroutine(GoToFeedback(startPos.position, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Vector3 finalPos, Action<Vector3> OnEndGo)
    {
        var go = ParticlesManager.Instance.PlayParticle(goToPos.name, initPos);

        particlesGoTo.Add(go);

        float timer = 0;

        while (timer < timeToGoPos)
        {
            timer += Time.deltaTime;

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
            timer += Time.deltaTime;

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
