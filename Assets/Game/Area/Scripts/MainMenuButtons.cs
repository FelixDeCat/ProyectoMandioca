using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField] GameObject primaryButton;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem?.SetSelectedGameObject(null);
        StartCoroutine(SelectButtonCoroutine(primaryButton));
    }

    IEnumerator SelectButtonCoroutine(GameObject button)
    {
        yield return new WaitForEndOfFrame();
        eventSystem?.SetSelectedGameObject(button);
    }

    public void SelectButton(GameObject button) => eventSystem?.SetSelectedGameObject(button);
}
