using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class GoldHandler : MonoBehaviour
{
    public static GoldHandler instance;

    [SerializeField] Gold gold = null;

    [SerializeField] ContextualMenu contextual = null;
    [SerializeField] UI_Gold ui_gold = null;
    [SerializeField] Animator MyAnim = null;

    SimpleTimer timer;



    private void Awake()
    {
        instance = this;
        timer = new SimpleTimer();
        gold = new Gold(999999999,0);
        OverrideQuantity(0);
        gold.SubscribeToOnChangeValue(ChangeValue);
        
    } 

    void ChangeValue(int val)
    {
        if (!contextual.IsActive)
        {
            contextual.Open();
        }

        MyAnim.Play("UI_GoldCollect");
        ui_gold.SetGold(val);

        timer.Begin(1f,CloseContextual);
    }

    void CloseContextual()
    {
        contextual.Close();
    }

    private void Update()
    {
        timer.Refresh();
    }

    public void AddCollect() => gold.IncreaseValue(1);
    public void AddCollect(int val) => gold.IncreaseValue(val);
    public void OverrideQuantity(int val) => gold.SetValue(val);
}
