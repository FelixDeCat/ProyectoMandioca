using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamItemWorld : ItemWorld
{
    [HideInInspector]
    public ScreamPool myPool;

    public override void OnExecute(WalkingEntity collector)
    {
        CollectOnEndAnimation(collector, OnEndAnimation);
    }

    void OnEndAnimation(WalkingEntity collector)
    {
        to_collect.Invoke();
        myPool.ReturnScream(this);
        collector.OnReceiveItem(this);
        to_collect.Invoke();
    }
}
