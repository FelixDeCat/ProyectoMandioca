using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCollector : MonoBehaviour
{
    public int cant_exp_to_collect = 1;

    public void Collect()
    {
        Main.instance.GetLevelSystem().AddExperiencie(cant_exp_to_collect);
    }
}
