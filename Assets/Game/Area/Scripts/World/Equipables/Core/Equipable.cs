using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Equipable : MonoBehaviour
{
    [Header("[no tocar] Tipo de Lugar")]
    public SpotType spot_type;

    [Header("Aca va el scriptable object")]
    public Item item;

    bool equiped;
    public bool Equiped { get { return equiped; } }

    public virtual void Equip() { OnEquip(); equiped = true; }
    public virtual void UnEquip() { OnUnequip(); equiped = false; }

    protected abstract void OnEquip();
    protected abstract void OnUnequip();
}
