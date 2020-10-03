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
    bool executing;

    public void ExecuteBool(bool b) => executing = b;

    public string actionName = "hold to grab";

    [SerializeField] Image _loadBar = null;
    private void Start()
    {
        _executeAction += DestroyGameObject;
        _executeAction += UE_EndDelayExecute.Invoke;
        SetPredicate(() => !executing);
    }
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Object", "Hold to grab object", actionName, false, false);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        _executeAction();
        WorldItemInfo.instance.Hide();
    }

    public override void OnInterrupt()
    {
        _loadBar.fillAmount = 0;
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
        _loadBar.fillAmount = 0;
    }
    public override void DelayExecute(float loadTime)
    {
        base.DelayExecute(loadTime);
        float amount = (currentTime / delayTime);
        _loadBar.fillAmount=amount;
    }
    void DestroyGameObject()
    {
        execute?.Invoke();
        if (destroy) Destroy(this.gameObject);
        else
        {
            customDestroy.Invoke();
        }
    }

}
