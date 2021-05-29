using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExposeLowLoadEvents : MonoBehaviour
{
    public UnityEvent Turn_ON_LOW;
    public UnityEvent Turn_OFF_LOW;
    LocalSceneHandler handler;

    HashSet<LoadComponent> components;

    public Transform contentParent;
    SceneSwitcher switcher;

    private void Start()
    {
        switcher = FindObjectOfType<SceneSwitcher>();
        components = contentParent.GetComponentsInChildren<LoadComponent>().ToHashSet();
        components.Add(switcher);
        handler = GetComponent<LocalSceneHandler>();
        handler.SubscribeEventsLOWObjects(On, Off);
    }

    public void On()
    {
        Turn_ON_LOW.Invoke();
        foreach (var c in components)
        {
            StartCoroutine(c.Load());
        }
    }

    public void Off()
    {
        Turn_OFF_LOW.Invoke();
        foreach (var c in components)
        {
            StartCoroutine(c.Unload());
        }
    }
}
