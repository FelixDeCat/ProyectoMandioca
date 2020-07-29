using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mision : MonoBehaviour
{
    
    public int id_mision;
    [SerializeField] public int[] progression;

    [Multiline(3)] public string mision_name;
    [Multiline(10)] public string description;
    [Multiline(5)] public string subdescription;

    public ItemInInventory[] recompensa;

    public bool alreadyconfigured;

    public Mision next;
    public bool completed = false;
    public bool isactive;

    public abstract void PermanentConfigurations();
    private void Start() {
        if(!alreadyconfigured) ConfigureProgresion();
    }
    public abstract void Begin();
    public abstract void End();
    public abstract void Refresh();
    public abstract void OnUpdate();
    public abstract void Failed();

    public abstract void CheckProgresion();
    public abstract void ConfigureProgresion();
    public abstract void SetProgresion(int[] prog);

}
