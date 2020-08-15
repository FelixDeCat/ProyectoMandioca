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

    [SerializeField] Transform startPos;

    Func<IEnumerator, Coroutine> StartCoroutine;
    Action<IEnumerator> StopCoroutine;

    public void Initialize(Func<IEnumerator, Coroutine> _StartCoroutine, Action<IEnumerator> _StopCoroutine)
    {
        StartCoroutine = _StartCoroutine;
        StopCoroutine = _StopCoroutine;

        var go = MonoBehaviour.Instantiate(chargeParticle);

        go.transform.position = startPos.position;

        chargeParticle = go;
    }

    public void StartChargeFeedback(float timeToCast)
    {
        float percent = timeToCast * percentToAppear;

        StartCoroutine(StartToCharge(percent));
    }

    IEnumerator StartToCharge(float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast);
        chargeParticle.transform.position = startPos.position;
        chargeParticle?.Play();
    }

    public void InterruptCharge() => StopCoroutine(StartToCharge(0));

    public void StartGoToFeedback(Vector3 finalPos, Action<Vector3> OnEndGo) => StartCoroutine(GoToFeedback(startPos.position, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Vector3 finalPos, Action<Vector3> OnEndGo)
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

        OnEndGo?.Invoke(finalPos);

        go.Stop();
        go.gameObject.SetActive(false);
    }

    public void StartGoToFeedback(Transform finalPos, Action<Vector3> OnEndGo) => StartCoroutine(GoToFeedback(startPos.position, finalPos, OnEndGo));

    IEnumerator GoToFeedback(Vector3 initPos, Transform finalPos, Action<Vector3> OnEndGo)
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

        OnEndGo?.Invoke(finalPos.position);

        go.Stop();
        go.gameObject.SetActive(false);
    }
}
