using UnityEngine;
using System;
public class CharacterAnimator : BaseAnimator
{
    public CharacterAnimator(Animator _anim) : base(_anim) { }
    public void Move(float _speed, float dirX, float dirY)
    {
        myAnim.SetFloat("Speed", _speed);
        myAnim.SetFloat("moveX", dirX);
        myAnim.SetFloat("moveY", dirY);
    }

    public void Roll() => myAnim.SetTrigger("Roll");
    public void SetVerticalRoll(float x) => myAnim.SetFloat("dirX", x);
    public void SetHorizontalRoll(float y) => myAnim.SetFloat("dirY", y);
    public void Block(bool _block) => myAnim.SetBool("BeginBlock", _block);
    public void BlockSomething() => myAnim.SetTrigger("BlockSomething");
    public void Parry(bool b) => myAnim.SetBool("IsParry", b);
    public void OnAttackBegin(bool b) => myAnim.SetBool("AttackBegin", b);
    public void NormalAttack() => myAnim.SetTrigger("NormalAttack");
    public void HeavyAttack() => myAnim.SetBool("HeavyAttack", true);
    public void AttackAntiBug(){ myAnim.ResetTrigger("HeavyAttack"); myAnim.ResetTrigger("NormalAttack"); }

    public void BeginSpin(Action callbackEndAnimation) { myAnim.SetTrigger("BeginSpin"); myAnim.GetBehaviour<ANIM_SCRIPT_BeginSpin>().ConfigureCallback(callbackEndAnimation); }
    public void EndSpin(Action callbackEndAnimation) { myAnim.SetTrigger("EndSpin"); myAnim.GetBehaviour<ANIM_SCRIPT_EndSpin>().ConfigureCallback(callbackEndAnimation); }
    public void Stun(bool stunvalue) { myAnim.SetBool("Stun", stunvalue); }

}
