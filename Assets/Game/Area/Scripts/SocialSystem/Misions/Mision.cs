using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Mision", menuName = "RPG/Misions/Mision", order = 10)]
[System.Serializable]
public class Mision: ScriptableObject
{
    [Multiline(2)] public string mision_name;
    public int id_mision;
    public string id_command;
    [SerializeField] internal bool AutoEnd = true;
    [SerializeField] bool isHided = false;
    public Misions.Core.Serializable_DescriptiveInfo info;
    public Misions.Core.Serializable_MisionData data;
    
    public Misions.Core.Serializable_Reward rewarding;
    public bool Completed { get { return data.Completed; } }
    internal bool IsHided { get { return isHided; } }
    public Action<int> mision_end_callback;
    public Action mision_end_callback_simple;
    public Action Callback_Refresh;

    public override string ToString()
    {
        return "chacha";
    }

    public void Begin(Action Refresh)
    {
        foreach (var mi in data.MisionItems) mi.SubscribeTo_ItemSelfUpdate(OnRefresh);
        data.ActivateMision();
        Callback_Refresh = Refresh;
    }
    public void AddCallbackToEnd(Action<int> callbackToEnd)
    {
        mision_end_callback += callbackToEnd;
    }
    public void AddCallbackToEnd(Action callbackToEnd)
    {
        mision_end_callback_simple += callbackToEnd;
    }

    public void End() => data.DeactivateMision();

    public void OnRefresh()
    {
        if (AllItemsFinished()) Finish();
        Callback_Refresh.Invoke();
    }

    public bool CanFinishMision()
    {
        return AllItemsFinished();
    }

    bool AllItemsFinished()
    {
        for (int i = 0; i < data.MisionItems.Length; i++)
        {
            //data.MisionItems[i].CheckMemory(id_mision, i);

            if (!data.MisionItems[i].IsCompleted) return false;
            else continue;
        }
        return true;
    }

    protected void Finish()
    { 
        mision_end_callback.Invoke(id_mision);
        try
        {
            mision_end_callback_simple.Invoke();
        }
        catch { Debug.LogWarning("No tiene linkeado un Fase Handler"); }
        
    }
}

namespace Misions.Core
{
    using UnityEngine.Events;

    #region [ CORE ] Serializable Item Mision
    #region INFO
    [System.Serializable]
    public class Serializable_DescriptiveInfo
    {
        [Multiline(10)] public string description;
        [Multiline(5)] public string finish_message;
    }
    #endregion
    #region DATA
    [System.Serializable]
    public class Serializable_MisionData
    {
        public bool completed = false;
        public bool isactive = false;
        
        public bool delivered;
        [SerializeField] ItemMision[] mision_item = new ItemMision[0];
        
        string[] regions_to_enable =  new string[1];
        internal bool Completed { get { return completed; } }
        internal bool IsActive { get { return isactive; } }
        
        internal string[] Regions { get { return regions_to_enable; } }
        internal ItemMision[] MisionItems { get { return mision_item; } }
        internal bool Delivered { get { return delivered; } }
        internal string ItemsCompleteString()
        {
            string aux = "";
            for (int i = 0; i < mision_item.Length; i++)
            {
                if (i < mision_item.Length - 1)
                {
                    aux += mision_item[i].ToString() + "\n";
                }
                else
                {
                    aux += mision_item[i].ToString();
                }
            }
            return aux;
        }
        internal void SetItemsMision(ItemMision[] items) => mision_item = items;
        internal void ModifyItemMision(int index, int cant = 1, string _name = "default")
        {
            if (index < mision_item.Length)
            {
                mision_item[index].SetCurrentValue(cant);
                mision_item[index].SetName(_name);
            }
        }
        internal void ActivateMision() => isactive = true;
        internal void DeactivateMision() => isactive = false;
        internal void SetCompletedMision() => completed = true;
        internal void SetDeliveredMision() => delivered = true;
        
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
        [SerializeField] internal ItemInInventory[] items_rewarding = new ItemInInventory[0];
    }
    #endregion
    #endregion


}