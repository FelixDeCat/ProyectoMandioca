using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jacintDOS : MonoBehaviour
{
    public Animator anim;
    public float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 40000000 && timer >= 24)
        {
            anim.SetBool("VeAlAmigo", false);
        }
        else if (timer >= 15)
        {
            anim.SetBool("VeAlAmigo", true);
        }
        else if (timer <= 14)
        {
            anim.SetBool("VeAlAmigo", false);
        }
    }
}
