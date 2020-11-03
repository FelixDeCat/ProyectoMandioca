using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosCamino : MonoBehaviour
{
    [SerializeField] ObjetiveSubscriber objective_kill_cuervo;

    public void BeginEventosCaminoCuervo()
    {
        objective_kill_cuervo.BeginObjetive(OnKillCuervo);
    }

    void OnKillCuervo()
    {
        Debug.Log("KIL CUERVO");
       
    }
}
