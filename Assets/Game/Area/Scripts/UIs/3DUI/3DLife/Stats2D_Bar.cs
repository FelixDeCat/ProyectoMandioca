using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats2D_Bar : FrontendStatBase
{
    private Image img;
    private void Start()
    {
        img = GetComponent<Image>();
    }

    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        float val = value;
        float mx = max;
        float numberF = (val / mx);
        Debug.Log($"{numberF} y el de la fill amount es {img.fillAmount}" );
        
        img.fillAmount = numberF;
    }

    public override void OnValueChangeWithDelay(int value, float delay, int max = 100, bool anim = false)
    {
        
    }
}
