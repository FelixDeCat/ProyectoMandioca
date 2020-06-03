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

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        
       //var aux = myAnim.GetBehaviour<StateLinkerBehaviour>();
       // aux.Configure(OnEnterIdle, OnEnterIdle, () => { } ) ;

        FarFarAway();

    }

    public void FarFarAway() => gameObject.transform.position = new Vector3(999999, 0, 0);
    public void GoToHero() => gameObject.transform.position = Main.instance.GetChar().transform.position;
    public void GoToHero(float offset) => gameObject.transform.position = Main.instance.GetChar().transform.position + transform.forward * offset;

    private void Update()
    {
        var info = myAnim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Exit"))
        {
            OnEnterIdle();
        }
    }

    void empty() { }
    void OnEnterIdle()
    {
       // Debug.Log("asdsadsad");
        this.gameObject.SetActive(false);
        //apago mesh o render o lo que sea... o fade
    }

    public void Anim_Freeze()
    {
        myAnim.Play("Atenea_Freeze");
    }
    public void Anim_Arrows()
    {
        myAnim.Play("Atenea_Arrows");
    }
    public void Anim_DamageRoom()
    {
        myAnim.Play("Atenea_DamageRoom");
    }
    public void Anim_Heal()
    {
        myAnim.Play("Atenea_Buff");
    }
    public void Anim_SmiteBegin()
    {
        myAnim.Play("Atenea_SmiteBegin");
    }

}
