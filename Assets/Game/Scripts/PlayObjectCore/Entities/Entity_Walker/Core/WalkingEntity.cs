﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkingEntity : EntityBase
{
    /// <summary>
    /// aca se puede implementar un A* desactivado
    /// </summary>
    /// 

    private void Awake()
    {
        
    }

    private bool executeAStar;
    protected override void OnUpdate() { if (executeAStar) {/*Execute AStar*/}  OnUpdateEntity(); }
    protected void Callback_IHaveArrived() { /*llegue a mi posicion, por callback*/ executeAStar = false; }
    public void GoToPosition(Transform pos) { /*Configure and Active AStar*/executeAStar = true; }
    public void GoToPosition(Vector2 pos) { /*Configure and Active AStar*/ executeAStar = true; }
    public void GoToPosition() { /*Configure and Active AStar*/ executeAStar = true; }
    protected abstract void OnUpdateEntity();

    public virtual void OnReceiveItem(ItemWorld itemworld) { }
    public virtual void OnStun() { }
    public virtual void OnPetrified() { }
    public virtual void OnFire() { }
    public virtual void HalfLife() { }
}
