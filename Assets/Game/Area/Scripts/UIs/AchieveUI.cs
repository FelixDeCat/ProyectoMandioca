using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchieveUI : MonoBehaviour
{
    [SerializeField] Image achieveImg = null;
    [SerializeField] Image background = null;
    [SerializeField] Sprite imgLock = null;
    [SerializeField] TextMeshProUGUI achieveTitle = null;
    [SerializeField] TextMeshProUGUI achieveDesc = null;
    [SerializeField] Color blockColor;
    [SerializeField] Color completeColor;

    public void SetAchieve(Achieves achieve, bool isComplete)
    {
        if (isComplete)
        {
            achieveImg.sprite = achieve.achiveImg;
            achieveTitle.text = achieve.title;
            achieveDesc.text = achieve.description;
            background.color = completeColor;
        }
        else
        {
            achieveImg.sprite = imgLock;
            achieveTitle.text = "???";
            achieveDesc.text = achieve.blockDescription;
            background.color = blockColor;
        }
    }

    public void SelectAchieve()
    {

    }

    public void UnselectAchieve()
    {

    }
}
