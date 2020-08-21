using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestInteractableHold : Interactable
{
    [SerializeField] UnityEvent execute;

    [SerializeField] Image _loadBar;
    private void Start()
    {
        _executeAction += DestroyGameObject;
    }
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Object", "Hold to grab object", "Hold to grab", false, false);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        _executeAction();
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
        Destroy(this.gameObject);
    }

}
