using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Anims : MonoBehaviour
{
    public Animator myAnim;

    public string npc_Name;

    //NPC_Animation_Play_Jacinta_Explainning
    //NPC_Animation_Play_Jacinta_GiveAReward
    //NPC_Animation_Play_Jacinta_Idle
    //NPC_Animation_Play_Jacinta_Cry
    //NPC_Animation_Play_Jacinta_Thanks

    public bool useCommands = true;



    private void Start()
    {
        if (useCommands)
        {
            Command
               .AddBranch(new CommandBranch("NPC")
                   .AddBranch(new CommandBranch("Anim")
                       .AddBranch(new CommandBranch("Play")
                           .AddBranch(new CommandBranch(npc_Name)
                               .AddLeaf(Play_Explainning, "Explainning")
                               .AddLeaf(Play_GiveAReward, "GiveAReward")
                               .AddLeaf(Play_Idle, "Idle")
                               .AddLeaf(Play_Accept, "Accept")
                               .AddLeaf(Play_Reject, "Reject")
                               .AddLeaf(Play_Cry, "Cry")
                               .AddLeaf(Play_Thanks, "Thanks")
                               ))
                       .AddBranch(new CommandBranch("Stop")
                           .AddBranch(new CommandBranch(npc_Name)
                               .AddLeaf(Stop_Crying, "Cry")
                               ))
                       ));
        }
        //NPC_Animation_Play_Jacinta_Reject
    }

    [Range(0,1)]
    public float death_anim_cursor;
    public void PlayDeath(string s) => myAnim.SetFloat("Death", death_anim_cursor);
    public void PlayResurrect(string s) => myAnim.SetFloat("Death", -1);

    public void StartWalk(string s) => myAnim.SetBool("Walk", true);
    public void StopWalk(string s) => myAnim.SetBool("Walk", false);

    public void Play_Explainning(string s) => myAnim.SetBool("Explaining", true);
    public void Play_GiveAReward(string s) => myAnim.SetTrigger("GiveAReward");
    public void Play_Idle(string s) { }
    public void Play_Cry(string s) { myAnim.SetBool("Crying", true); }
    public void Play_Thanks(string s) { }
    public void Play_Accept(string s) { myAnim.SetBool("Explaining", false); myAnim.SetTrigger("Accepted"); }
    public void Play_Reject(string s) { myAnim.SetBool("Explaining", false); myAnim.SetTrigger("Rejected"); }

    public void Stop_Crying(string s) { myAnim.SetBool("Crying", false); } 
}
