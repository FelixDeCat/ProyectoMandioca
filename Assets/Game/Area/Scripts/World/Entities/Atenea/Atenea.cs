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
    public AnimEvent events;

    public SkinnedMeshRenderer[] renders;
    Material[] mats;
    float timer;
    bool animdisapear;
    float current_opacity;
 
    private void Start() { 
        myAnim = GetComponent<Animator>();
        events.Add_Callback("ev_freeze", BeginDisapear);
        mats = new Material[renders.Length];
        for (int i = 0; i < renders.Length; i++)
        {
            mats[i] = renders[i].material;
        }
    }

    public void Anim_Freeze() => myAnim.Play("Atenea_Freeze");
    public void Anim_Arrows() => myAnim.Play("Atenea_Arrows");
    public void Anim_DamageRoom() => myAnim.Play("Atenea_DamageRoom");
    public void Anim_Heal() => myAnim.Play("Atenea_Buff");
    public void Anim_SmiteBegin() => myAnim.Play("Atenea_SmiteBegin");

    public void TalkOrGoodbye(bool val)
    {
        if (val)
        {
            myAnim.Play("Atenea_SmiteBegin");
        }
        else
        {
            myAnim.Play("Atenea_Freeze");
        }
    }

    [ContextMenu("asd")]
    void BeginDisapear()
    {
        animdisapear = true;
        timer = 0;
        current_opacity = mats[0].GetFloat("_OpacityIntensity");
    }

    private void Update()
    {
        if (animdisapear)
        {
            if (timer > 0)
            {
                timer = timer - 1 * Time.deltaTime;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].SetFloat("_OpacityIntensity", timer);
                }
            }
            else
            {
                animdisapear = false;
            }
        }
    }



}
