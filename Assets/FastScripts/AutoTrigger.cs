using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrigger : MonoBehaviour
{
    string scene = "";
    TriggerDispatcher trigger_Dispatcher;
    private void Start()
    {
        scene = this.gameObject.scene.name;
        trigger_Dispatcher = GetComponent<TriggerDispatcher>();
        trigger_Dispatcher.SubscribeToEnter(OnPlayerTriggerScene);
    }
    public void OnPlayerTriggerScene()
    {
        //ya tengo el evento de player piso este trigger
        //y tengo el nombre de la escena
    }
}
