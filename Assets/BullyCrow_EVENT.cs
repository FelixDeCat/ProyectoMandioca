using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BullyCrow_EVENT : MonoBehaviour
{
    [SerializeField] ObjetiveSubscriber objective_kill_cuervo;

    [SerializeField] List<NPC_Anims> personitasEscondidas = new List<NPC_Anims>();

    public UnityEvent UE_OnKillCuervo;

    private void Start()
    {
        for (int i = 0; i < personitasEscondidas.Count; i++)
        {
            personitasEscondidas[i].GetComponent<RigidbodyPathFinder>().Initialize(personitasEscondidas[i].GetComponent<Rigidbody>());
        }
    }

    public void BeginEventosCaminoCuervo()
    {
        
        objective_kill_cuervo.BeginObjetive(OnKillCuervo);
    }

    void OnKillCuervo()
    {

        UE_OnKillCuervo.Invoke();
        for (int i = 0; i < personitasEscondidas.Count; i++)
        {
            personitasEscondidas[i].StopFetalPos("");
            personitasEscondidas[i].GetComponent<NPCFleing>().GoToPos_RunningDesesperated();
        }
       
    }
}
