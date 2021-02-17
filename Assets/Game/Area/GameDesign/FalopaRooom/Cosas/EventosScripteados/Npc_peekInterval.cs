using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_peekInterval : MonoBehaviour, IPauseable
{

    [SerializeField] float timeIntervalToPeek = 4;
    [SerializeField] float timeIntervalPeeking = 3;
    [SerializeField] NPC_Anims anim = null;

    bool peeking = false;
    bool canUpdate;

    public float _count;

    public void Pause()
    {
        canUpdate = false;
    }

    public void Resume()
    {
        canUpdate = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        anim.StartFetalPos("");
        canUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canUpdate) return;

        _count += Time.deltaTime;

        if(_count >= timeIntervalToPeek && !peeking)
        {
            peeking = true;
            anim.Play_Peek("");
        }

        if(_count >= timeIntervalPeeking)
        {
            _count = 0;
            peeking = false;
            anim.Play_EndPeek("");
        }
    }
}
