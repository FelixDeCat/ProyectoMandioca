using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRecollect : MonoBehaviour
{
    public void Collect()
    {
        GoldHandler.instance.AddCollect();
    }
}
