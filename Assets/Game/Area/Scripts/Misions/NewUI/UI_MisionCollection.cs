using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class UI_MisionCollection : MonoBehaviour
{
    protected List<Mision> misions = new List<Mision>();
    public UI_MissionButtonElement model;
    public RectTransform parent;
    public abstract void Refresh(List<Mision> misions, Action<int> callbackselection);
}
