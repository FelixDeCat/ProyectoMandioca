using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCloseUp_Camera : MonoBehaviour
{ 
    Action OnTurnOn;
    Action OnTurnOff;

    public void TurnOnSkillCamera(){OnTurnOn?.Invoke();transform.gameObject.SetActive(true);}

    public void TurnOffSkillCamera(){OnTurnOff?.Invoke();transform.gameObject.SetActive(false);} 

    public void SubscribeOnTurnOnCamera(Action listener) => OnTurnOn += listener;
    public void UnsubscribeOnTurnOnCamera(Action listener) => OnTurnOn -= listener;
    
    public void SubscribeOnTurnOffCamera(Action listener) => OnTurnOff += listener;
    public void UnsubscribeOnTurnOffCamera(Action listener) => OnTurnOff -= listener;
}
