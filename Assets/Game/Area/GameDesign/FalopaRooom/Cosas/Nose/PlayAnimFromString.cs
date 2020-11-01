using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimFromString : MonoBehaviour
{
    [SerializeField] string animationName = "sarasa";

    Animator _anim;

    public bool useNpcAnims;
    [SerializeField] NPC_Anims npcAnims;

    private void Start()
    {
        if(useNpcAnims)
        {
            npcAnims.StartFetalPos("");
            return;
        }

        _anim = GetComponent<Animator>();
        StartAnimation();
    }

    public void StartAnimation()
    {
        _anim.Play(animationName);
    }
}
