using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeebackEmission : FeedbackBase
{
    public float onHitFlashTime;
    public Color onHitColor = Color.red;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    protected override void OnPlayFeedback()
    {
        StartCoroutine(OnHitted());
    }

    public IEnumerator OnHitted()
    {
        Material[] mats = skinnedMeshRenderer.materials;
        for (int i = 0; i < onHitFlashTime; i++)
        {
            if (i < (onHitFlashTime / 2f))
            {
                mats[1].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
            }
            else
            {
                mats[1].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
