using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sellerController : MonoBehaviour
{
    public Animator anim;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 8)
        {
            timer = 0;
        }
        else if(timer > 4)
        {
            anim.SetBool("Walking", true);
        }
        else if(timer < 4)
        {
            anim.SetBool("Walking", false);
        }

    }
}
