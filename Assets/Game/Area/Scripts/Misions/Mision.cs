using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mision : MonoBehaviour
{
    public int id_mision;

    public Misions.Core.Serializable_DescriptiveInfo info;
    public Misions.Core.Serializable_MisionData data;

    public bool Completed { get { return data.Completed; } }

    public ItemInInventory[] recompensa;

    public Mision next;

    bool canupdate;

    
    private void Start()
    {
        ConfigureProgresion();
    }
    public void Begin()
    {
        data.ActivateMision();
        canupdate = true;
        OnBegin();
    }
    public void End()
    {
        data.DeactivateMision();
        canupdate = false;
        OnEnd();
    }
    

    private void Update()
    {
        if (canupdate)
        {
            OnUpdate();
        }
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

namespace Misions.Core
{
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
        internal int[] Progression { get { return progression; } }
        internal bool Completed { get { return completed; } }
        internal bool IsActive { get { return isactive; } }
        internal Serializable_ItemMision[] MisionItem { get { return mision_item; } }
        internal void ActivateMision() => isactive = true;
        internal void DeactivateMision() => isactive = false;
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
    #endregion
    #endregion

}