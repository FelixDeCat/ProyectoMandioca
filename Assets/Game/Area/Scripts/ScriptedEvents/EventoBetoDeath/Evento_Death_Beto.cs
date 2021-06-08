using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Evento_Death_Beto : MonoBehaviour
{
    public CameraCinematic cameraCinematic;

    public UnityEvent OnBeginCinematic;
    public UnityEvent OnEndCinematic;
    public Animator Animator;
    public string animName = "Evento_BetoDeath";

    public void UNITY_EVENT_BetoDeath()
    {
        Animator.Play(animName);
        cameraCinematic.StartCinematic(EndCinematic);
        OnBeginCinematic.Invoke();
    }

    void EndCinematic()
    {
        OnEndCinematic.Invoke();
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.B))
        {
            FindObjectOfType<BetoBoss>().DEBUG_InstaKill();
        }
#endif
    }
}
