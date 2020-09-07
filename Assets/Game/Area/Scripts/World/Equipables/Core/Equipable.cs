using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoScripts.Core;

public abstract class Equipable : MonoBehaviour, IPausable, IResumeable
{
    bool equiped;
    bool canUpdateequipation;
    public bool Equiped { get { return equiped; } }
    public virtual void Equip() { equiped = true; canUpdateequipation = true; }
    public virtual void UnEquip() { equiped = false; canUpdateequipation = false; }
    protected virtual void Update() { if (canUpdateequipation) OnUpdateEquipation(); }
    protected abstract void OnUpdateEquipation();
    public virtual void Pause() { canUpdateequipation = false; }
    public virtual void Resume() { canUpdateequipation = true; }
}
