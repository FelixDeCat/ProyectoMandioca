    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutlineFeedback : FeedbackInteractBase
{
    [SerializeField] GameObject parentWithMaterials = null;


    [SerializeField] Material[] myMat = new Material[1];
    [SerializeField] float borderOpacityOn = 1;
    const string borderOpacity = "_OpacityOutline";

    public string[] shaders_name;

    private void Start()
    {
        myMat = parentWithMaterials.GetComponentsInChildren<Renderer>()
            .Select(x => x.material)
            .ToArray();
                shaders_name = parentWithMaterials.GetComponentsInChildren<Renderer>()
            .Select(x => x.material.shader.name)
            .ToArray();
    }

    protected override void OnShow()
    {
        for(int i = 0; i < myMat.Length; i++)
        {
            for (int j = 0; j < shaders_name.Length; j++)
            {
                if (myMat[i].shader.name == shaders_name[j])
                {
                    myMat[i].SetFloat(borderOpacity, borderOpacityOn);
                }
            }
            
        }
    }

    protected override void OnHide()
    {
        for (int i = 0; i < myMat.Length; i++)
        {
            for (int j = 0; j < shaders_name.Length; j++)
            {
                if (myMat[i].shader.name == shaders_name[j])
                {
                    myMat[i].SetFloat(borderOpacity, 0);
                }
            }
        }
    }


    protected override void On_Condicional_Update() { }

    protected override void On_Permanent_Update() { }
}
