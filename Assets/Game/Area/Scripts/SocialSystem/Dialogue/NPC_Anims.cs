using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Anims : MonoBehaviour
{
    public Animator myAnim;
    Dictionary<string, Action<string>> animRegistry = new Dictionary<string, Action<string>>();

    public string npc_Name;

    //NPC_Animation_Play_Jacinta_Explainning
    //NPC_Animation_Play_Jacinta_GiveAReward
    //NPC_Animation_Play_Jacinta_Idle
    //NPC_Animation_Play_Jacinta_Cry
    //NPC_Animation_Play_Jacinta_Thanks

    public bool useCommands = true;



    private void Start()
    {
        RegisterAnimations();


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

    public void PlayAnimation(string animName)
    {
        if (animRegistry.ContainsKey(animName))
            animRegistry[animName].Invoke("");
    }

    void RegisterAnimations()
    {
        animRegistry.Add("Death", PlayDeath);
        animRegistry.Add("Resurrect", PlayResurrect);
        animRegistry.Add("Walk", StartWalk);
        animRegistry.Add("StopWalk", StopWalk);
        animRegistry.Add("RunDesesperated", StartRunDesesperate);
        animRegistry.Add("StopRunDesesperated", StopRunDesesperate);
        animRegistry.Add("RunNormal", StartRunNormal);
        animRegistry.Add("StopRunNormal", StopRunNormal);
        animRegistry.Add("FetalPos", StartFetalPos);
        animRegistry.Add("StopFetalPos", StopFetalPos);
        animRegistry.Add("Explain", Play_Explainning);
        animRegistry.Add("GiveReward", Play_GiveAReward);
        animRegistry.Add("Peek", Play_Peek);
        animRegistry.Add("EndPeek", Play_EndPeek);
        animRegistry.Add("Idle", Play_Idle);
        animRegistry.Add("Cry", Play_Cry);
        animRegistry.Add("StopCry", Stop_Crying);
        animRegistry.Add("Thanks", Play_Thanks);
        animRegistry.Add("Accept", Play_Accept);
        animRegistry.Add("Reject", Play_Reject);
        animRegistry.Add("ForceAnimation", ForcePlayAnimation);
    }

    [Range(0, 1)]
    public float death_anim_cursor;
    public void PlayDeath(string s) => myAnim.SetFloat("Death", death_anim_cursor);
    public void ForcePlayAnimation(string s) => myAnim.Play(s);
    public void PlayResurrect(string s) => myAnim.SetFloat("Death", -1);

    public void StartWalk(string s) => myAnim.SetBool("Walk", true);
    public void StopWalk(string s) => myAnim.SetBool("Walk", false);

    public void StartRunDesesperate(string s) => myAnim.SetBool("RunDesperated", true);
    public void StopRunDesesperate(string s) => myAnim.SetBool("RunDesperated", false);

    public void StartRunNormal(string s) => myAnim.SetBool("NormalRun", true);
    public void StopRunNormal(string s) => myAnim.SetBool("NormalRun", false);

    public void StartFetalPos(string s) => myAnim.SetBool("fetalPos", true);
    public void StopFetalPos(string s) => myAnim.SetBool("fetalPos", false);

    public void Play_Explainning(string s) => myAnim.SetBool("Explaining", true);
    public void Play_GiveAReward(string s) => myAnim.SetTrigger("GiveAReward");
    public void Play_Peek(string s) => myAnim.SetTrigger("peek");
    public void Play_EndPeek(string s) => myAnim.SetTrigger("endPeek");
    public void Play_Idle(string s) { }
    public void Play_Cry(string s) { myAnim.SetBool("Crying", true); }
    public void Play_Thanks(string s) { }
    public void Play_Accept(string s) { myAnim.SetBool("Explaining", false); myAnim.SetTrigger("Accepted"); }
    public void Play_Reject(string s) { myAnim.SetBool("Explaining", false); myAnim.SetTrigger("Rejected"); }

    public void Stop_Crying(string s) { myAnim.SetBool("Crying", false); }
}
