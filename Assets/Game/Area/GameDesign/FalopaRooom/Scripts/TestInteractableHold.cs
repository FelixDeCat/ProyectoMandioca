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
    bool executing;

    public void ExecuteBool(bool b) => executing = b;

    public string actionName = "hold to grab";

    bool oneshot;

    [SerializeField] Image _loadBar = null;
    private void Start()
    {
        _executeAction += OnEndDelayExecute;

        SetPredicate(() => !executing);
    }
    public override void OnEnter(WalkingEntity entity)
    {
        //if (!string.IsNullOrEmpty(actionName)) WorldItemInfo.instance.Show(pointToMessage.position, "Object", "Hold to grab object", actionName, true, false);

        ContextualBarSimple.instance.Show();
        ContextualBarSimple.instance.Set_Sprite_Photo(image_to_interact);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        if (!oneshot)
        {
            oneshot = true;
            _executeAction();
            ContextualBarSimple.instance.Hide();
            // WorldItemInfo.instance.Hide();
        }
    }

    public override void OnInterrupt()
    {
        oneshot = false;
        _loadBar.fillAmount = 0;
        ContextualBarSimple.instance.Hide();
    }

    public override void OnExit()
    {
        oneshot = false;
       // WorldItemInfo.instance.Hide();
        ContextualBarSimple.instance.Hide();
        _loadBar.fillAmount = 0;
    }
    public override void DelayExecute()
    {
        base.DelayExecute();

        ContextualBarSimple.instance.Set_Values_Load_Bar(delayTime, currentTime);

        float amount = (currentTime / delayTime);
        _loadBar.fillAmount = amount;
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
