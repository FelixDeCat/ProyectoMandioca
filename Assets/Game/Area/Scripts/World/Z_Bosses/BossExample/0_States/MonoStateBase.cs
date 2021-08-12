using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStateBase : MonoBehaviour
{
    
    public MonoStateBaseOptions _monoStateBaseOptions = new MonoStateBaseOptions();
    
    //esto ahora que lo estoy viendo bien... podriamos tener una especie de
    //[ManagersDataCenter]... onda de pasarle una sola cosa por parametro como lo hicimos con DamageData
    //luego en los Getters lo traducimos aca mismo
    SensorsAndBehaviours sensors_and_behaviours;
    FastSubscriberPerState myfastSubscriber;
    InputSenderBase inputSender;
    FeedbackManager feedbackManager;

    protected Rigidbody Get_Rigidbody { get { return sensors_and_behaviours.rigidBody; } }
    protected SensorManager Get_Sensors { get { return sensors_and_behaviours.Sensor; } }
    protected BehavioursManager Get_Behaviours { get { return sensors_and_behaviours.Behaviours; } }
    protected InputSenderBase Get_InputSender { get { return inputSender; } }
    protected FastSubscriberPerState Get_Anim_Event_Subscriber { get { return myfastSubscriber; } }
    protected FeedbackManager Get_FeedbackHandler { get { return feedbackManager; } }
    public void Configure(SensorsAndBehaviours _sensorsAndBehaviours, FastSubscriberPerState fastSubscriber, InputSenderBase _inputSender, FeedbackManager _feedbackManager)
    {
        sensors_and_behaviours = _sensorsAndBehaviours;
        myfastSubscriber = fastSubscriber;
        inputSender = _inputSender;
        feedbackManager = _feedbackManager;

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
            if (state._monoStateBaseOptions.Linker == _monoStateBaseOptions.Linker) throw new System.Exception("ERROR::: Hay un linker que se esta repitiendo, corregir de inmediato:::");
        }
        #endregion
    }

    public void Begin()
    {
        _monoStateBaseOptions.isactive = true;
        //DebugCustom.Log("WENDIGO", "State", gameObject.name);
        OnBegin();
    }

    public void Exit()
    {
        _monoStateBaseOptions.isactive = false;
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


    [System.Serializable]
    public class MonoStateBaseOptions
    {
        [SerializeField] private LabelStatesLinkType linker = LabelStatesLinkType.STATE_01_ACTION;
        internal LabelStatesLinkType Linker { get => linker; }
        [System.NonSerialized] internal bool isactive;
    }
}
