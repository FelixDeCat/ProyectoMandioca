using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class DungeonBoss_event : MonoBehaviour, IScriptedEvent
{
    [SerializeField] GameObject beto = null;

    [SerializeField] GameObject trigger = null;

    //Reset settings
    Vector3 originialBeto_pos;

    void Start()
    {

    }

    void ResetSettings()
    {
        originialBeto_pos = beto.transform.position;
    }

    public void InitBossFight()
    {
        Main.instance.GetScriptedEventManager().RegisterEvents(this);
        ResetSettings();
    }

    public void ResetEvent()
    {
        Debug.Log("Reset dungeon boss");
        trigger.SetActive(true);
        beto.transform.position = originialBeto_pos;
        beto.GetComponent<Ente>().ResetEnte();
    }
}
