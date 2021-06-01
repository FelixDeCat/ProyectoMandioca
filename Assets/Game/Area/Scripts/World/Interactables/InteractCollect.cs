using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;
using System;
//implementar items
//implementar collector

public class InteractCollect : Interactable
{
    [Header("Item World Setup")]
    public Item item;

    Item_animRecolect recolector_anim;
    public bool canrecolectoranim;

    public UnityEvent to_collect;
    public UnityEvent OnCreate;
    [SerializeField] AudioClip _feedBack = null;
    protected bool destroy_on_collect = true;

    private void Awake()
    {
        recolector_anim = GetComponent<Item_animRecolect>();
        if (recolector_anim != null) canrecolectoranim = true;
    }
    private void Start()
    {
        if (_feedBack)
            AudioManager.instance.GetSoundPool(_feedBack.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _feedBack);
    }
    public void OnAppearInScene()
    {
        OnCreate.Invoke();
    }

    ///////////////////////////////////////////////////////////////////
    ///// PROPIAS DE INTERACTABLE (HERENCIA)
    ///////////////////////////////////////////////////////////////////
    public override void OnExecute(WalkingEntity collector)
    {
        if (canrecolectoranim)
        {
            recolector_anim.BeginRecollect(collector, Collect);
        }
        else
        {
            Collect(collector);
        }
    }

    Action<WalkingEntity> callbackCollect;
    protected virtual void CollectOnEndAnimation(WalkingEntity walkingEnt, Action<WalkingEntity> callback)
    {
        recolector_anim.BeginRecollect(walkingEnt, callback);
    }

    void Collect(WalkingEntity collector)
    {
        if (_feedBack)
            AudioManager.instance.PlaySound(_feedBack.name, transform);
        collector.OnReceiveItem(this);
        to_collect.Invoke();
        if (destroy_on_collect) Destroy(this.gameObject);
    }

    public override void OnExit(WalkingEntity collector)
    {
        WorldItemInfo.instance.Hide();
    }

    public override void OnEnter(WalkingEntity entity)
    {
        if (!autoexecute)
        {
            if (item)
            {
                WorldItemInfo.instance.Show(
                    pointToMessage != null ? pointToMessage.position: this.transform.position, 
                    item.name, 
                    item.description, 
                    "agarrar", 
                    false, 
                    true);
            }
        }
        else
        {
            //para el auto execute
            Execute(entity);
        }

    }

    public override void OnInterrupt()
    {
    }

}
