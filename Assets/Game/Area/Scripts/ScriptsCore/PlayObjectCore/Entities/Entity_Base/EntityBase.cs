using UnityEngine;
using System;
public abstract class EntityBase : PlayObject 
{
    public virtual void OnReceiveItem(InteractCollect itemworld) { }
}
