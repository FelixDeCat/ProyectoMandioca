using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amigoDelMele : MonoBehaviour
{
    public Animator anim;
    public float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 40000000 && timer >= 36)
        {
            anim.SetBool("GoesToMelemaco", false);
        }
        else if (timer >= 31 && timer <= 35)
        {
            anim.SetBool("GoesToMelemaco", true);
        }
        else if (timer <= 30)
        {
            anim.SetBool("GoesToMelemaco", false);
        }
    }
}
