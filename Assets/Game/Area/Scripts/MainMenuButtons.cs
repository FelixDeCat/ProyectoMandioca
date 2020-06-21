using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField] GameObject primaryButton;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem?.SetSelectedGameObject(primaryButton);
    }

    public void SelectButton(GameObject button) => eventSystem?.SetSelectedGameObject(button);
}
