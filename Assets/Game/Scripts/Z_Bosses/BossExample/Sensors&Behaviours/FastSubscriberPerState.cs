using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AnimEventLabel
{
    Boss_StopLookAt,
    Boss_TakeRock,
    Boss_Throw,
    Boss_EndLoadHeavyAttack,
    Boss_HitTheFloor,
    Boss_Close_Melee,
    Boss_Close_Melee_End
}

public class FastSubscriberPerState : MonoBehaviour
{
    public AnimEvent animevent;
    List<Tuple<AnimEventLabel, Action>> actions = new List<Tuple<AnimEventLabel, Action>>();

    public void SubscribeMeTo(AnimEventLabel eventLabel, Action callback)
    {
        animevent.Add_Callback(eventLabel.ToString(), callback);
        actions.Add(new Tuple<AnimEventLabel, Action>(eventLabel,callback));
    }

    public void EraseSubscriptions()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            animevent.Remove_Callback(actions[i].Item1.ToString(), actions[i].Item2);
        }
    }

}
