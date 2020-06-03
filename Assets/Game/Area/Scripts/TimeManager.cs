﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    float normTimeScale = 1f;

    bool isInSlowMo = false;


    public void DoSlowMotion(float _scale, float _time)
    {
        StartCoroutine(SlowMotion(_scale, _time));
    }

    public void DoHitStop()
    {
        StartCoroutine(HitStop());
    }

    public void DoSlowMo(float scale)
    {
        if (isInSlowMo) return;

        isInSlowMo = true;
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public void StopSlowMo()
    {
        isInSlowMo = false;
        Time.timeScale = normTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    

    IEnumerator SlowMotion (float scale, float time)
    {
        if (isInSlowMo)
            yield break;

        isInSlowMo = true;
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSecondsRealtime(time);

        isInSlowMo = false;
        Time.timeScale = normTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    IEnumerator HitStop()
    {
        if (isInSlowMo)
            yield break;

        isInSlowMo = true;
        Time.timeScale = 0f;
       // Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSecondsRealtime(0.1f);

        isInSlowMo = false;
        Time.timeScale = normTimeScale;
     //   Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
