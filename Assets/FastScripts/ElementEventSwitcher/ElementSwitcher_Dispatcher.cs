using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSwitcher_Dispatcher : MonoBehaviour
{
    public string element_name;
    public void UE_TURN_ON() => ElementSwitcher.On(element_name);
    public void UE_TURN_OFF() => ElementSwitcher.Off(element_name);
}
