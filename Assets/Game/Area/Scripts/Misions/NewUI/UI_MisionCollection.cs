using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class UI_MisionCollection : MonoBehaviour
{
    public UI_MissionButtonElement model;
    public List<UI_MissionButtonElement> elements = new List<UI_MissionButtonElement>();
    public RectTransform parent;
    public abstract void Refresh(List<Mision> misions, Action<int> callbackselection);
}
