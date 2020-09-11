using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField] GameObject primaryButton = null;
    [SerializeField] GameObject selected;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem?.SetSelectedGameObject(null);
        StartCoroutine(SelectButtonCoroutine(primaryButton));
    }

    private void Update()
    {
        selected = eventSystem.currentSelectedGameObject;
    }

    IEnumerator SelectButtonCoroutine(GameObject button)
    {
        yield return new WaitForEndOfFrame();
        eventSystem?.SetSelectedGameObject(button);
    }

    public void SelectButton(GameObject button) => eventSystem?.SetSelectedGameObject(button);
}
