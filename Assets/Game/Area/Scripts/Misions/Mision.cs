using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Mision : MonoBehaviour
{
    public int id_mision;

    public Misions.Core.Serializable_DescriptiveInfo info;
    public Misions.Core.Serializable_MisionData data;
    public Misions.Core.Serializable_WorldTracking[] currenttracking;
    public Misions.Core.Serializable_Reward rewarding;
    public Misions.Core.Serializable_Events events;

    public bool Completed { get { return data.Completed; } }

    bool canupdate;

    public Action<Mision> mision_end_callback;
    
    private void Start()
    {
        ConfigureProgresion();
    }
    public void Begin(Action<Mision> _mision_end_callback)
    {
        data.SetItemsMision(GetComponentsInChildren<ItemMision>());
        foreach (var mi in data.MisionItems) mi.SubscribeTo_ItemSelfUpdate(OnRefresh);
        data.ActivateMision();
        canupdate = true;
        OnBegin();
        mision_end_callback = _mision_end_callback;
        events.OnStartMission.Invoke();
    }
    public void End()
    {
        data.DeactivateMision();
        canupdate = false;
        OnEnd();
        events.OnEndMission.Invoke();
    }

    public void OnRefresh()
    {
        //un item me avisó que acaba de ser modificado
    }
    

    private void Update()
    {
        if (canupdate)
        {
            OnUpdate();
        }
    }

    protected void Finish()
    {
        mision_end_callback.Invoke(this);
    }

    public abstract void Refresh();
    public abstract void OnFailed();
    public abstract void CheckProgresion();
    public abstract void ConfigureProgresion();
    public abstract void SetProgresion(int[] prog);
    public abstract void PermanentConfigurations();

    protected abstract void OnBegin();
    protected abstract void OnEnd();
    protected abstract void OnUpdate();
}

public class QuestFile
{

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
        [Multiline(5)] public string subdescription;
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
    [System.Serializable]
    public class Serializable_Events
    {
        [SerializeField] internal UnityEvent OnStartMission;
        [SerializeField] internal UnityEvent OnEndMission;
    }
    #endregion
    #endregion
    public  class Serializable_WorldTracking
    {
        internal string scene = "";
        internal Vector3 position = new Vector3(0, 0, 0);
    }

}