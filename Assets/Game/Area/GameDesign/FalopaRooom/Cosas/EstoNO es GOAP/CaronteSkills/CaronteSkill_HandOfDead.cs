using GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteSkill_HandOfDead : GOAP_Skills_Base
{

    [SerializeField] HandOfDead_Handler hand_pf;

    protected override void OnExecute()
    {
        SpawnHand();
    }

    void SpawnHand()
    {
        var hand = Instantiate<HandOfDead_Handler>(hand_pf);

        var characterPos = Main.instance.GetChar().Root;
        var dir = characterPos.position - owner.position;
        dir = dir.normalized;


        hand.Initialize(owner, dir, 1000);

        hand.OnGrabPlayer += KillPlayer;
        hand.OnReachedDestination += DecideIfTeleport;
    }

    public void KillPlayer(HandOfDead_Handler hand)
    {

        
    }

    void DecideIfTeleport(HandOfDead_Handler hand)
    {
        if(Vector3.Distance(owner.position, heroRoot.position) > Vector3.Distance(hand.Root.transform.position, heroRoot.position))
        {
            owner.GetComponent<Rigidbody>().MovePosition(hand.Root.transform.position);
        }

        OnFinishSkill?.Invoke();
        Destroy(hand.gameObject);
    }

    protected override void OnFixedUpdate(){}

    protected override void OnInitialize()
    {
        
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnResume()
    {
        
    }

    protected override void OnTurnOff()
    {
        
    }

    protected override void OnTurnOn()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

  
}
