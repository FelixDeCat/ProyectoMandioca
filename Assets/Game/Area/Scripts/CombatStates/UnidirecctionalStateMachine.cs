using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidirecctionalStateMachine : MonoBehaviour
{
    public BaseFase[] combatStates;

    public int current = 0;

    bool canupdate;

    private void Awake()
    {
        combatStates = GetComponentsInChildren<BaseFase>();
        for (int i = 0; i < combatStates.Length; i++)
        {
            combatStates[i].Configurate(NextState);
        }
    }

    public void Begin()
    {
        combatStates[current].Init();
        canupdate = true;
    }

    public void NextState()
    {
        combatStates[current].Exit();
        current++;
        if (current >= combatStates.Length)
        {
            canupdate = false;
        }
        else
        {
            combatStates[current].Init();
        }
    }

    private void Update()
    {
        if(canupdate)
        combatStates[current].Refresh();
    }


}
