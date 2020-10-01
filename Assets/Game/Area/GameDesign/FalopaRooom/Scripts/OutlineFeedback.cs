    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutlineFeedback : FeedbackInteractBase
{
    [SerializeField] GameObject parentWithMaterials = null;


    [SerializeField] Material[] myMat = new Material[1];
    [SerializeField] float mainOpacityOn = 0.3f;
    [SerializeField] float borderOpacityOn = 1;

    const string mainOpacity = "_MainOpacity";
    const string borderOpacity = "_OpacityOutline";

    public string shader_name;

    private void Start()
    {
        myMat = parentWithMaterials.GetComponentsInChildren<Renderer>()
            .Where(x =>  x.materials.Length > 1)
            .Select(x => x.materials[1])
            .ToArray();
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
