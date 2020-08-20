using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ExecutableBase : MonoBehaviour
{
    int id = -1;
    [SerializeField] string info;
    public int ID { get { return id; } }
    public void SetID(int id) { this.id = id; }
    public string GetInfo() => info;
    public void Execute() => OnExecute();
    public bool CanExecute => OnCanExecute();
    protected abstract void OnExecute();
    protected abstract bool OnCanExecute();
}
