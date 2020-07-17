using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackFlashMaterial : FeedbackBase
{
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRederer = null;

    const int INDEX_MAT = 1;
    const string MAT_PARAM_VALUE = "_EmissionColor";

    protected override void OnPlayFeedback()
    {
        StartCoroutine(OnHitted());
    }

    IEnumerator OnHitted()
    {
        if (skinnedMeshRederer != null)
        {
            Material[] mats = skinnedMeshRederer.materials;
            skinnedMeshRederer.materials = mats; // ??
            for (int i = 0; i < onHitFlashTime; i++)
            {
                if (i < (onHitFlashTime / 2f))
                {
                    mats[INDEX_MAT].SetColor(MAT_PARAM_VALUE, Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
                }
                else
                {
                    mats[INDEX_MAT].SetColor(MAT_PARAM_VALUE, Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    
}
