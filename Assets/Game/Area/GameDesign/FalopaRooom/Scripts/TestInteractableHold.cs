using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestInteractableHold : Interactable
{
    [SerializeField] UnityEvent execute = null;
    public bool destroy = true;
    public UnityEvent customDestroy;
    public UnityEvent UE_EndDelayExecute;
    public Sprite image_to_interact;

    public void ExecuteBool(bool b) => executing = b;

    public string actionName = "hold to grab";

    bool oneshot;

    private void Start()
    {
        _executeAction += OnEndDelayExecute;

        SetPredicate(() => !executing);
    }
    public override void OnEnter(WalkingEntity entity)
    {
        ContextualBarSimple.instance.Show();
        //ContextualBarSimple.instance.Set_Sprite_Photo(image_to_interact);
        ContextualBarSimple.instance.Set_Sprite_Button_Custom(InputImageDatabase.InputImageType.interact);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        collector.GetComponentInChildren<InteractSensor>()?.Dissappear(this);
        if (!oneshot)
        {
            oneshot = true;
            _executeAction();
            
        }
    }

    public override void OnInterrupt()
    {
        oneshot = false;
        ContextualBarSimple.instance.Set_Values_Load_Bar(delayTime, 0);
    }

    public override void OnExit(WalkingEntity collector)
    {
        oneshot = false;
        ContextualBarSimple.instance.Hide();
    }
    public override void DelayExecute()
    {
        base.DelayExecute();
        ContextualBarSimple.instance.Set_Values_Load_Bar(delayTime, currentTime);
    }
    void OnEndDelayExecute()
    {
        UE_EndDelayExecute.Invoke();
        execute?.Invoke();
        if (destroy) Destroy(this.gameObject);
        else
        {
            customDestroy.Invoke();
        }
    }

}
