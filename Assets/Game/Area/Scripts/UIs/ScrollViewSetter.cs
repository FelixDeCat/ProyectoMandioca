﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSetter : MonoBehaviour
{
    [SerializeField] AchieveUI achievePrefab = null;
    [SerializeField] Transform parent = null;
    [SerializeField] Scrollbar bar = null;
    [SerializeField] RectTransform posToSelected = null;
    [SerializeField] Button backButton = null;
    [SerializeField] Button resetButton = null;
    public AchieveUI selectedAchieve;
    int currentSelection;

    AchieveUI[] achieves = new AchieveUI[0];
    List<AchieveUI> toNavigate = new List<AchieveUI>();

    private void Awake()
    {
        toNavigate = new List<AchieveUI>();
        achieves = new AchieveUI[AchievesManager.instance.allAchieves.Count];
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i] = Instantiate(achievePrefab, parent);
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
            if(!AchievesManager.instance.achieves.achievesComplete[i]) toNavigate.Add(achieves[i]);
        }
        for (int i = achieves.Length - 1; i >= 0; i--)
        {
            if (AchievesManager.instance.achieves.achievesComplete[i])
            {
                achieves[i].GetComponent<RectTransform>().SetAsFirstSibling();
                toNavigate.Insert(0, achieves[i]);
            }
            StartCoroutine(WaitSeconds());
        }
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForEndOfFrame();
        MoveParent();
    }

    public void RefreshAchieves()
    {
        toNavigate = new List<AchieveUI>();
        for (int i = 0; i < achieves.Length; i++)
        {
            achieves[i].SetAchieve(AchievesManager.instance.allAchieves[i], AchievesManager.instance.achieves.achievesComplete[i]);
            if (!AchievesManager.instance.achieves.achievesComplete[i]) toNavigate.Add(achieves[i]);
        }
        for (int i = achieves.Length - 1; i >= 0; i--)
        {
            if (AchievesManager.instance.achieves.achievesComplete[i])
            {
                achieves[i].GetComponent<RectTransform>().SetAsFirstSibling();
                toNavigate.Insert(0, achieves[i]);
            }
        }
        StartCoroutine(WaitSeconds());
    }

    public void ResetAchieves()
    {
        AchievesManager.instance.ClearAllAchieves(true);
        RefreshAchieves();
    }

    public void Open()
    {
        RefreshAchieves();
        selectedAchieve = toNavigate[0];
        currentSelection = 0;
        selectedAchieve.SelectAchieve();
        MoveParent();
    }

    void MoveParent()
    {
        float dif = posToSelected.transform.position.y - selectedAchieve.transform.position.y;
        Debug.Log(dif + "/" + selectedAchieve.transform.position.y + "/" + posToSelected.transform.position.y);
        if (bar.value >= 1 && dif < 0) return;
        else if (bar.value <= 0 && dif > 0) return;
        parent.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y + dif, parent.transform.position.z);
    }

    public void Close()
    {
        begintimer = false;
        timer = 0;
    }
    bool begintimer;
    float timer;

    private void Update()
    {
        if (begintimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.3f)
            {
                begintimer = false;
                timer = 0;
            }
            return;
        }

        if (Input.GetButton("Back")) backButton.onClick.Invoke();
        else if (Input.GetButton("Interact")) resetButton.onClick.Invoke();

        float vertical = Input.GetAxis("Vertical");
        int dir = 0;
        if (vertical > 0.5f || vertical < -0.5f)
        {
            dir = vertical > 0 ? -1 : 1;
        }

        if (dir != 0 && dir + currentSelection >= 0 && dir + currentSelection < achieves.Length)
            ChangeItemSelect(currentSelection + dir);
    }

    void ChangeItemSelect(int dir)
    {
        begintimer = true;
        selectedAchieve.UnselectAchieve();
        currentSelection = dir;
        selectedAchieve = toNavigate[currentSelection];
        selectedAchieve.SelectAchieve();

        MoveParent();
    }
}
