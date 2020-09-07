using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DestructibleSystem : MonoBehaviour
{
    [SerializeField] DamageReceiver dmgReceiver = null;
    [SerializeField] _Base_Life_System lifeSystem = null;

    [SerializeField] Float_TDListDictionary percentTier = new Float_TDListDictionary();
    int maxLife;
    [SerializeField] float force;

    private void Start()
    {
        if (percentTier.Count <= 0) return;

        dmgReceiver?.Initialize(null, null, (x) => { }, TakeDamage, null, lifeSystem);

        maxLife = lifeSystem.life;
    }

    void TakeDamage(DamageData data)
    {
        List<float> percents = new List<float>();
        float per = (float)lifeSystem.Life / (float)maxLife;

        foreach (var item in percentTier)
        {
            if (item.Key >= per) percents.Add(item.Key);
        }

        for (int i = 0; i < percents.Count; i++)
        {
            if (percentTier.ContainsKey(percents[i]))
            {
                for (int v = 0; v < percentTier[percents[i]].Count; v++)
                {
                    Vector3 dir = (percentTier[percents[i]][v].transform.position - transform.position).normalized;
                    percentTier[percents[i]][v].DropPiece(dir, force);
                }

                percentTier.Remove(percents[i]);
            }
        }
    }
}
