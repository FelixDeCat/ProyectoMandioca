using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimExpresions : MonoBehaviour
{

    public AnimControllerHandler anims;

    [SerializeField] string name_Hurtz = "hurtz";
    [SerializeField] string name_Shooting = "shooting";
    [SerializeField] string name_Walking = "walking";

    private void Awake()
    {
        anims.Add_ConfigureNewAnimation(name_Hurtz);
        anims.Add_ConfigureNewAnimation(name_Shooting);
        anims.Add_ConfigureNewAnimation(name_Walking);
    }

    public void Anim_Hurtz() => anims.Play(name_Hurtz);
    public void Anim_Shooting()
    {
        //anims.Play(name_Shooting);
    }
    public void Anim_Walking()
    {
        anims.Play(name_Walking);
    }
}
