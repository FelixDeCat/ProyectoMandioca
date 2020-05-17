using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AnimEventLabel
{
    Boss_StopLookAt,
    Boss_TakeRock,
    Boss_Throw
}

public class FastSubscriberPerState : MonoBehaviour
{
    public AnimEvent animevent;
    List<Tuple<AnimEventLabel, Action>> actions = new List<Tuple<AnimEventLabel, Action>>();

    public void SubscriveMeTo(AnimEventLabel eventLabel, Action callback)
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
