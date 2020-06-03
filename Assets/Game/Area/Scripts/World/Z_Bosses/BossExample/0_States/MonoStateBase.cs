using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStateBase : MonoBehaviour
{
    [System.Serializable]
    public class MonoStateBaseOptions { public LabelStatesLinkType linker; }
    public MonoStateBaseOptions _monoStateBaseOptions = new MonoStateBaseOptions();

    SensorsAndBehaviours sensors_and_behaviours;
    FastSubscriberPerState myfastSubscriber;
    protected SensorManager Get_Sensors { get { return sensors_and_behaviours.Sensor; } }
    protected BehavioursManager Get_Behaviours { get { return sensors_and_behaviours.Behaviours; } }
    protected FastSubscriberPerState Get_Anim_Event_Subscriber { get { return myfastSubscriber; } }
    public void Configure(SensorsAndBehaviours _sensorsAndBehaviours, FastSubscriberPerState fastSubscriber)
    {
        sensors_and_behaviours = _sensorsAndBehaviours;
        myfastSubscriber = fastSubscriber;

        OnOneAwake();
    }

    private void Awake()
    {
        #region comprobacion para saber si otro estado esta usando el mismo linker que yo
        var myparent = this.transform.parent;
        var nakamas = myparent.GetComponentsInChildren<MonoStateBase>();
        foreach (var state in nakamas)
        {
            if (state == this) continue;
            if (state._monoStateBaseOptions.linker == _monoStateBaseOptions.linker) throw new System.Exception("ERROR::: Hay un linker que se esta repitiendo, corregir de inmediato:::");
        }
        #endregion
    }

    public void Begin()
    {
        DebugCustom.Log("WENDIGO", "State", gameObject.name);
        OnBegin();
    }

    public void Exit()
    {
        OnExit();
    }

    public void Refresh()
    {
        OnUpdate();
    }

    protected abstract void OnOneAwake();
    protected abstract void OnUpdate();
    protected abstract void OnBegin();
    protected abstract void OnExit();
}
