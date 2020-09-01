using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkingEntity : EntityBase
{
    protected override void OnUpdate() { if (executeAStar) {/*Execute AStar*/}  OnUpdateEntity(); }
    protected abstract void OnUpdateEntity();
    public virtual void OnReceiveItem(InteractCollect itemworld) { }
    public virtual void OnStun() { }

    #region en desuso
    /// <summary>
    /// aca se puede implementar un A* desactivado
    /// </summary>
    /// 
    private bool executeAStar;
    protected void Callback_IHaveArrived() { /*llegue a mi posicion, por callback*/ executeAStar = false; }
    public void GoToPosition(Transform pos) { /*Configure and Active AStar*/executeAStar = true; }
    public void GoToPosition(Vector2 pos) { /*Configure and Active AStar*/ executeAStar = true; }
    public void GoToPosition() { /*Configure and Active AStar*/ executeAStar = true; }
    #endregion
}
