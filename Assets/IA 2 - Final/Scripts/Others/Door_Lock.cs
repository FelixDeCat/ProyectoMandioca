using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Lock : MonoBehaviour
{
    [SerializeField] GameObject doorClosed;
    [SerializeField] GameObject doorOpen;
    AudioSource As;
    bool opened;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Key_container>())
        {
            if (opened == false)
            {
                doorClosed.SetActive(false);
                doorOpen.SetActive(true);
                As = GetComponent<AudioSource>();
                As.Play();
                opened = true;
            }
           
        }
    }
}
