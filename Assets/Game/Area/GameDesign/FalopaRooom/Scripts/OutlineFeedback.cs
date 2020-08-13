    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineFeedback : FeedbackInteractBase
{
    [SerializeField] GameObject parentWithMaterials;


    [SerializeField] Material[] myMat;
    [SerializeField] float mainOpacityOn = 0.3f;
    [SerializeField] float borderOpacityOn = 1;

    const string mainOpacity = "_MainOpacity";
    const string borderOpacity = "_OpacityOutline";

    private void Start()
    {
        myMat = parentWithMaterials.GetComponentInChildren<Renderer>().materials;
    }

    protected override void OnShow()
    {
        myMat[1].SetFloat(mainOpacity, mainOpacityOn);
        myMat[1].SetFloat(borderOpacity, borderOpacityOn);
    }

    protected override void OnHide()
    {
        myMat[1].SetFloat(mainOpacity, 0);
        myMat[1].SetFloat(borderOpacity, 0);
    }


    protected override void On_Condicional_Update() { }

    protected override void On_Permanent_Update() { }
}
