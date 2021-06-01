using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;

public class Door_Lock : MonoBehaviour
{
    [SerializeField] GameObject doorClosed = null;
    [SerializeField] GameObject doorOpen = null;
    AudioSource As;
    bool opened;
    private void Start()
    {
        Debug_UI_Tools.instance.CreateToogle("Abrir puerta del boss", false, OpenDoor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Key_container>())
        {
            OpenDoor(true);
        }
    }

    string OpenDoor(bool value)
    {
        if (opened == false)
        {
            doorClosed.SetActive(false);
            doorOpen.SetActive(true);
            As = GetComponent<AudioSource>();
            As.Play();
            opened = true;
        }

        return opened ? "Abierto" : "Cerrado";
    }
}
