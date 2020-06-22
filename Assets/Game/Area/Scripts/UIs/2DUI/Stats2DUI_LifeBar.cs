using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats2DUI_LifeBar : FrontendStatBase
{
    Image filledImage;

    private void Start()
    {
        filledImage = GetComponent<Image>();
        OnValueChange(100, 100);
    }

    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        float fValue = value;
        float fMax = max;
        
        if (fValue > fMax)
            fValue = fMax;

        float result = fValue / fMax;

        filledImage.fillAmount = result;
    }

    public override void OnValueChangeWithDelay(int value, float delay, int max = 100, bool anim = false)
    {
        float fValue = value;
        float fMax = max;

        if (fValue > fMax)
            fValue = fMax;

        StartCoroutine(DelayEffect(fValue, delay, fMax));
    }

    IEnumerator DelayEffect(float value, float delay, float max)
    {
        yield return new WaitForSeconds(delay);

        float current = filledImage.fillAmount;
        float meta = value / max;

        while (current > meta)
        {
            current--;
            filledImage.fillAmount = current;
            yield return new WaitForEndOfFrame();
        }

    }
}
