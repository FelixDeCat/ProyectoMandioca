using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_InventoryBase : UI_Base
{
    [SerializeField] public List<UI_ItemBase> ui_items;
    public InventoryHandler inventoryHandler;
    public GameObject feedbackSelectedInventory;

    public abstract bool Contains(UI_ItemBase itembase );

    protected abstract override void OnAwake();
    public abstract void Refresh(itmAction _itmAction);
    public abstract override void Refresh();
    protected abstract override void OnStart();
    protected abstract override void OnEndOpenAnimation();
    protected abstract override void OnEndCloseAnimation();
    protected abstract override void OnUpdate();
}
