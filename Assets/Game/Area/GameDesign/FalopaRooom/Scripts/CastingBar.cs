using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastingBar : MonoBehaviour
{
    //Settings
    [SerializeField] Image castingBar_img;
    [SerializeField] GameObject castingBar;
    bool isCasting;

    //Setters
    public float castingTime;

    //Events
    Action OnStartCasting = delegate { };
    Action OnFinishCasting = delegate { };
    Action OnInterruptCasting = delegate { };

    private void Start()
    {
        AddEventListener_OnStartCasting(() => Debug.Log("StartCasting"));
        AddEventListener_OnInterruptCasting(() => Debug.Log("InterruptCasting"));
        AddEventListener_OnFinishCasting(() => Debug.Log("FinishCasting"));
    }

    /// <summary>
    /// Llamarla cuando comienza el cast. Invoca el evento y arranca la corrutina
    /// </summary>
    public void StartCasting()
    {
        OnStartCasting?.Invoke();
        StartCoroutine(Casting());
    }

    public void StartCasting(float time)
    {
        castingTime = time;
        OnStartCasting?.Invoke();
        StartCoroutine(Casting());
    }
    /// <summary>
    /// Se encarga del proceso de casteo. Es un timer con el fill de la barra(no creo que usemos barra igual, sino una animacion clara)
    /// </summary>
    /// <returns></returns>
    IEnumerator Casting()
    {
        castingBar.SetActive(true);
        isCasting = true;
        castingBar_img.fillAmount = 0;
        float count = 0;

        while(count <= castingTime)
        {
            count += Time.deltaTime;
            var percent = count / castingTime;
            castingBar_img.fillAmount = percent;
            yield return null;
        }

        OnFinishCasting?.Invoke();
        isCasting = false;
        castingBar.SetActive(false);
    }

    /// <summary>
    /// Si es interrumpido el cast, se llama a esta. Para la corrutina y dispara el evento de interrupcion
    /// </summary>
    public void InterruptCasting()
    {
        if (!isCasting) return;

        StopAllCoroutines();
        OnInterruptCasting?.Invoke();
        castingBar_img.fillAmount = 0;
        castingBar.SetActive(false);
    }

    //Suscribirse a los eventos
    public void AddEventListener_OnStartCasting(Action callback) => OnStartCasting += callback;
    public void RemoveEventListener_OnStartCasting(Action callback) => OnStartCasting -= callback;
    public void AddEventListener_OnInterruptCasting(Action callback) => OnInterruptCasting += callback;
    public void RemoveEventListener_OnInterruptCasting(Action callback) => OnInterruptCasting -= callback;
    public void AddEventListener_OnFinishCasting(Action callback) => OnFinishCasting += callback;
    public void RemoveEventListener_OnFinishCasting(Action callback) => OnFinishCasting -= callback;


}
