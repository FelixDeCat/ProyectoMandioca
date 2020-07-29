using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsMisionEnemigo : MonoBehaviour
{
    MisionEnemigo misionEnemigo;

    //public MaloMaloCinematica malomalocinematica;

    private void Awake()
    {
        misionEnemigo = FindObjectOfType<MisionEnemigo>();
       // malomalocinematica = FindObjectOfType<MaloMaloCinematica>();
    }

    public void Event_EndAttack()
    {
       // malomalocinematica.EndMaloMaloAttack();
    }


    public void InitAttack()
    {
       // malomalocinematica.Hit();
    }

    public void EndAttack()
    {

    }
    public void EndAnimation()
    {

    }


    public void Event_LlegoAlCore()
    {
        //misionEnemigo.EV_LlegoAlCore();
    }

    public void Event_ElementoRobado()
    {
       //misionEnemigo.EV_elementoRobado();
    }

    public void Destroy() {
        //FindObjectOfType<MisionEnemigo>().EndStealth(); 
    }
}
