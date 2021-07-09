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
    [SerializeField] Vector3 scaleToSelect;
    [SerializeField] Vector3 scaleToUnselect;
    Vector3 targetScale;
    bool isAnim;
    [SerializeField]float animSpeed = 0.8f;
    Vector3 scale;
    RectTransform rect;
    float timer;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
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
        scale = rect.localScale;
        isAnim = true;
        targetScale = scaleToSelect;
        timer = 0;
    }

    public void UnselectAchieve()
    {
        scale = rect.localScale;
        isAnim = true;
        targetScale = scaleToUnselect;
        timer = 0;
    }

    private void Update()
    {
        if (isAnim)
        {
            rect.localScale = Vector3.Lerp(scale, targetScale, timer += animSpeed * Time.deltaTime);
            if (rect.localScale == targetScale)
                isAnim = false;
        }
    }
}
