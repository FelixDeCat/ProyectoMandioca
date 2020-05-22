using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Day_Night : MonoBehaviour
{
    bool day;

    [SerializeField] Material skybox_day;
    [SerializeField] Material skybox_night;
    [SerializeField] GameObject postProcess_Day;
    [SerializeField] GameObject postProcess_Night;
    public UnityEvent OnSetDay;
    public UnityEvent OnSetNight;

    private void Start() 
    {
        startFog = RenderSettings.fogStartDistance;
        endfog = RenderSettings.fogEndDistance;
        SetDay();
    }
    public bool Change()
    {
        if (day)
        {
            day = false;
            SetNight();
        }
        else
        {
            day = true;
            SetDay();
        }

        return day;
    }

    float startFog;
    float endfog;
    public void SetDay()
    {
        RenderSettings.skybox = skybox_day;
        postProcess_Day.SetActive(true);
        postProcess_Night.SetActive(false);
        RenderSettings.fogStartDistance = startFog;
        RenderSettings.fogEndDistance = endfog;
        OnSetDay.Invoke();
    }
    public void SetNight()
    {
        RenderSettings.skybox = skybox_night;
        postProcess_Day.SetActive(false);
        postProcess_Night.SetActive(true);
        RenderSettings.fogStartDistance = 10;
        RenderSettings.fogEndDistance = 40;
        OnSetNight.Invoke();
    }
}
