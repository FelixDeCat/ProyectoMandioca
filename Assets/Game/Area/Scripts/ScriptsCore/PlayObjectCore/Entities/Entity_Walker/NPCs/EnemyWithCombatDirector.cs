using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWithCombatDirector : EnemyBase
{
    [SerializeField] protected CombatDirectorElement combatElement = null;
    [SerializeField, Range(0.5f, 30)] protected float distancePos = 1.5f;
    [SerializeField] protected float combatDistance = 20;
    protected CombatDirector director;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        director = Main.instance.GetCombatDirector();
        combatElement.Initialize(distancePos, director);
    }

    public override void ResetEntity()
    {
        base.ResetEntity();
        combatElement.ResetCombat();
    }

    public IEnumerator OnHitted(Material[] myMat, float onHitFlashTime, Color onHitColor)
    {
        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;
            Material[] mats = smr.materials;
            smr.materials = mats; // ??
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
}
