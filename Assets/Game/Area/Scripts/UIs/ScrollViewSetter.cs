using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewSetter : MonoBehaviour
{
    [SerializeField] AchieveUI achievePrefab = null;
    [SerializeField] Transform parent = null;

    AchieveUI[] achieves = new AchieveUI[0];

    private void Awake()
    {
        achieves = new AchieveUI[AchievesManager.instance.allAchieves.Count];
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i] = Instantiate(achievePrefab, parent);
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
        }
    }

    public void RefreshAchieves()
    {
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
        }
    }
}
