using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//atenea tira flecha al aire***
//atenea daño en toda la room
//atenea hielo***
//mata enemigo


public class Atenea : MonoBehaviour
{
    [SerializeField] Animator myAnim;

    private void Awake() => myAnim = GetComponent<Animator>();

    public void Anim_Freeze() => myAnim.Play("Atenea_Freeze");
    public void Anim_Arrows() => myAnim.Play("Atenea_Arrows");
    public void Anim_DamageRoom() => myAnim.Play("Atenea_DamageRoom");
    public void Anim_Heal() => myAnim.Play("Atenea_Buff");
    public void Anim_SmiteBegin() => myAnim.Play("Atenea_SmiteBegin");

}
