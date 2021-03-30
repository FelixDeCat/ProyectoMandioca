using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats2D_Bar : FrontendStatBase
{
    [SerializeField] Image img = null;
    [SerializeField] Image img_back = null;

    bool anim;
    public float speed = 0.5f;
    float val_destiny;
    public float delay = 0.5f;

    Color normalColor;

    private void Start()
    {
        normalColor = img.color;
    }

    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        float val = value;
        float mx = max;
        float numberF = (val / mx);
        val_destiny = numberF;
        img.fillAmount = numberF;
        img_back.enabled = true;
        if (img_back.fillAmount < numberF)
        { 
           img_back.fillAmount = numberF;
        }
        Invoke("BeginAnim", delay);
    }

    void BeginAnim()
    {
        this.anim = true;
    }

    public void ChangeBarColor(Color newColor) => img.color = newColor;

    public void ReturnToNormalColor() => img.color = normalColor;

    private void Update()
    {
        if (anim)
        {
            if (img_back.fillAmount >= val_destiny)
            {
                img_back.fillAmount = img_back.fillAmount - 1 * Time.deltaTime * speed;
            }
            else
            {
                img_back.fillAmount = val_destiny;
                img_back.enabled = false;
                anim = false;
            }
        }
    }

    public override void OnValueChangeWithDelay(int value, float delay, int max = 100, bool anim = false)
    {
        
    }


    //[SerializeField] Image img;
    //[SerializeField] Image img_back;

    //bool anim;
    //public float speed = 0.5f;
    //float val_destiny;
    //public float delay = 0.5f;

    //public override void OnValueChange(int value, int max = 100, bool anim = false)
    //{
    //    float numberF = (value / max);
    //    val_destiny = numberF;
    //    img.fillAmount = numberF;
    //    //if (img_back.fillAmount < val_destiny) img_back.fillAmount = val_destiny;
    //    //Invoke("BeginAnim", delay);
    //}

    //void BeginAnim()
    //{
    //    anim = true;
    //}

    //IEnumerator DelayEffect(float value, float delay, float max)
    //{
    //    yield return new WaitForSeconds(delay);
    //    float val = value;
    //    float mx = max;
    //    float numberF = (val / mx);
    //    while (val > numberF)
    //    {
    //        val--;
    //        img_back.fillAmount = val;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
