using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualBarAnimator : MonoBehaviour
{
    private bool _canHide;
    public Animator iconAnim;
    public void StartAnim()
    {
        CanHide();
        iconAnim.SetTrigger("Start");        
    }
    public void EndAnim()
    {
        iconAnim.SetTrigger("End");
    }
    public bool CanHide()
    {
        if (!_canHide) return true;
        else return false;
    }
}
