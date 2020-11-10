using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] UnityEvent OnActivated = null;
    Action<Checkpoint> OnCheckPointActivated;
    public string sceneName = "";

    public Transform Mytranform;

    public void Initialize()
    {
        Checkpoint_Manager.instance.ConfigureCheckPoint(this, ref OnCheckPointActivated);
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<CharacterHead>();
        if (obj != null) { 
            OnCheckPointActivated?.Invoke(this);
            OnActivated.Invoke();

        }
    }
}
