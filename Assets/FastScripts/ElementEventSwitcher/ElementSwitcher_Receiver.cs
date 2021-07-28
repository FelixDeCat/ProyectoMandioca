using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementSwitcher_Receiver : MonoBehaviour
{
    public string element_name;
    public GameObject parent;
    [SerializeField] Element_switcher_AuxiliarEvents sw;

    public bool startEnabled = false;

    void Start()
    {
        ElementSwitcher.Subscribe(element_name, TurnOn, TurnOff);
        if (!startEnabled) { Invoke("TurnOff", 0.2f); }
    }
    void TurnOn()
    {
        parent.SetActive(true);
        sw.AUX_TurnOn.Invoke();
    }
    void TurnOff()
    {
        parent.SetActive(false);
        sw.AUX_TurnOff.Invoke();
    }
    [System.Serializable]
    internal class Element_switcher_AuxiliarEvents
    {
        [SerializeField]internal UnityEvent AUX_TurnOn;
        [SerializeField]internal UnityEvent AUX_TurnOff;
    }
}
