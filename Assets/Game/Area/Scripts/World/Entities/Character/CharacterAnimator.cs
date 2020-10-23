using UnityEngine;
using System;
public class CharacterAnimator : BaseAnimator
{
    public CharacterAnimator(Animator _anim) : base(_anim) { }
    public void Move(float dirX, float dirY)
    {       
        myAnim.SetFloat("moveX", dirX);
        myAnim.SetFloat("moveY", dirY);
    }

    public void Grounded(bool b) => myAnim.SetBool("Grounded", b);
    public void Dash(bool b) => myAnim.SetBool("Dash", b);
    public void CatchProp() => myAnim.SetTrigger("catchProp");
    public void SetVerticalRoll(float x) => myAnim.SetFloat("dirX", x);
    public void SetHorizontalRoll(float y) => myAnim.SetFloat("dirY", y);
    public void Block(bool _block) => myAnim.SetBool("BeginBlock", _block);
    public void BlockSomething() => myAnim.SetTrigger("BlockSomething");
    public void Parry() => myAnim.SetTrigger("IsParry");
    public void OnAttackBegin(bool b) => myAnim.SetBool("CheckHeavy", b);
    public void NormalAttack() => myAnim.SetTrigger("NormalAttack");
    public void HeavyAttack() => myAnim.SetTrigger("HeavyAttack");
    public void SetForceHeavy() => myAnim.SetBool("ForceHeavy", true);
    public void Dead() => myAnim.SetTrigger("Death");

    public void BashDashAnim() => myAnim.SetTrigger("BashDash");
    public void ForceAttack(bool b) { myAnim.SetBool("ForceAttack", b); if(b) myAnim.SetTrigger("ForceCombo"); }

    public void Combo(bool val) => myAnim.SetBool("IsCombo", val);

    public void Falling(bool val) => myAnim.SetBool("falling", val);

    public void InCombat(int val)
    {
        if(val == 0) myAnim.SetTrigger("Env");
        else myAnim.ResetTrigger("Env");

        myAnim.SetFloat("InCombat", val);
    }

    public void SetInteract(bool val, int interactType) { myAnim.SetBool("Interact", val); myAnim.SetInteger("InteractType", interactType); }

    public void SetTypeDamge(int val) => myAnim.SetFloat("DamageType", val);

    public void OnHit() => myAnim.SetTrigger("Hit");

    public void IdleFancy() => myAnim.SetTrigger("IdleTwo");

    public void StartThrow(bool b) => myAnim.SetBool("ChargeShield", b);
    public void ThrowShield(bool b) => myAnim.SetBool("ThrowShield", b);

    public void MedusaStunStart() => myAnim.SetTrigger("MedusaStart");
    public void MedusaStunShort() => myAnim.SetTrigger("MedusaTap");
    public void MedusaStunLong() => myAnim.SetTrigger("MedusaHold");

    public void SetLightnings(bool b) => myAnim.SetBool("ThrowLightnings", b);
    public void ThrowLightningBullets() => myAnim.SetTrigger("ThrowLightningBullets");
    public void ThrowLightningOrb() => myAnim.SetTrigger("ThrowLightningOrb");

    public void BeginSpin(Action callbackEndAnimation) { myAnim.SetTrigger("BeginSpin"); myAnim.GetBehaviour<ANIM_SCRIPT_BeginSpin>().ConfigureCallback(callbackEndAnimation); }
    public void EndSpin(Action callbackEndAnimation) { myAnim.SetTrigger("EndSpin"); myAnim.GetBehaviour<ANIM_SCRIPT_EndSpin>().ConfigureCallback(callbackEndAnimation); }
    public void Stun(bool stunvalue) { myAnim.SetBool("Stun", stunvalue); }

    public void SetUpdateMode(AnimatorUpdateMode updateMode) { myAnim.updateMode = updateMode; }

}