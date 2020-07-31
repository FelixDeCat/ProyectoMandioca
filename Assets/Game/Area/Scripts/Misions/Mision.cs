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
        [SerializeField] Serializable_ItemMision[] mision_item = new Serializable_ItemMision[0];
        [SerializeField] string[] regions_to_enable;
        internal int[] Progression { get { return progression; } }
        internal bool Completed { get { return completed; } }
        internal bool IsActive { get { return isactive; } }
        internal Serializable_ItemMision[] MisionItem { get { return mision_item; } }
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
    internal class Serializable_BoolItemMision : Serializable_ItemMision
    {
        [SerializeField] private bool iscompleted;
        internal bool IsCompleted { get { return iscompleted; } }
        public Serializable_BoolItemMision(string _description, bool _isCompleted) : base(_description)
        {
            iscompleted = _isCompleted;
        }
    }
    [System.Serializable]
    internal class Serializable_ByteItemMision : Serializable_ItemMision
    {
        [SerializeField] private byte currentvalue;
        [SerializeField] private byte maxvalue;
        internal int CurrentValue { get { return (int)currentvalue; } }
        internal int MaxValue { get { return (int)maxvalue; } }

        public Serializable_ByteItemMision(string _description, int _currentValue, int _maxValue) : base(_description)
        {
            currentvalue = (byte)_currentValue;
            maxvalue = (byte)_maxValue;
        }
    }
    [System.Serializable]
    internal class Serializable_ItemMision
    {
        [SerializeField] protected string description;
        internal string Description { get { return description; } }
        public Serializable_ItemMision(string _description)
        {
            this.description = _description;
        }
    }
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