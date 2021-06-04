using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;
using UnityEngine.Events;

public class BrazaleteCondition : MonoBehaviour
{
    public EventCounterPredicate contrapredicado;

    public UnityEvent YaTengoBrazalete;
   

    public static BrazaleteCondition instance;
    private void Awake() => instance = this;

    public Animator MyAnim;

    bool iHaveBracelet = false;

    public static void TakeBracelet() { instance.iHaveBracelet = true; instance.YaTengoBrazalete.Invoke(); }

    private void Start()
    {
        contrapredicado.Invoke(Condicion);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
        {
            iHaveBracelet = true;
        }
    }

    bool Condicion()
    {
        return iHaveBracelet;
    }

    public void Execute()
    {
        
    }

    public void ANIM_NegateBrazalet()
    {
        MyAnim.Play("BrazaletNegate");
    }
    public void ANIM_AcceptBrazalet()
    {
        MyAnim.Play("BrazaletAccept");

    }


}
