using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericAnim
{
    Animator animator;
    string anim_name;

    public GenericAnim(Animator _animator, string _name)
    {
        animator = _animator;
        anim_name = _name;
    }

    public void Play()
    {
        animator.Play(anim_name);
    }
}


/////////////////////////////////////

public class GenericAnim<T> where T : ANIM_SCRIPT_Base
{
    Animator animator;
    string anim_name;

    T sm_behaviour = default;

    public GenericAnim(Animator _animator, string _name, Action _callback)
    {
        animator = _animator;
        anim_name = _name;

        sm_behaviour = animator.GetBehaviour<T>();
        if (sm_behaviour == null) { Debug.LogWarning("Che, no pusiste el Behaviour"); return; }
        sm_behaviour.ConfigureCallback(_callback);
    }

    public void ReconfigureCallback(Action _callback)
    {
        sm_behaviour.ConfigureCallback(_callback);
    }
    public void Play()
    {
        animator.Play(anim_name);
    }
}
