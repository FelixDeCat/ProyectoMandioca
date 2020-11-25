using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvents_Manager : MonoBehaviour
{

    [SerializeField] Brazalete_event brazaletEvent;

    Dictionary<IScriptedEvent, bool> eventRegistry = new Dictionary<IScriptedEvent, bool>();

    public int debug;

    private void Start()
    {
        Main.instance.SetScriptedEventManager(this);
    }

    public void CheckEvent(IScriptedEvent evento)
    {
        if (eventRegistry.ContainsKey(evento))
            eventRegistry[evento] = true;
        else  Debug.LogError("No esta este evento en el registry"); 
    }

    public void RegisterEvents(IScriptedEvent evento)
    {

        if (!eventRegistry.ContainsKey(evento))
        {
            Debug.Log("registro " + evento.ToString());
            eventRegistry.Add(evento, false);
        }
            
    }

    public void ResetEvents()
    {
        Debug.Log("Reseteo eventos");
        foreach (var item in eventRegistry)
        {
            if(!item.Value)
                item.Key.ResetEvent();
        }
    }

    private void Update()
    {
        debug = eventRegistry.Count;
    }
}
