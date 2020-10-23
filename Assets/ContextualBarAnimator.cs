using UnityEngine;
public class ContextualBarAnimator : MonoBehaviour
{
    public Animator iconAnim;
    public void StartAnim() { 
        iconAnim.SetBool("Start",true);
        iconAnim.SetBool("End", false);
    }
    public void EndAnim() {
        iconAnim.SetBool("End", true);
        iconAnim.SetBool("Start", false);
    }
}
