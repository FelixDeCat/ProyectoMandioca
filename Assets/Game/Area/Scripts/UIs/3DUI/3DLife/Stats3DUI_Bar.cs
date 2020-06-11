using UnityEngine;
public class Stats3DUI_Bar : FrontendStatBase {
    public Material mat;
    private void Start() { 
        mat = GetComponent<MeshRenderer>().material;
        OnValueChange(100,100);
    }
    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        mat.SetFloat("_Value1", value * 10f);
    }
}
