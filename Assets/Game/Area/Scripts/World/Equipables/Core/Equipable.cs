using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoScripts.Core;

public abstract class Equipable : MonoBehaviour, IPausable, IResumeable
{
    [Header("[no tocar] Tipo de Lugar")]
    public SpotType spot_type;

    [Header("Aca va el scriptable object")]
    public Item item;

    bool equiped;
    bool canUpdateequipation;
    public bool Equiped { get { return equiped; } }
    public virtual void Equip() { OnEquip(); equiped = true; canUpdateequipation = true; }
    public virtual void UnEquip() { OnUnequip(); equiped = false; canUpdateequipation = false; }
    protected virtual void Update() { if (canUpdateequipation) OnUpdateEquipation(); }
    protected abstract void OnEquip();
    protected abstract void OnUnequip();
    protected abstract void OnUpdateEquipation();
    public virtual void Pause() { canUpdateequipation = false; }
    public virtual void Resume() { canUpdateequipation = true; }
}
