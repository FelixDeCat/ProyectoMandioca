using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimControllerHandler : MonoBehaviour
{
    public Animator animator;
    public Dictionary<string, AnimHelperBase> anims = new Dictionary<string, AnimHelperBase>();
    public void Add_ConfigureNewAnimation<T>(string _name, Action callback) where T : ANIM_SCRIPT_Base => anims.Add(_name, new AnimHelper<T>(animator, callback, _name));
    public void Add_ConfigureNewAnimation(string _name) => anims.Add(_name, new AnimHelper(animator, _name));
    public void Play(string anim_name) => anims[anim_name].Play();

    [System.Serializable]
    public class AnimHelper : AnimHelperBase
    {
        GenericAnim gen_animscript;
        public AnimHelper(Animator animator, string _name) => gen_animscript = new GenericAnim(animator, _name);
        public override void Play() => gen_animscript.Play();
    }
    [System.Serializable]
    public class AnimHelper<T> : AnimHelperBase where T : ANIM_SCRIPT_Base
    {
        GenericAnim<T> animscript;
        public AnimHelper(Animator animator, Action callback, string _name) => animscript = new GenericAnim<T>(animator, _name, callback);
        public override void Play() => animscript.Play();
    }
    public abstract class AnimHelperBase
    {
        public abstract void Play();
    }
}
