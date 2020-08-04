﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Mision
{
    public int id_mision;
    public Misions.Core.Serializable_DescriptiveInfo info;
    public Misions.Core.Serializable_MisionData data;
    public Misions.Core.Serializable_Reward rewarding;
    public bool Completed { get { return data.Completed; } }
    public Action<Mision> mision_end_callback;
    public Action Callback_Refresh;
    

    public void Begin(Action<Mision> _mision_end_callback, Action Refresh)
    {
        foreach (var mi in data.MisionItems) mi.SubscribeTo_ItemSelfUpdate(OnRefresh);
        data.ActivateMision();
        mision_end_callback = _mision_end_callback;
        Callback_Refresh = Refresh;
    }
    public void End() => data.DeactivateMision();

    public void OnRefresh()
    {
        if (AllItemsFinished()) Finish();
        Callback_Refresh.Invoke();
    }

    bool AllItemsFinished()
    {
        foreach (var mi in data.MisionItems)
        {
            if (!mi.IsCompleted) return false;
            else continue;
        }
        return true;
    }

    protected void Finish() => mision_end_callback.Invoke(this);
}

namespace Misions.Core
{
    using UnityEngine.Events;

    #region [ CORE ] Serializable Item Mision
    #region INFO
    [System.Serializable]
    public class Serializable_DescriptiveInfo
    {
        [Multiline(3)] public string mision_name;
        [Multiline(10)] public string description;
    }
    #endregion
    #region DATA
    [System.Serializable]
    public class Serializable_MisionData
    {
        [SerializeField] int[] progression = new int[0];
        [SerializeField] bool completed = false;
        [SerializeField] bool isactive = false;
        [SerializeField] ItemMision[] mision_item = new ItemMision[0];
        [SerializeField] string[] regions_to_enable;
        internal int[] Progression { get { return progression; } }
        internal bool Completed { get { return completed; } }
        internal bool IsActive { get { return isactive; } }
        internal string[] Regions { get { return regions_to_enable; } }
        internal ItemMision[] MisionItems { get { return mision_item; } }
        internal string ItemsCompleteString()
        {
            string aux = "";
            for (int i = 0; i < mision_item.Length; i++)
            {
                if (i < mision_item.Length - 1)
                {
                    aux += i + ". "+  mision_item[i].ToString() + "\n";
                }
                else
                {
                    aux += i + ". " + mision_item[i].ToString();
                }
            }
            return aux;
        }
        internal void SetItemsMision(ItemMision[] items) => mision_item = items;
        internal void ActivateMision() => isactive = true;
        internal void DeactivateMision() => isactive = false;
        
        internal bool CanPrint(string place)
        {
            foreach (var p in regions_to_enable)
            {
                if (p == place)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
    #region DATA_CORE
    
    [System.Serializable]
    public class Serializable_Reward
    {
        [SerializeField] internal ItemInInventory[] items_rewarding;

    }
    #endregion
    #endregion


}