using UnityEngine;
public class Stats3DUI_Bar : FrontendStatBase {
    public Material mat;
    private void Start() => mat = GetComponent<MeshRenderer>().material;
    public override void OnValueChange(int value, int max = 100, bool anim = false) => mat.SetFloat("_Value", value * 10f);
}
