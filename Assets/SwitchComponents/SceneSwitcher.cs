using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : LoadComponent
{
    SwitchComponent[] components;

    private void Awake()
    {
        components = GetComponentsInChildren<SwitchComponent>();
    }

    protected override IEnumerator LoadMe()
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].TurnOn();
            yield return null;
        }
    }
    protected override IEnumerator UnLoadMe()
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].TurnOff();
            yield return null;
        }
    }
}
