using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TotemFeedback
{
    [SerializeField] ParticleSystem goToPos;
    [SerializeField] float timeToGoPos = 1f;

    [SerializeField] ParticleSystem chargeParticle;
    [SerializeField, Range(0, 1)] float percentToAppear = 0.5f;

    Func<IEnumerator, Coroutine> StartCoroutine = null;

    public void Initialize(Func<IEnumerator, Coroutine> _StartCoroutine)
    {

    }

    public void StartChargeFeedback(float timeToCast)
    {
        float percent = timeToCast * percentToAppear;

        StartCoroutine(StartToCharge(percent));
    }

    IEnumerator StartToCharge(float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast);
        chargeParticle?.Play();
    }


    public void StartGoToFeedback(Vector3 initPos, Vector3 finalPos, Action OnEndGo) => StartCoroutine(GoToFeedback(initPos, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Vector3 finalPos, Action OnEndGo)
    {
        var go = MonoBehaviour.Instantiate(goToPos);

        go.transform.position = initPos;
        go.Play();

        float timer = 0;

        while (timer < timeToGoPos)
        {
            timer += Time.deltaTime;

            go.transform.position = Vector3.Lerp(initPos, finalPos, timer / timeToGoPos);

            yield return new WaitForEndOfFrame();
        }

        OnEndGo?.Invoke();
    }

    public void StartGoToFeedback(Vector3 initPos, Transform finalPos, Action OnEndGo) => StartCoroutine(GoToFeedback(initPos, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Transform finalPos, Action OnEndGo)
    {
        var go = MonoBehaviour.Instantiate(goToPos);

        go.transform.position = initPos;
        go.Play();

        float timer = 0;

        while (timer < timeToGoPos)
        {
            timer += Time.deltaTime;

            go.transform.position = Vector3.Lerp(initPos, finalPos.position, timer / timeToGoPos);

            yield return new WaitForEndOfFrame();
        }

        OnEndGo?.Invoke();
    }
}
