using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAtenea : MonoBehaviour
{

    Animator myAnim;
    public Renderer[] renders;
    public ParticleSystem part;

    private void Awake()
    {
        myAnim = this.GetComponent<Animator>();
        myAnim.GetBehaviour<ANIM_SCRIPT_AttackHeavy>().ConfigureCallback(OnEndAnimation);

        foreach(var r in renders)
            r.enabled = false;
    }

    void OnEndAnimation()
    {
        foreach (var r in renders)
            r.enabled = false;
        part.Stop();
    }
    public void AteneaAttack()
    {
        foreach (var r in renders)
            r.enabled = true;
        part.Play();
        myAnim.SetTrigger("HeavyAttack");
    }
}
