using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class AccionActive : Interactable
{
    [Multiline(5)]
    public string message;
    public string tiitle;
    public string action;

    public ActionRealize action_to_realize;

    private void Awake()
    {
        //lookat.SetSource(0, Camera.main.gameObject.GetComponent<ConstraintSource>());
    }

    public override void OnExecute(WalkingEntity entity)
    {
        action_to_realize.Excecute();
    }

    public override void OnExit()
    {
        
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(this.transform.position, tiitle, message, action);
    }
    
}
