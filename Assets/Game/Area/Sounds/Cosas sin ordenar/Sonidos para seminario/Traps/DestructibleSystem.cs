using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleSystem : MonoBehaviour
{
    [SerializeField] DamageReceiver dmgReceiver = null;
    [SerializeField] _Base_Life_System lifeSystem = null;

    [SerializeField] Float_TDListDictionary percentTier = new Float_TDListDictionary();
    Float_TDListDictionary internalTier = new Float_TDListDictionary();
    int maxLife;
    [SerializeField] float force = 15;

    public void Initialize()
    {
        if (percentTier.Count <= 0) return;

        dmgReceiver?.AddTakeDamage(TakeDamage);

        maxLife = lifeSystem.life;
        foreach (var item in percentTier)
            internalTier.Add(item.Key, item.Value);
    }

    public void OnReset()
    {
        maxLife = lifeSystem.life;
        internalTier.Clear();
        foreach (var item in percentTier)
        {
            internalTier.Add(item.Key, item.Value);
            for (int v = 0; v < percentTier[item.Key].Count; v++) percentTier[item.Key][v].OnReset();
        }
    }

    void TakeDamage(DamageData data)
    {
        List<float> percents = new List<float>();
        float per = (float)lifeSystem.Life / (float)maxLife;

        foreach (var item in internalTier)
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
                    internalTier[percents[i]][v].DropPiece(dir, force);
                }

                internalTier.Remove(percents[i]);
            }
        }
    }
}
