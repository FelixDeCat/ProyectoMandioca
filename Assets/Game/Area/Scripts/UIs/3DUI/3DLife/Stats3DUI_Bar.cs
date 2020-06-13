using System.Collections;
using UnityEngine;
public class Stats3DUI_Bar : FrontendStatBase {
    public Material mat;
    private void Start() { 
        mat = GetComponent<MeshRenderer>().material;
        OnValueChange(100,100);
    }
    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        float val = value;
        float mx = max;
        float numberF = (val / mx) * 100;
        mat.SetFloat("_Value1", numberF);
    }
    
    public override void OnValueChangeWithDelay(int value, float delay,int max = 100, bool anim = false)
    {
        float auxValue = value;
        float auxMax = max;
        StartCoroutine(DelayEffect(auxValue, delay, auxMax));
    }

    IEnumerator DelayEffect(float value, float delay, float max)
    {
        //float _count = 0;
        float current = mat.GetFloat("_Value1");
        
        float meta = (value / max) * 100;
        Debug.Log($"value {value}, max {max}");
        while (current > meta)
        {
            Debug.Log($"current {current}, meta {meta}");
            //_count += Time.deltaTime;
            current--;
            
            mat.SetFloat("_Value1", current);
            yield return new WaitForEndOfFrame();
        }
        
    }
}
