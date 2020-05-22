using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public abstract class UI_ItemBaseButton : Button
{
    public Image mainImage;
    public Text cant;
    [System.NonSerialized] public int id;
    [System.NonSerialized] int index;
    [System.NonSerialized] public bool selected;
    public Shadow outline;

    float cantspeedscale = 5;
    float cantidadDeEscala = 1.4f;

    public PingPongLerp pingpongScale;
    Vector3 currentscale;
    Vector3 amplitudDeEscala;
    
    public RectTransform rect;

    protected Color SelectedColor;

    Action<int> OnUI_Selected;
   
    
    //Animacion bonita cuando tengo OnHover o cuando lo selecciono
    public void Anim_Scale() => pingpongScale.Play(cantspeedscale);
    void Update()
    {
        /// <summary>
        /// MUCHO OJOOOO, este update lo esta ejecutando EXECUTE IN EDIT MODE!!!
        /// recordar que esto esta heredando de Selectable, no deberiamos hacer esto pero bue... :D
        /// </summary>
        if (pingpongScale != null) pingpongScale.Updatear();
    }

    //sobreescritura del OnSelect
    //esto es para que funque nomas la selection
    //es de Unity, solo ignoralo
    public override void OnSelect(BaseEventData eventData)
    {
        OnUI_Selected(index);
        base.OnSelect(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Debug.Log("pointer down");
        
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        Debug.Log("Submit");
    }
    protected abstract void BeginFeedback();
    protected abstract void EndFeedback();

    

    ////////////////////////////////////////////////////////////////////
    //// BUILDER CONFIGURATIONS
    ////////////////////////////////////////////////////////////////////
    public UI_ItemBaseButton Initialize(int _index, Action<int> _OnSelect)
    {
        index = _index;

        rect = GetComponent<RectTransform>();
        pingpongScale = new PingPongLerp();
        currentscale = rect.localScale;
        rect.localScale = new Vector3(currentscale.x, currentscale.y, 1);
        amplitudDeEscala = currentscale * cantidadDeEscala;
        pingpongScale.Configure(AnimLerpScale, false, true);

        OnUI_Selected = _OnSelect;

        return this;
    }
    public UI_ItemBaseButton SetId(int id) { this.id = id; return this; }
    public UI_ItemBaseButton SetImage(Sprite sp) { mainImage.sprite = sp; return this; }
    public UI_ItemBaseButton SetCant(int c) { cant.enabled = c <= 0 ? false : true; cant.text = c.ToString(); return this; }


    ////////////////////////////////////////////////////////////////////
    //// SELECTION & UNSELECTION
    ////////////////////////////////////////////////////////////////////
    public void OnUI_Select()
    {
        
        Draw(new Color(1, 1, 1, 1), new Color(0, 0, 0, 1), new Vector3(1.3f, 1.3f, 1.3f));
        BeginFeedback();
    }
    public void OnUI_Unselect()
    {
        if (mainImage.sprite != null)  Draw(new Color(0.25f, 0.25f, 0.25f, 1), new Color(0, 0, 0, 0), new Vector3(0.9f, 0.9f, 0.9f));
        else Draw(new Color(0.5f, 0.5f, 0.5f, 0), new Color(0, 0, 0, 0), new Vector3(0.9f, 0.9f, 0.9f));
        EndFeedback();
    }

    #region draw
    void Draw(Color _main, Color _outline, Vector3 _scale)
    {
        if (mainImage) mainImage.color = _main;
        if (outline) outline.effectColor = _outline;
        if (mainImage) mainImage.gameObject.transform.localScale = _scale;
    }
    void Draw(Color _main, Vector3 _scale)
    {
        if (mainImage) mainImage.color = _main;
        if (mainImage) mainImage.gameObject.transform.localScale = _scale;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////
    //// PING PONG LERP ( FUNCTION CALLBACK RESULT )
    ////////////////////////////////////////////////////////////////////
    void AnimLerpScale(float anim)
    {
        transform.localScale = Vector3.Lerp(currentscale, amplitudDeEscala, anim);
        mainImage.color = Color.Lerp(Color.white, SelectedColor, anim);
    }

    ////////////////////////////////////////////////////////////////////
    //// OLD SYSTEM REFRESH
    ////////////////////////////////////////////////////////////////////
    #region sistema viejo de inventario

    //public virtual void Refresh(ItemInInventory item)
    //{
    //    if (item.item != null)
    //    {
    //        if (item.item.id != -1)
    //        {
    //            SetId(item.item.id);
    //            SetImage(item.item.img);
    //            SetCant(item.cant);
    //        }
    //        else
    //        {
    //            SetId(-1);
    //            SetImage(ManagerInventories.instancia.inventory_empty_icon);
    //            SetCant(0);
    //        }
    //    }
    //    else
    //    {
    //        SetId(-1);
    //        SetImage(ManagerInventories.instancia.inventory_empty_icon);
    //        SetCant(0);
    //    }
    //}
    #endregion
}
