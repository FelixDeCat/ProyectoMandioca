using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AutoShutDownOnBuild : MonoBehaviour
{

    public UnityEvent OnEnable;
    public UnityEvent OnDisable;
    bool en;

    void Start()
    {
#if UNITY_STANDALONE
        OnDisable.Invoke();
        en = false;
#endif
#if UNITY_EDITOR
        OnEnable.Invoke();
        en = true;
#endif

        Invoke("TurnOff", 0.1f);
    }

    void TurnOff()
    {
        en = false;
        OnDisable.Invoke();
        Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);
        if (DevelopToolsCenter.instance) DevelopToolsCenter.instance.ShowChilds(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
        {
            en = !en;
            if (en)
            {
                OnEnable.Invoke();
                Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);
                if (DevelopToolsCenter.instance) DevelopToolsCenter.instance.ShowChilds(true);
            }
            else
            {
                OnDisable.Invoke();
                Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);
                if (DevelopToolsCenter.instance) DevelopToolsCenter.instance.ShowChilds(false);
            }
        }
    }
}
