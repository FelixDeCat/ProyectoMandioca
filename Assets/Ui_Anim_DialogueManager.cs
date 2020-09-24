using UnityEngine;
public class Ui_Anim_DialogueManager : MonoBehaviour
{
    public Animator myAnimBase;
    public Animator myAnimAuxiliar;
    void Awake() => myAnimBase = this.GetComponent<Animator>();
    public void Open(bool _open)
    { 
        if (myAnimBase == null) myAnimBase.SetBool("Open", _open);
        if (myAnimAuxiliar == null) myAnimAuxiliar.SetBool("Open", _open);
    }
}
