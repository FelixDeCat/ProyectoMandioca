using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSetter : MonoBehaviour
{
    [SerializeField] AchieveUI achievePrefab = null;
    [SerializeField] Transform parent = null;
    [SerializeField] Scrollbar bar = null;

    AchieveUI[] achieves = new AchieveUI[0];

    private void Awake()
    {
        achieves = new AchieveUI[AchievesManager.instance.allAchieves.Count];
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i] = Instantiate(achievePrefab, parent);
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
        }
        for (int i = achieves.Length - 1; i >= 0; i--)
        {
            if (AchievesManager.instance.achieves.achievesComplete[i]) achieves[i].GetComponent<RectTransform>().SetAsFirstSibling();
        }
        bar.value = 1;
    }

    public void RefreshAchieves()
    {
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
        }
        for (int i = achieves.Length - 1; i >= 0; i--)
        {
            if (AchievesManager.instance.achieves.achievesComplete[i]) achieves[i].GetComponent<RectTransform>().SetAsFirstSibling();
        }

        bar.value = 1;
    }

    public void ResetAchieves()
    {
        AchievesManager.instance.ClearAllAchieves(true);
        RefreshAchieves();
    }
}
