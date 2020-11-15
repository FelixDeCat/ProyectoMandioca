using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Tools;

public class MainMenuButtons : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField] GameObject primaryButton = null;

    private void Awake()
    {
        MyEventSystem.instance.SelectGameObject(primaryButton);
    }
    
}
