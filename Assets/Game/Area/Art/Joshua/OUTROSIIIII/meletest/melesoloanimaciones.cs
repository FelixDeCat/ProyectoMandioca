using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melesoloanimaciones : MonoBehaviour
{
    public Animator anim;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 4000000000 && timer >= 30f)
        {
            anim.SetBool("Change", false);
        }
        else if (timer >= 7.5f)
        {
            anim.SetBool("Change", true);
        }
        else if (timer >= 1.8f)
        {
            anim.SetBool("Change", false);
        }
    }
}
