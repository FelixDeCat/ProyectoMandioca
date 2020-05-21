using UnityEngine;
public abstract class BaseAnimator
{
    protected Animator myAnim;
    public BaseAnimator(Animator _anim) 
    {
        myAnim = _anim;
    }
}
