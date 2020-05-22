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

    public virtual void Equip() => OnEquip();
    public virtual void UnEquip() => OnUnequip();

    protected abstract void OnEquip();
    protected abstract void OnUnequip();
}
