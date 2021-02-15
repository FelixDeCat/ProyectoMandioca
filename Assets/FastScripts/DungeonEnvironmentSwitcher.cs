using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnvironmentSwitcher : MonoBehaviour
{
    public GameObject parent;

    private void Start()
    {
        SwitchEnterDungeon.instance.Subscribe(this);
    }

    public void SwitchEnvironment(bool val)
    {
        parent.SetActive(val);
    }
}
