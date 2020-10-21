using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

[SerializeField]
public abstract class Goap_Action_rooster
{
    public abstract List<GoapAction> PosibleActions();
    public abstract Dictionary<string, ItemType> DicItem();
}

