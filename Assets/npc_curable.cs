using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_curable : MonoBehaviour
{
    public NPC_Anims anims;

    private void Start()
    {
        anims.PlayDeath("asdasdasd");
    }

    public void Resurrect()
    {
        anims.PlayResurrect("asdasdsad");
    }
}
