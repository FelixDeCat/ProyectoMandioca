using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievesManager : MonoBehaviour
{
    public static AchievesManager instance { get; private set; }

    [SerializeField] List<Achieves> allAchieves = new List<Achieves>();
    [SerializeField] float timeToReturn = 2;
    AchievesSaveData achieves;
    Queue<Achieves> achievesToShow = new Queue<Achieves>();

    [Header("UI things")]
    [SerializeField] UI_Anim_Code baseAnim = null;
    [SerializeField] TextMeshProUGUI title = null;
    [SerializeField] TextMeshProUGUI desc = null;
    [SerializeField] Image img = null;

    private void Awake()
    {
        instance = this;
        achieves = new AchievesSaveData();
        achieves.achievesComplete = new bool[allAchieves.Count];
        baseAnim.AddCallbacks(() => updating = true, CheckQueue);
    }

    public void CompleteAchieve(string ID)
    {
        Achieves achieveToComplete = null;
        int index = 0;
        for (int i = 0; i < allAchieves.Count; i++)
        {
            if(ID == allAchieves[i].ID)
            {
                achieveToComplete = allAchieves[i];
                index = i;
                break;
            }
        }

        Debug.Log("averga");
        if (achieveToComplete == null) return;

        if (achieves.achievesComplete[index] != true)
        {
            achieves.achievesComplete[index] = true;
            ShowAchieve(achieveToComplete);
        }
    }

    public void CompleteAchieve(Achieves achieveToComplete)
    {
        if (allAchieves.Contains(achieveToComplete))
        {
            int index = allAchieves.IndexOf(achieveToComplete);
            if (achieves.achievesComplete[index] != true)
            {
                achieves.achievesComplete[index] = true;
                ShowAchieve(achieveToComplete);
            }
        }
    }

    public void ShowAchieve(Achieves achieve)
    {
        if (showing)
        {
            achievesToShow.Enqueue(achieve);
        }
        else
            OpenUI(achieve);
    }

    void CheckQueue()
    {
        showing = false;
        if (achievesToShow.Count > 0)
            OpenUI(achievesToShow.Dequeue());
    }

    void OpenUI(Achieves achieve)
    {
        Debug.Log("?");
        title.text = achieve.title;
        desc.text = achieve.description;
        img.sprite = achieve.achiveImg;
        baseAnim.Open();
        showing = true;
    }

    bool showing = false;
    bool updating = false;
    float timer = 0;

    private void Update()
    {
        if (!updating) return;

        timer += Time.deltaTime;

        if (timer >= timeToReturn)
        {
            timer = 0;
            updating = false;
            baseAnim.Close();
        }
    }
}
