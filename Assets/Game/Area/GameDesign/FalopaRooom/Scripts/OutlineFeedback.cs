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

    public string shader_name;

    private void Start()
    {
        myMat = parentWithMaterials.GetComponentInChildren<Renderer>().materials;
    }

    protected override void OnShow()
    {
        for(int i = 0; i < myMat.Length; i++)
        {
            if (myMat[i].shader.name == shader_name)
            {
                myMat[i].SetFloat(mainOpacity, mainOpacityOn);
                myMat[i].SetFloat(borderOpacity, borderOpacityOn);
            }
        }
    }

    protected override void OnHide()
    {
        for (int i = 0; i < myMat.Length; i++)
        {
            if (myMat[i].shader.name == shader_name)
            {
                myMat[i].SetFloat(mainOpacity, 0);
                myMat[i].SetFloat(borderOpacity, 0);
            }
        }
    }


    protected override void On_Condicional_Update() { }

    protected override void On_Permanent_Update() { }
}
