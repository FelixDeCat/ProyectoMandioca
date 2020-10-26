using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimFromString : MonoBehaviour
{
    [SerializeField] string animationName = "sarasa";

    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        _anim.Play(animationName);
    }
}
