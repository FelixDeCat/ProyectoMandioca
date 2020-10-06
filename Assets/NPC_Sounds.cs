using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sounds : MonoBehaviour
{
    public string npc_Name = "Jacinta";
    // Start is called before the first frame update
    void Start()
    {
        Command
               .AddBranch(new CommandBranch("NPC")
                   .AddBranch(new CommandBranch("Sound")
                       .AddBranch(new CommandBranch("Play")
                           .AddBranch(new CommandBranch(npc_Name)
                           .AddLeaf(Play_Cry,"Cry")
                           ))));
    }

    public void Play_Cry(string s)
    {

    }
}
