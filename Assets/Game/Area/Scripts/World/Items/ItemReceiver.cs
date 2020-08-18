using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : ItemInterceptor
{
    protected override bool OnCollect()
    {
        return EquipedManager.instance.EquipItem(myitemworld);
    }
}
